using Microsoft.AspNetCore.Mvc;

namespace FC_Application.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

    }
}
