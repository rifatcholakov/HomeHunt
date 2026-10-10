using HomeHunt.Services;
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
            List<LocationListViewModel> locations = await _locationService.GetAllAsync();
            return View(locations);
        }

        public async Task<IActionResult> Details(int id)
        {
            LocationDetailsViewModel? location = await _locationService.GetByIdAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return View(location);
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

            await _locationService.CreateAsync(model);

            TempData["SuccessMessage"] = "The location was created.";
            return RedirectToAction(nameof(Index));
        }
    }
}
