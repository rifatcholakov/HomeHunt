using HomeHunt.Service.Data.Agents;
using HomeHunt.Service.Data.Agents.Request;
using HomeHunt.Service.Data.Agents.Response;
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
            IEnumerable<GetAllAgentsResponseData> response = await _agentService.GetAllAgentsAsync();

            List<AgentListViewModel> agents = response
                .Select(a => new AgentListViewModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    AgencyName = a.AgencyName,
                    Email = a.Email,
                    Phone = a.Phone,
                    PropertyCount = a.PropertyCount
                })
                .ToList();

            return View(agents);
        }

        public async Task<IActionResult> Details(int id)
        {
            GetAgentDetailsResponseData? agent = await _agentService.GetAgentDetailsAsync(id);

            if (agent == null)
            {
                return NotFound();
            }

            AgentDetailsViewModel model = new AgentDetailsViewModel
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                AgencyName = agent.AgencyName,
                Email = agent.Email,
                Phone = agent.Phone,
                PropertyCount = agent.PropertyCount,
                ForSaleCount = agent.ForSaleCount,
                ForRentCount = agent.ForRentCount,
                Properties = agent.Properties
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

        public async Task<IActionResult> Edit(int id)
        {
            GetAgentForEditResponseData? agent = await _agentService.GetAgentForEditAsync(id);

            if (agent == null)
            {
                return NotFound();
            }

            AgentFormViewModel model = new AgentFormViewModel
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                Email = agent.Email,
                Phone = agent.Phone,
                AgencyName = agent.AgencyName
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AgentFormViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _agentService.EmailExistsAsync(model.Email, id))
            {
                ModelState.AddModelError(nameof(model.Email), "An agent with this email already exists.");
            }

            if (await _agentService.PhoneExistsAsync(model.Phone, id))
            {
                ModelState.AddModelError(nameof(model.Phone), "An agent with this phone number already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool updated = await _agentService.UpdateAgentAsync(id, new UpdateAgentRequestData
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                AgencyName = model.AgencyName
            });

            if (!updated)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "The agent was updated.";
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            GetAgentDetailsResponseData? agent = await _agentService.GetAgentDetailsAsync(id);

            if (agent == null)
            {
                return NotFound();
            }

            AgentDetailsViewModel model = new AgentDetailsViewModel
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                AgencyName = agent.AgencyName,
                Email = agent.Email,
                Phone = agent.Phone,
                PropertyCount = agent.PropertyCount,
                ForSaleCount = agent.ForSaleCount,
                ForRentCount = agent.ForRentCount,
                Properties = agent.Properties
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
            bool deleted = await _agentService.DeleteAgentAsync(id);

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

            await _agentService.CreateAgentAsync(new CreateAgentRequestData
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                AgencyName = model.AgencyName
            });

            TempData["SuccessMessage"] = "The agent was created.";
            return RedirectToAction(nameof(Index));
        }
    }
}
