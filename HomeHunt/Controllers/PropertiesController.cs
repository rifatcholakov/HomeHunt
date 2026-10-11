using HomeHunt.Services;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> Index()
        {
            List<PropertyListViewModel> properties = await _propertyService.GetAllAsync();
            return View(properties);
        }

        public async Task<IActionResult> Details(int id)
        {
            PropertyDetailsViewModel? property = await _propertyService.GetByIdAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
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
            if (model.LocationId != null && !await _locationService.ExistsAsync(model.LocationId.Value))
            {
                ModelState.AddModelError(nameof(model.LocationId), "Please select a valid location.");
            }

            if (model.AgentId != null && !await _agentService.ExistsAsync(model.AgentId.Value))
            {
                ModelState.AddModelError(nameof(model.AgentId), "Please select a valid agent.");
            }

            if (!ModelState.IsValid)
            {
                await FillDropdownsAsync(model);
                return View(model);
            }

            await _propertyService.CreateAsync(model);

            TempData["SuccessMessage"] = "The property was created.";
            return RedirectToAction(nameof(Index));
        }

        private async Task FillDropdownsAsync(PropertyFormViewModel model)
        {
            model.Locations = await _locationService.GetOptionsAsync();
            model.Agents = await _agentService.GetOptionsAsync();
        }
    }
}
