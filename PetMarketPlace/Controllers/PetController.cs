using Microsoft.AspNetCore.Mvc;

namespace WebProject.Controllers
{
    public class PetController : Controller
    {
        
        [HttpGet]
        public IActionResult SellPet()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SellPet(string petChoice)
        {
            if (petChoice == "Cat")
            {
                return RedirectToAction("AddCatSpecification", "PetSpecification");
            }
            else if (petChoice == "Dog")
            {
                return RedirectToAction("AddDogSpecification", "PetSpecification");
            }
            else
            {
                // Optionally handle unexpected values or errors
                return BadRequest("Invalid pet choice.");
            }
        }
    }
}
