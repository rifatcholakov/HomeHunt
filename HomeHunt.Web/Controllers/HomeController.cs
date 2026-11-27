using HomeHunt.Mapping;
using HomeHunt.Service.Data.Locations;
using HomeHunt.Service.Data.Locations.Response;
using HomeHunt.Service.Data.Properties;
using HomeHunt.Service.Data.Properties.Response;
using HomeHunt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeHunt.Controllers
{
    public class HomeController : Controller
    {
        private const int RecentPropertiesCount = 3;

        private readonly IPropertyService _propertyService;
        private readonly ILocationService _locationService;

        public HomeController(IPropertyService propertyService, ILocationService locationService)
        {
            _propertyService = propertyService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<GetPropertiesResponseData> recentProperties = await _propertyService.GetRecentPropertiesAsync(RecentPropertiesCount);
            IEnumerable<string> cities = await _locationService.GetCitiesAsync();
            IEnumerable<GetCityCountsResponseData> cityCounts = await _locationService.GetCityCountsAsync();

            HomeViewModel model = new HomeViewModel
            {
                AvailableCount = await _propertyService.CountAvailablePropertiesAsync(),
                ForSaleCount = await _propertyService.CountAvailablePropertiesForSaleAsync(),
                ForRentCount = await _propertyService.CountAvailablePropertiesForRentAsync(),
                RecentProperties = recentProperties
                    .Select(p => p.ToViewModel())
                    .ToList(),
                Cities = cities.ToList(),
                CityCounts = cityCounts
                    .Select(c => new CityCountViewModel
                    {
                        City = c.City,
                        PropertyCount = c.PropertyCount
                    })
                    .ToList()
            };

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
