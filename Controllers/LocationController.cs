using Microsoft.AspNetCore.Mvc;

namespace FC_Application.Controllers
{
    public class LocationController : Controller
    {
        public IActionResult Location()
        {
            return View();
        }
        public IActionResult LocationCreate()
        {
            return View();
        }
    }
}
