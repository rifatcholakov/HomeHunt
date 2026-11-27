using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HomeHunt.Mapping;
using HomeHunt.Validation;
using HomeHunt.Service.Data;
using HomeHunt.Service.Data.Agents;
using HomeHunt.Service.Data.Agents.Response;
using HomeHunt.Service.Data.Locations;
using HomeHunt.Service.Data.Locations.Response;
using HomeHunt.Service.Data.Properties;
using HomeHunt.Service.Data.Properties.Request;
using HomeHunt.Service.Data.Properties.Response;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeHunt.Common;
using static HomeHunt.Common.Messages.Property;

namespace HomeHunt.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly ILocationService _locationService;
        private readonly IAgentService _agentService;
        private readonly IAuthorizationService _authorizationService;

        public PropertiesController(
            IPropertyService propertyService,
            ILocationService locationService,
            IAgentService agentService,
            IAuthorizationService authorizationService)
        {
            _propertyService = propertyService;
            _locationService = locationService;
            _agentService = agentService;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Index(PropertySearchViewModel searchModel)
        {
            if (searchModel.MinPrice != null && searchModel.MaxPrice != null && searchModel.MinPrice > searchModel.MaxPrice)
            {
                ModelState.AddModelError(nameof(searchModel.MinPrice), MinPriceGreaterThanMaxMessage);
            }

            IEnumerable<string> cities = await _locationService.GetCitiesAsync();
            searchModel.Cities = cities.ToList();

            if (ModelState.IsValid)
            {
                PagedResponseData<GetPropertiesResponseData> response = await _propertyService
                    .SearchPropertiesAsync(new SearchPropertiesRequestData
                    {
                        City = searchModel.City,
                        PropertyType = searchModel.PropertyType,
                        ListingType = searchModel.ListingType,
                        MinPrice = searchModel.MinPrice,
                        MaxPrice = searchModel.MaxPrice,
                        MinBedrooms = searchModel.MinBedrooms,
                        MinArea = searchModel.MinArea,
                        SortBy = searchModel.SortBy,
                        Page = searchModel.Page
                    });

                searchModel.Page = response.Page;
                searchModel.TotalPages = response.TotalPages;
                searchModel.TotalCount = response.TotalCount;

                searchModel.Properties = response.Items
                    .Select(p => p.ToViewModel())
                    .ToList();
            }

            return View(searchModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            GetPropertyDetailsResponseData? property = await _propertyService.GetPropertyDetailsAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            PropertyDetailsViewModel model = property.ToViewModel();

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> GetStartedListing()
        {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, Policies.CanManageProperties);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Create));
            }

            return RedirectToPage(Routes.UpgradeToAgentPage, new { area = Routes.IdentityArea });
        }

        [Authorize(Policy = Policies.CanManageProperties)]
        public async Task<IActionResult> Create()
        {
            PropertyFormViewModel model = new PropertyFormViewModel();

            if (!await AssignOwnAgentAsync(model))
            {
                return Forbid();
            }

            await PopulateLocationAndAgentOptionsAsync(model);

            return View(model);
        }

        [Authorize(Policy = Policies.CanManageProperties)]
        [HttpPost]
        public async Task<IActionResult> Create(PropertyFormViewModel model)
        {
            if (!await AssignOwnAgentAsync(model))
            {
                return Forbid();
            }

            await ValidateFormAsync(model);

            if (!ModelState.IsValid)
            {
                await PopulateLocationAndAgentOptionsAsync(model);
                return View(model);
            }

            OperationResponseData result = await _propertyService.CreatePropertyAsync(model.ToCreateRequest(), HttpContext.RequestAborted);

            if (!result.Succeeded)
            {
                AddErrors(result);
                await PopulateLocationAndAgentOptionsAsync(model);
                return View(model);
            }

            TempData[TempDataKeys.SuccessMessage] = CreatedMessage;
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = Policies.CanManageProperties)]
        public async Task<IActionResult> Edit(int id)
        {
            IActionResult? denied = await CheckCanModifyAsync(id);

            if (denied != null)
            {
                return denied;
            }

            GetPropertyForEditResponseData? property = await _propertyService.GetPropertyForEditAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            PropertyFormViewModel model = new PropertyFormViewModel
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Price = property.Price,
                Area = property.Area,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                PropertyType = property.PropertyType,
                ListingType = property.ListingType,
                Status = property.Status,
                Images = PropertyImageFormMapper.ToViewModels(property.Images),
                LocationId = property.LocationId,
                AgentId = property.AgentId
            };

            await PopulateLocationAndAgentOptionsAsync(model);

            return View(model);
        }

        [Authorize(Policy = Policies.CanManageProperties)]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, PropertyFormViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            IActionResult? denied = await CheckCanModifyAsync(id);

            if (denied != null)
            {
                return denied;
            }

            if (!await AssignOwnAgentAsync(model))
            {
                return Forbid();
            }

            await ValidateFormAsync(model);

            if (!ModelState.IsValid)
            {
                await PopulateLocationAndAgentOptionsAsync(model);
                await LoadExistingImagesAsync(model);
                return View(model);
            }

            OperationResponseData result = await _propertyService.UpdatePropertyAsync(id, model.ToUpdateRequest(), HttpContext.RequestAborted);

            if (result.NotFound)
            {
                return NotFound();
            }

            if (!result.Succeeded)
            {
                AddErrors(result);
                await PopulateLocationAndAgentOptionsAsync(model);
                await LoadExistingImagesAsync(model);
                return View(model);
            }

            TempData[TempDataKeys.SuccessMessage] = UpdatedMessage;
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Policy = Policies.CanManageProperties)]
        public async Task<IActionResult> Delete(int id)
        {
            IActionResult? denied = await CheckCanModifyAsync(id);

            if (denied != null)
            {
                return denied;
            }

            GetPropertyDetailsResponseData? property = await _propertyService.GetPropertyDetailsAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            PropertyDetailsViewModel model = property.ToViewModel();

            return View(model);
        }

        [Authorize(Policy = Policies.CanManageProperties)]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            IActionResult? denied = await CheckCanModifyAsync(id);

            if (denied != null)
            {
                return denied;
            }

            bool deleted = await _propertyService.DeletePropertyAsync(id, HttpContext.RequestAborted);

            if (!deleted)
            {
                return NotFound();
            }

            TempData[TempDataKeys.SuccessMessage] = DeletedMessage;
            return RedirectToAction(nameof(Index));
        }

        private async Task ValidateFormAsync(PropertyFormViewModel model)
        {
            if (model.LocationId != null && !await _locationService.LocationExistsAsync(model.LocationId.Value))
            {
                ModelState.AddModelError(nameof(model.LocationId), LocationInvalidMessage);
            }

            if (model.AgentId != null && !await _agentService.AgentExistsAsync(model.AgentId.Value))
            {
                ModelState.AddModelError(nameof(model.AgentId), AgentInvalidMessage);
            }

            foreach (string error in await ImageFileValidator.ValidateAsync(model.NewImages, HttpContext.RequestAborted))
            {
                ModelState.AddModelError(nameof(model.NewImages), error);
            }
        }

        private void AddErrors(OperationResponseData result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private async Task LoadExistingImagesAsync(PropertyFormViewModel model)
        {
            GetPropertyForEditResponseData? property = await _propertyService.GetPropertyForEditAsync(model.Id);

            model.Images = PropertyImageFormMapper.ToViewModels(property?.Images ?? new List<GetPropertyImageResponseData>());
        }

        private async Task<IActionResult?> CheckCanModifyAsync(int propertyId)
        {
            string? ownerUserId = await _propertyService.GetOwnerUserIdAsync(propertyId);

            if (ownerUserId == null)
            {
                return NotFound();
            }

            AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, ownerUserId, Policies.CanModifyProperty);

            return result.Succeeded ? null : Forbid();
        }

        private async Task<bool> AssignOwnAgentAsync(PropertyFormViewModel model)
        {
            if (User.IsInRole(Roles.Administrator))
            {
                return true;
            }

            int? ownAgentId = await _agentService.GetAgentIdByUserIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (ownAgentId == null)
            {
                return false;
            }

            model.AgentId = ownAgentId;
            ModelState.Remove(nameof(model.AgentId));

            return true;
        }

        private async Task PopulateLocationAndAgentOptionsAsync(PropertyFormViewModel model)
        {
            model.CanChooseAgent = User.IsInRole(Roles.Administrator);

            IEnumerable<GetLocationOptionsResponseData> locations = await _locationService.GetLocationOptionsAsync();
            IEnumerable<GetAgentOptionsResponseData> agents = model.CanChooseAgent
                ? await _agentService.GetAgentOptionsAsync()
                : Enumerable.Empty<GetAgentOptionsResponseData>();

            model.Locations = locations
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Text
                })
                .ToList();

            model.Agents = agents
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Text
                })
                .ToList();
        }
    }
}
