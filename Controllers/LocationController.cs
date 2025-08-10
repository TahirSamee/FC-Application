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

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please upload a valid Excel file.";
                return RedirectToAction("Location");
            }

            var locations = new List<Location>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = file.OpenReadStream())
            using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
            {
                int sheetIndex = 0;

                do
                {
                    sheetIndex++;
                    if (sheetIndex > 1)
                    {
                        TempData["Error"] = "Please upload an Excel file with only one worksheet.";
                        return RedirectToAction("Location");
                    }

                    bool isHeaderSkipped = false;
                    List<string> requiredHeaders = new List<string>
            {
                "Surveyor Name","Location ID","Sales Order ID","Client Location Identifier","Status",
                "Service Due Date","Service Date","Client","Customer","Brand Name",
                "Location Number","Location Nickname","Service","Address","City","State","Zip",
                "Phone Number","Email","Manager Name","L1 Manager Name","L1 Manager Email","L1 Manager Phone",
                "L2 Manager Name","L2 Manager Email","L2 Manager Phone","Assets Verified","Asset Count",
                "Sq Ft","Value","Notes","Verifier?","Date Verified"
            };

                    while (reader.Read())
                    {
                        if (!isHeaderSkipped)
                        {
                            // Read header row and validate
                            var headers = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                headers.Add(reader.GetValue(i)?.ToString()?.Trim() ?? "");
                            }

                            var missing = requiredHeaders
                                .Where(r => !headers.Any(h => string.Equals(h, r, StringComparison.OrdinalIgnoreCase)))
                                .ToList();

                            if (missing.Any())
                            {
                                TempData["Error"] = "Wrong sheet for Location. Missing columns: " + string.Join(", ", missing);
                                return RedirectToAction("Location");
                            }

                            isHeaderSkipped = true;
                            continue;
                        }

                        // Safe column accessor
                        string GetCol(int i) => i < reader.FieldCount ? reader.GetValue(i)?.ToString() : null;

                        var location = new Location
                        {
                            SurveyorName = GetCol(0),
                            LocationID = GetCol(1),
                            SalesOrderID = GetCol(2),
                            ClientLocationIdentifier = GetCol(3),
                            Status = GetCol(4),
                            ServiceDueDate = GetCol(5),
                            ServiceDate = GetCol(6),
                            Client = GetCol(7),
                            Customer = GetCol(8),
                            BrandName = GetCol(9),
                            LocationNumber = GetCol(10),
                            LocationNickname = GetCol(11),
                            Service = GetCol(12),
                            Address = GetCol(13),
                            City = GetCol(14),
                            State = GetCol(15),
                            Zip = GetCol(16),
                            PhoneNumber = GetCol(17),
                            Email = GetCol(18),
                            ManagerName = GetCol(19),
                            L1ManagerName = GetCol(20),
                            L1ManagerEmail = GetCol(21),
                            L1ManagerPhone = GetCol(22),
                            L2ManagerName = GetCol(23),
                            L2ManagerEmail = GetCol(24),
                            L2ManagerPhone = GetCol(25),
                            AssetsVerified = GetCol(26),
                            AssetCount = GetCol(27),
                            SqFt = GetCol(28),
                            Value = GetCol(29),
                            Notes = GetCol(30),
                            Verifier = GetCol(31),
                            DateVerified = GetCol(32)
                        };

                        locations.Add(location);
                    }
                }
                while (reader.NextResult());
            }

            // Check duplicates inside the Excel file
            var duplicates = locations
                .GroupBy(l => l.LocationID?.Trim().ToLower())
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            if (duplicates.Any())
            {
                TempData["Error"] = $"Found {duplicates.Count} duplicate rows in the Excel file.";
                return RedirectToAction("Location");
            }

            // Save to DB
            foreach (var loc in locations)
            {
                await _repository.AddLocationAsync(loc);
            }

            TempData["Success"] = $"{locations.Count} locations imported successfully.";
            return RedirectToAction("Location");
        }



    }
}
