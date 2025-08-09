using FC_Application.Models;
using FC_Application.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FC_Application.Controllers
{
    public class FinanceController : Controller
    {
        private readonly FinanceRepository _repository;

        public FinanceController(IConfiguration configuration)
        {
            _repository = new FinanceRepository(configuration);
        }
        // List / Index with pagination and search
        public async Task<IActionResult> Finance(string search = "", int page = 1, int pageSize = 10)
        {
            var finances = await _repository.GetPagedFinancesAsync(search, page, pageSize);
            int totalRecords = await _repository.GetTotalCountAsync(search);

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(finances);
        }

        // GET: Create
        public IActionResult FinanceCreate()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        public async Task<IActionResult> FinanceCreate(Finance finance)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.AddFinanceAsync(finance);
                if (result)
                {
                    TempData["Success"] = "Finance record saved successfully.";
                    return RedirectToAction("FinanceCreate");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while saving the finance record.");
                }
            }

            return View(finance);
        }

        // GET: Edit
        public async Task<IActionResult> FinanceEdit(int SrNo)
        {
            if (SrNo == 0)
                return NotFound();

            var finance = await _repository.GetFinanceByIdAsync(SrNo);
            if (finance == null)
                return NotFound();

            return View(finance);
        }

        // POST: Edit
        [HttpPost]
        public async Task<IActionResult> FinanceEdit(Finance finance)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.UpdateFinanceAsync(finance);
                if (result)
                {
                    TempData["Success"] = "Finance record updated successfully.";
                    return RedirectToAction("Finance");
                }

                ModelState.AddModelError("", "Update failed.");
            }

            return View(finance);
        }

        // GET: Delete (optional confirmation)
        public async Task<IActionResult> FinanceDelete(int SrNo)
        {
            if (SrNo == 0)
                return NotFound();

            var finance = await _repository.GetFinanceByIdAsync(SrNo);
            if (finance == null)
                return NotFound();

            return View(finance); // Optional: Show confirmation page
        }

        // POST: Confirm Delete
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int SrNo)
        {
            var result = await _repository.DeleteFinanceAsync(SrNo);
            if (result)
            {
                TempData["Success"] = "Finance record deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Delete failed.";
            }

            return RedirectToAction("Finance");
        }
    }
}
