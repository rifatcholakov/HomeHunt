using HomeHunt.Services;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeHunt.Controllers
{
    public class AgentsController : Controller
    {
        private readonly IAgentService _agentService;

        public AgentsController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        public async Task<IActionResult> Index()
        {
            List<AgentListViewModel> agents = await _agentService.GetAllAsync();
            return View(agents);
        }

        public async Task<IActionResult> Details(int id)
        {
            AgentDetailsViewModel? agent = await _agentService.GetByIdAsync(id);

            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        public async Task<IActionResult> Delete(int id)
        {
            AgentDetailsViewModel? agent = await _agentService.GetByIdAsync(id);

            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleted = await _agentService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "The agent and all of their properties were deleted.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View(new AgentFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AgentFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _agentService.EmailExistsAsync(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), "An agent with this email already exists.");
            }

            if (await _agentService.PhoneExistsAsync(model.Phone))
            {
                ModelState.AddModelError(nameof(model.Phone), "An agent with this phone number already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _agentService.CreateAsync(model);

            TempData["SuccessMessage"] = "The agent was created.";
            return RedirectToAction(nameof(Index));
        }
    }
}
