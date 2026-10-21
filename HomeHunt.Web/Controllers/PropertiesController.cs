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

namespace HomeHunt.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly ILocationService _locationService;
        private readonly IAgentService _agentService;

        public PropertiesController(
            IPropertyService propertyService,
            ILocationService locationService,
            IAgentService agentService)
        {
            _propertyService = propertyService;
            _locationService = locationService;
            _agentService = agentService;
        }

        public async Task<IActionResult> Index(PropertySearchViewModel searchModel)
        {
            if (searchModel.MinPrice != null && searchModel.MaxPrice != null && searchModel.MinPrice > searchModel.MaxPrice)
            {
                ModelState.AddModelError(nameof(searchModel.MinPrice), "Minimum price cannot be greater than maximum price.");
            }

            IEnumerable<string> cities = await _locationService.GetCitiesAsync();
            searchModel.Cities = cities.ToList();

            if (ModelState.IsValid)
            {
                IEnumerable<GetPropertiesResponseData> response = await _propertyService
                    .SearchPropertiesAsync(new SearchPropertiesRequestData
                    {
                        City = searchModel.City,
                        PropertyType = searchModel.PropertyType,
                        ListingType = searchModel.ListingType,
                        MinPrice = searchModel.MinPrice,
                        MaxPrice = searchModel.MaxPrice,
                        MinBedrooms = searchModel.MinBedrooms,
                        MinArea = searchModel.MinArea,
                        SortBy = searchModel.SortBy
                    });

                searchModel.Properties = response
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
                        ImageUrl = p.ImageUrl
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
                AgentFirstName = property.AgentFirstName,
                AgentLastName = property.AgentLastName,
                AgentEmail = property.AgentEmail,
                AgentPhone = property.AgentPhone
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            PropertyFormViewModel model = new PropertyFormViewModel();
            await FillDropdownsAsync(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PropertyFormViewModel model)
        {
            if (model.LocationId != null && !await _locationService.LocationExistsAsync(model.LocationId.Value))
            {
                ModelState.AddModelError(nameof(model.LocationId), "Please select a valid location.");
            }

            if (model.AgentId != null && !await _agentService.AgentExistsAsync(model.AgentId.Value))
            {
                ModelState.AddModelError(nameof(model.AgentId), "Please select a valid agent.");
            }

            if (!ModelState.IsValid)
            {
                await FillDropdownsAsync(model);
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

            TempData["SuccessMessage"] = "The property was created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
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

            await FillDropdownsAsync(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PropertyFormViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (model.LocationId != null && !await _locationService.LocationExistsAsync(model.LocationId.Value))
            {
                ModelState.AddModelError(nameof(model.LocationId), "Please select a valid location.");
            }

            if (model.AgentId != null && !await _agentService.AgentExistsAsync(model.AgentId.Value))
            {
                ModelState.AddModelError(nameof(model.AgentId), "Please select a valid agent.");
            }

            if (!ModelState.IsValid)
            {
                await FillDropdownsAsync(model);
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

            TempData["SuccessMessage"] = "The property was updated.";
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Delete(int id)
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
                AgentFirstName = property.AgentFirstName,
                AgentLastName = property.AgentLastName,
                AgentEmail = property.AgentEmail,
                AgentPhone = property.AgentPhone
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleted = await _propertyService.DeletePropertyAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "The property was deleted.";
            return RedirectToAction(nameof(Index));
        }

        private async Task FillDropdownsAsync(PropertyFormViewModel model)
        {
            IEnumerable<GetLocationOptionsResponseData> locations = await _locationService.GetLocationOptionsAsync();
            IEnumerable<GetAgentOptionsResponseData> agents = await _agentService.GetAgentOptionsAsync();

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
