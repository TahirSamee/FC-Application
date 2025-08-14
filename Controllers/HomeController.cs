using FC_Application.Models;
using FC_Application.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace FC_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LocationRepository _locationRepo;
        private readonly FinanceRepository _financeRepo;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;

            _locationRepo = new LocationRepository(configuration);
            _financeRepo = new FinanceRepository(configuration);
        }

        public async Task<IActionResult> Index()
        {
            var locations = await _locationRepo.GetPendingLocationsAsync();
            var finances = await _financeRepo.GetPendingFinanceAsync();

            var model = new DashboardViewModel
            {
                TotalLocations = 0,//await _locationRepo.GetTotalLocationCountAsync(),
                PendingLocations = 0, //await _locationRepo.GetPendingLocationCountAsync(),
                TotalFinances = 0, //await _financeRepo.GetTotalFinanceCountAsync(),
                PendingFinances = 0, //await _financeRepo.GetPendingFinanceCountAsync(),
                Locations = locations,
                Finances = finances,
            };

            var geoLocations = new List<object>();
            foreach (var loc in locations)
            {
                if (loc.Address != null)
                {
                    
                    geoLocations.Add(new { locationid =loc.LocationID,status= loc.Status, address = loc.Address, city=loc.City, state=loc.State, zip=loc.Zip });
                }

            }

            ViewBag.PendingLocations = JsonConvert.SerializeObject(geoLocations);
            return View(model);
        }


        


        public IActionResult Privacy()
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
