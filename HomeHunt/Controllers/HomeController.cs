using System.Diagnostics;
using HomeHunt.Models;
using HomeHunt.Services;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeHunt.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly ILocationService _locationService;

        public HomeController(IPropertyService propertyService, ILocationService locationService)
        {
            _propertyService = propertyService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                AvailableCount = await _propertyService.CountAvailableAsync(),
                ForSaleCount = await _propertyService.CountAvailableForSaleAsync(),
                ForRentCount = await _propertyService.CountAvailableForRentAsync(),
                RecentProperties = await _propertyService.GetRecentAsync(3),
                Cities = await _locationService.GetCitiesAsync(),
                CityCounts = await _locationService.GetCityCountsAsync()
            };

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
