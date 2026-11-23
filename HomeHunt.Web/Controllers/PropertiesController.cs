using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
                SearchPropertiesResponseData response = await _propertyService
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
                    .Select(p => new PropertyListViewModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        City = p.City,
                        Neighborhood = p.Neighborhood,
                        Price = p.Price,
                        Area = p.Area,
                        Bedrooms = p.Bedrooms,
                        Bathrooms = p.Bathrooms,
                        PropertyType = p.PropertyType,
                        ListingType = p.ListingType,
                        Status = p.Status,
                        ImageUrl = p.ImageUrl,
                        OwnerUserId = p.OwnerUserId
                    })
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

            PropertyDetailsViewModel model = new PropertyDetailsViewModel
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
                ImageUrl = property.ImageUrl,
                CreatedOn = property.CreatedOn,
                LocationId = property.LocationId,
                City = property.City,
                Neighborhood = property.Neighborhood,
                AgentId = property.AgentId,
                OwnerUserId = property.OwnerUserId,
                AgentFirstName = property.AgentFirstName,
                AgentLastName = property.AgentLastName,
                AgentEmail = property.AgentEmail,
                AgentPhone = property.AgentPhone
            };

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

            if (model.LocationId != null && !await _locationService.LocationExistsAsync(model.LocationId.Value))
            {
                ModelState.AddModelError(nameof(model.LocationId), LocationInvalidMessage);
            }

            if (model.AgentId != null && !await _agentService.AgentExistsAsync(model.AgentId.Value))
            {
                ModelState.AddModelError(nameof(model.AgentId), AgentInvalidMessage);
            }

            if (!ModelState.IsValid)
            {
                await PopulateLocationAndAgentOptionsAsync(model);
                return View(model);
            }

            await _propertyService.CreatePropertyAsync(new CreatePropertyRequestData
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price ?? 0,
                Area = model.Area ?? 0,
                Bedrooms = model.Bedrooms,
                Bathrooms = model.Bathrooms,
                PropertyType = model.PropertyType,
                ListingType = model.ListingType,
                Status = model.Status,
                ImageUrl = model.ImageUrl,
                LocationId = model.LocationId ?? 0,
                AgentId = model.AgentId ?? 0
            });

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
                ImageUrl = property.ImageUrl,
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

            if (model.LocationId != null && !await _locationService.LocationExistsAsync(model.LocationId.Value))
            {
                ModelState.AddModelError(nameof(model.LocationId), LocationInvalidMessage);
            }

            if (model.AgentId != null && !await _agentService.AgentExistsAsync(model.AgentId.Value))
            {
                ModelState.AddModelError(nameof(model.AgentId), AgentInvalidMessage);
            }

            if (!ModelState.IsValid)
            {
                await PopulateLocationAndAgentOptionsAsync(model);
                return View(model);
            }

            bool updated = await _propertyService.UpdatePropertyAsync(id, new UpdatePropertyRequestData
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price ?? 0,
                Area = model.Area ?? 0,
                Bedrooms = model.Bedrooms,
                Bathrooms = model.Bathrooms,
                PropertyType = model.PropertyType,
                ListingType = model.ListingType,
                Status = model.Status,
                ImageUrl = model.ImageUrl,
                LocationId = model.LocationId ?? 0,
                AgentId = model.AgentId ?? 0
            });

            if (!updated)
            {
                return NotFound();
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

            PropertyDetailsViewModel model = new PropertyDetailsViewModel
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
                ImageUrl = property.ImageUrl,
                CreatedOn = property.CreatedOn,
                LocationId = property.LocationId,
                City = property.City,
                Neighborhood = property.Neighborhood,
                AgentId = property.AgentId,
                OwnerUserId = property.OwnerUserId,
                AgentFirstName = property.AgentFirstName,
                AgentLastName = property.AgentLastName,
                AgentEmail = property.AgentEmail,
                AgentPhone = property.AgentPhone
            };

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

            bool deleted = await _propertyService.DeletePropertyAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            TempData[TempDataKeys.SuccessMessage] = DeletedMessage;
            return RedirectToAction(nameof(Index));
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
