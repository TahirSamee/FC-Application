using FC_Application.Models;
using FC_Application.Repository;
using Microsoft.AspNetCore.Mvc;
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
                TotalLocations = await _locationRepo.GetTotalLocationCountAsync(),
                PendingLocations = await _locationRepo.GetPendingLocationCountAsync(),
                TotalFinances = await _financeRepo.GetTotalFinanceCountAsync(),
                PendingFinances = await _financeRepo.GetPendingFinanceCountAsync(),
                Locations= locations,
                Finances = finances,
            };

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
