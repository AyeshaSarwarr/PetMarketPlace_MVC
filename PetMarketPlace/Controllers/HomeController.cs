using Microsoft.AspNetCore.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string Role)
        {
            if (Role == "Buyer")
                return RedirectToAction("Login", "Buyer");
            

            return RedirectToAction("Login", "Seller");

        }

    }
}
