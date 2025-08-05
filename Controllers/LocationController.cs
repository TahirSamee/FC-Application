using FC_Application.Models;
using FC_Application.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FC_Application.Controllers
{
    public class LocationController : Controller
    {
        private readonly LocationRepository _repository;

        public LocationController(IConfiguration configuration)
        {
            _repository = new LocationRepository(configuration);
        }

        public IActionResult Location()
        {
            return View();
        }
        public IActionResult LocationCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LocationCreate(Location location)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.AddLocationAsync(location);
                if (result)
                {
                    TempData["Success"] = "Location saved successfully.";
                    return RedirectToAction("LocationCreate");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while saving the location.");
                }
            }

            return View(location);
        }
    }
}
