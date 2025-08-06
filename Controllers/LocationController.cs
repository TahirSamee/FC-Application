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

        public async Task<IActionResult> Location(string search = "", int page = 1, int pageSize = 10)
        {
            var locations = await _repository.GetPagedLocationsAsync(search, page, pageSize);
            int totalRecords = await _repository.GetTotalCountAsync(search);

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(locations);
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


        // GET: Edit
        public async Task<IActionResult> LocationEdit(int SrNo)
        {
            if (SrNo==0)
                return NotFound();

            var location = await _repository.GetLocationByIdAsync(SrNo);
            if (location == null)
                return NotFound();

            return View(location);
        }

        // POST: Edit
        [HttpPost]
        public async Task<IActionResult> LocationEdit(Location location)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.UpdateLocationAsync(location);
                if (result)
                {
                    TempData["Success"] = "Location updated successfully.";
                    return RedirectToAction("Location");
                }

                ModelState.AddModelError("", "Update failed.");
            }

            return View(location);
        }

        // GET: Delete (optional confirmation)
        public async Task<IActionResult> LocationDelete(int SrNo)
        {
            if (SrNo == 0)
                return NotFound();

            var location = await _repository.GetLocationByIdAsync(SrNo);
            if (location == null)
                return NotFound();

            return View(location); // Optional: Show confirmation page
        }

        // POST: Confirm Delete
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int SrNo)
        {
            var result = await _repository.DeleteLocationAsync(SrNo);
            if (result)
            {
                TempData["Success"] = "Location deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Delete failed.";
            }

            return RedirectToAction("Location");
        }

    }
}
