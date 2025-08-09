using FC_Application.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FC_Application.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountRepository _repository;

        public AccountController(IConfiguration configuration)
        {
            _repository = new AccountRepository(configuration);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Please provide both username and password.");
                return View();
            }

            var user = await _repository.GetByUserNameAsync(userName);
            if (user == null || user.Password != password)  // Replace with hashing comparison in production
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }

            // On successful login - here using basic cookie authentication
            HttpContext.Session.SetInt32("UserID", user.UserID);
            //TempData["Success"] = " Logged in successfully!";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
