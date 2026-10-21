using HomeHunt.Service.Data.Locations;
using HomeHunt.Service.Data.Locations.Request;
using HomeHunt.Service.Data.Locations.Response;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeHunt.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<GetAllLocationsResponseData> response = await _locationService.GetAllLocationsAsync();

            List<LocationListViewModel> locations = response
                .Select(l => new LocationListViewModel
                {
                    Id = l.Id,
                    City = l.City,
                    Neighborhood = l.Neighborhood,
                    PostalCode = l.PostalCode,
                    PropertyCount = l.PropertyCount
                })
                .ToList();

            return View(locations);
        }

        public async Task<IActionResult> Details(int id)
        {
            GetLocationDetailsResponseData? location = await _locationService.GetLocationDetailsAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            LocationDetailsViewModel model = new LocationDetailsViewModel
            {
                Id = location.Id,
                City = location.City,
                Neighborhood = location.Neighborhood,
                PostalCode = location.PostalCode,
                PropertyCount = location.PropertyCount,
                ForSaleCount = location.ForSaleCount,
                ForRentCount = location.ForRentCount,
                Properties = location.Properties
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
                    .ToList()
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View(new LocationFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(LocationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _locationService.LocationExistsAsync(model.City, model.Neighborhood))
            {
                ModelState.AddModelError(nameof(model.Neighborhood), "This location already exists.");
                return View(model);
            }

            string? cityOfPostalCode = await _locationService.GetCityByPostalCodeAsync(model.PostalCode);

            if (cityOfPostalCode != null && !string.Equals(cityOfPostalCode, model.City, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(model.PostalCode), $"This postal code belongs to {cityOfPostalCode}.");
                return View(model);
            }

            await _locationService.CreateLocationAsync(new CreateLocationRequestData
            {
                City = model.City,
                Neighborhood = model.Neighborhood,
                PostalCode = model.PostalCode
            });

            TempData["SuccessMessage"] = "The location was created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            GetLocationDetailsResponseData? location = await _locationService.GetLocationDetailsAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            LocationDetailsViewModel model = new LocationDetailsViewModel
            {
                Id = location.Id,
                City = location.City,
                Neighborhood = location.Neighborhood,
                PostalCode = location.PostalCode,
                PropertyCount = location.PropertyCount,
                ForSaleCount = location.ForSaleCount,
                ForRentCount = location.ForRentCount,
                Properties = location.Properties
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
                    .ToList()
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleted = await _locationService.DeleteLocationAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "The location and all of its properties were deleted.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            GetLocationForEditResponseData? location = await _locationService.GetLocationForEditAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            LocationFormViewModel model = new LocationFormViewModel
            {
                Id = location.Id,
                City = location.City,
                Neighborhood = location.Neighborhood,
                PostalCode = location.PostalCode
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, LocationFormViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _locationService.LocationExistsAsync(model.City, model.Neighborhood, id))
            {
                ModelState.AddModelError(nameof(model.Neighborhood), "This location already exists.");
                return View(model);
            }

            string? cityOfPostalCode = await _locationService.GetCityByPostalCodeAsync(model.PostalCode, id);

            if (cityOfPostalCode != null && !string.Equals(cityOfPostalCode, model.City, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(model.PostalCode), $"This postal code belongs to {cityOfPostalCode}.");
                return View(model);
            }

            bool updated = await _locationService.UpdateLocationAsync(id, new UpdateLocationRequestData
            {
                City = model.City,
                Neighborhood = model.Neighborhood,
                PostalCode = model.PostalCode
            });

            if (!updated)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "The location was updated.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
