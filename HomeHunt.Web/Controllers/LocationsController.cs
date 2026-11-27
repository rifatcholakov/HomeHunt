using HomeHunt.Mapping;
using Microsoft.AspNetCore.Authorization;
using HomeHunt.Service.Data;
using HomeHunt.Service.Data.Locations;
using HomeHunt.Service.Data.Locations.Request;
using HomeHunt.Service.Data.Locations.Response;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using HomeHunt.Common;
using static HomeHunt.Common.Messages.Location;

namespace HomeHunt.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            PagedResponseData<GetAllLocationsResponseData> response = await _locationService.GetAllLocationsAsync(page, DisplayConstants.TablePageSize);

            List<LocationListViewModel> locations = response.Items
                .Select(l => new LocationListViewModel
                {
                    Id = l.Id,
                    City = l.City,
                    Neighborhood = l.Neighborhood,
                    PostalCode = l.PostalCode,
                    PropertyCount = l.PropertyCount
                })
                .ToList();

            return View(new PagedViewModel<LocationListViewModel>
            {
                Items = locations,
                Pagination = new PaginationViewModel { CurrentPage = response.Page, TotalPages = response.TotalPages }
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            GetLocationDetailsResponseData? location = await _locationService.GetLocationDetailsAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            LocationDetailsViewModel model = location.ToDetailsViewModel();

            return View(model);
        }

        [Authorize(Policy = Policies.CanManageLocations)]
        public IActionResult Create()
        {
            return View(new LocationFormViewModel());
        }

        [Authorize(Policy = Policies.CanManageLocations)]
        [HttpPost]
        public async Task<IActionResult> Create(LocationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _locationService.LocationExistsAsync(model.City, model.Neighborhood))
            {
                ModelState.AddModelError(nameof(model.Neighborhood), AlreadyExistsMessage);
                return View(model);
            }

            string? cityOfPostalCode = await _locationService.GetCityByPostalCodeAsync(model.PostalCode);

            if (cityOfPostalCode != null && !string.Equals(cityOfPostalCode, model.City, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(model.PostalCode), string.Format(PostalCodeBelongsToMessage, cityOfPostalCode));
                return View(model);
            }

            await _locationService.CreateLocationAsync(new SaveLocationRequestData
            {
                City = model.City,
                Neighborhood = model.Neighborhood,
                PostalCode = model.PostalCode
            });

            TempData[TempDataKeys.SuccessMessage] = CreatedMessage;
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = Policies.CanManageLocations)]
        public async Task<IActionResult> Delete(int id)
        {
            GetLocationDetailsResponseData? location = await _locationService.GetLocationDetailsAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            LocationDetailsViewModel model = location.ToDetailsViewModel();

            return View(model);
        }

        [Authorize(Policy = Policies.CanManageLocations)]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleted = await _locationService.DeleteLocationAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            TempData[TempDataKeys.SuccessMessage] = DeletedMessage;
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = Policies.CanManageLocations)]
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

        [Authorize(Policy = Policies.CanManageLocations)]
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
                ModelState.AddModelError(nameof(model.Neighborhood), AlreadyExistsMessage);
                return View(model);
            }

            string? cityOfPostalCode = await _locationService.GetCityByPostalCodeAsync(model.PostalCode, id);

            if (cityOfPostalCode != null && !string.Equals(cityOfPostalCode, model.City, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(model.PostalCode), string.Format(PostalCodeBelongsToMessage, cityOfPostalCode));
                return View(model);
            }

            bool updated = await _locationService.UpdateLocationAsync(id, new SaveLocationRequestData
            {
                City = model.City,
                Neighborhood = model.Neighborhood,
                PostalCode = model.PostalCode
            });

            if (!updated)
            {
                return NotFound();
            }

            TempData[TempDataKeys.SuccessMessage] = UpdatedMessage;
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
