using HomeHunt.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHunt.Areas.Admin.Controllers
{
    [Area(Routes.AdminArea)]
    [Authorize(Policy = Policies.CanAccessAdmin)]
    public abstract class AdminControllerBase : Controller
    {
    }
}
