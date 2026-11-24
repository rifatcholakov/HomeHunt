using System.Diagnostics;
using HomeHunt.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HomeHunt.Controllers
{
    [IgnoreAntiforgeryToken]
    public class ErrorController : Controller
    {
        private const string NotFoundView = "NotFound";
        private const string ServerErrorView = "ServerError";

        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ServerError()
        {
            return View(ServerErrorView, CreateModel((int)HttpStatusCode.InternalServerError));
        }

        [Route("Error/{statusCode:int}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Status(int statusCode)
        {
            Response.StatusCode = statusCode;

            if (statusCode == (int)HttpStatusCode.NotFound)
            {
                return View(NotFoundView, CreateModel(statusCode));
            }

            return View(ServerErrorView, CreateModel(statusCode));
        }

        private ErrorViewModel CreateModel(int statusCode)
        {
            return new ErrorViewModel
            {
                StatusCode = statusCode,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
        }
    }
}
