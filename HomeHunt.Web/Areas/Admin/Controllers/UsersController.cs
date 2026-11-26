using System.Security.Claims;
using HomeHunt.Common;
using HomeHunt.Service.Data;
using HomeHunt.Service.Data.Agents;
using HomeHunt.Service.Data.Users;
using HomeHunt.Service.Data.Users.Request;
using HomeHunt.Service.Data.Users.Response;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static HomeHunt.Common.Messages.Admin;
using static HomeHunt.Common.Messages.Agent;

namespace HomeHunt.Areas.Admin.Controllers
{
    public class UsersController : AdminControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAgentService _agentService;

        public UsersController(IUserService userService, IAgentService agentService)
        {
            _userService = userService;
            _agentService = agentService;
        }

        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            PagedResponseData<GetUserResponseData> response = await _userService.GetUsersAsync(search, page, DisplayConstants.TablePageSize);

            List<UserListViewModel> users = response.Items
                .Select(CreateViewModel)
                .ToList();

            return View(new UserIndexViewModel
            {
                Search = search,
                Items = users,
                Pagination = new PaginationViewModel { CurrentPage = response.Page, TotalPages = response.TotalPages }
            });
        }

        public async Task<IActionResult> ChangeRole(string id)
        {
            if (IsCurrentUser(id))
            {
                return RedirectWithError(CannotChangeOwnAccountMessage);
            }

            GetUserResponseData? user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            ChangeRoleViewModel model = CreateRoleForm(user);
            model.Input.NewRole = model.CurrentRole;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string id, ChangeRoleInputModel input)
        {
            if (IsCurrentUser(id))
            {
                return RedirectWithError(CannotChangeOwnAccountMessage);
            }

            GetUserResponseData? user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            ChangeRoleViewModel model = CreateRoleForm(user);
            model.Input = input;

            bool needsAgentDetails = input.NewRole == Roles.Agent && !model.HasAgentProfile;

            if (!needsAgentDetails)
            {
                ModelState.Remove(InputField(nameof(input.FirstName)));
                ModelState.Remove(InputField(nameof(input.LastName)));
                ModelState.Remove(InputField(nameof(input.Phone)));
                ModelState.Remove(InputField(nameof(input.AgencyName)));
            }
            else if (await _agentService.PhoneExistsAsync(input.Phone))
            {
                ModelState.AddModelError(InputField(nameof(input.Phone)), PhoneTakenMessage);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserOperationResponseData result = await _userService.ChangeRoleAsync(new ChangeUserRoleRequestData
            {
                UserId = id,
                NewRole = input.NewRole,
                FirstName = input.FirstName,
                LastName = input.LastName,
                Phone = input.Phone,
                AgencyName = input.AgencyName
            });

            if (!result.Succeeded)
            {
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(model);
            }

            TempData[TempDataKeys.SuccessMessage] = RoleChangedMessage;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (IsCurrentUser(id))
            {
                return RedirectWithError(CannotChangeOwnAccountMessage);
            }

            GetUserResponseData? user = await _userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            UserListViewModel model = CreateViewModel(user);

            if (model.IsAdministrator)
            {
                return RedirectWithError(CannotDeleteAdministratorMessage);
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (IsCurrentUser(id))
            {
                return RedirectWithError(CannotChangeOwnAccountMessage);
            }

            UserOperationResponseData result = await _userService.DeleteUserAsync(id);

            if (!result.Succeeded)
            {
                return RedirectWithError(string.Join(" ", result.Errors));
            }

            TempData[TempDataKeys.SuccessMessage] = UserDeletedMessage;
            return RedirectToAction(nameof(Index));
        }

        private static ChangeRoleViewModel CreateRoleForm(GetUserResponseData user)
        {
            return new ChangeRoleViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                CurrentRole = user.Roles.FirstOrDefault() ?? string.Empty,
                HasAgentProfile = user.AgentId != null,
                RoleOptions = Roles.All
                    .Select(role => new SelectListItem { Value = role, Text = role })
                    .ToList()
            };
        }

        private static string InputField(string name)
        {
            return $"{nameof(ChangeRoleViewModel.Input)}.{name}";
        }

        private UserListViewModel CreateViewModel(GetUserResponseData user)
        {
            return new UserListViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = user.Roles,
                AgentId = user.AgentId,
                PropertyCount = user.PropertyCount,
                IsCurrentUser = IsCurrentUser(user.Id)
            };
        }

        private bool IsCurrentUser(string id)
        {
            return id == User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private IActionResult RedirectWithError(string message)
        {
            TempData[TempDataKeys.ErrorMessage] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}
