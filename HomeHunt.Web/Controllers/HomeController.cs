using System.Diagnostics;
using HomeHunt.Models;
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
        private readonly IPropertyService _propertyService;
        private readonly ILocationService _locationService;

        public HomeController(IPropertyService propertyService, ILocationService locationService)
        {
            _propertyService = propertyService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<GetPropertiesResponseData> recentProperties = await _propertyService.GetRecentPropertiesAsync(3);
            IEnumerable<string> cities = await _locationService.GetCitiesAsync();
            IEnumerable<GetCityCountsResponseData> cityCounts = await _locationService.GetCityCountsAsync();

            HomeViewModel model = new HomeViewModel
            {
                AvailableCount = await _propertyService.CountAvailablePropertiesAsync(),
                ForSaleCount = await _propertyService.CountAvailablePropertiesForSaleAsync(),
                ForRentCount = await _propertyService.CountAvailablePropertiesForRentAsync(),
                RecentProperties = recentProperties
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
