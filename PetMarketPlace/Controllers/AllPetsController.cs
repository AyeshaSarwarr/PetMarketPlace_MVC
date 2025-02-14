using Microsoft.AspNetCore.Mvc;
using WebProject.Models.Interfaces;
using WebProject.Models.Entities;
using System.IO;

namespace WebProject.Controllers
{
    public class AllPetsController : Controller
    {
        private readonly IAllPetsInterface _allPets;
        private readonly ICatInterface _cats;
        private readonly IDogInterface _dogs;
        private readonly IWebHostEnvironment _environment;

        public AllPetsController(IAllPetsInterface allPets, ICatInterface cats, IDogInterface dogs, IWebHostEnvironment environment)
        {
            _allPets = allPets;
            _cats = cats;
            _dogs = dogs;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var dogs = _allPets.RetrieveDogs();
            var cats = _allPets.RetrieveCats();

            var petEntity = new AllPetsEntity
            {
                Dogs = dogs,
                Cats = cats
            };

            return View(petEntity);
        }

        [HttpPost]
        public IActionResult Index(int PetId, string actionn, string PetType)
        {
            if(actionn == "Buy")
                    TempData["PetType"] = PetType;
                    TempData["PetId"] = PetId;
                    return RedirectToAction("BuyPet", "Buyer");

            return View();
        }

        [HttpGet]
        public IActionResult MyPets()
        {
            var sellerId = HttpContext.Session.GetInt32("UserId");

            if (sellerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cats = _allPets.SearchCatsUsingId(sellerId.Value);
            var dogs = _allPets.SearchDogsUsingId(sellerId.Value);

            var petEntity = new AllPetsEntity
            {
                Cats = cats,
                Dogs = dogs
            };

            return View("MyPets", petEntity);
        }

        [HttpPost]
        public IActionResult MyPets(int PetId, string action, string PetType)
        {
            TempData["PetId"] = PetId;

            switch (PetType)
            {
                case "Cat":
                    if (action == "Update")
                    {
                        return RedirectToAction("UpdateCat");
                    }
                    else if (action == "Delete")
                    {
                        return RedirectToAction("DeleteCat");
                    }
                    break;
                case "Dog":
                    if (action == "Update")
                    {
                        return RedirectToAction("UpdateDog");
                    }
                    else if (action == "Delete")
                    {
                        return RedirectToAction("DeleteDog");
                    }
                    break;
            }
            return RedirectToAction("MyPets");
        }

        [HttpGet]
        public IActionResult UpdateCat()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateCat(string CatType, int CatAge, string CatColor, decimal CatPrice, IFormFile CatPicture)
        {
            int petId = Convert.ToInt32(TempData["PetId"]);
            CatEntity cat = new CatEntity
            {
                CatType = CatType,
                CatPrice = CatPrice,
                CatAge = CatAge,
                CatColor = CatColor,
                CatId = petId
            };

            if (ModelState.IsValid)
            {
                if (CatPicture != null && CatPicture.Length > 0)
                {
                    string wwwPath = _environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "images/cats");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var fileName = Path.GetFileName(CatPicture.FileName);
                    var filePath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        CatPicture.CopyTo(stream);
                    }

                    cat.CatPicturePath = $"/images/cats/{fileName}";
                }

                bool success = _cats.UpdateCat(cat);
                if (success)
                {
                    return RedirectToAction("MyPets");
                }

                ViewBag.Message = "Failed to update the pet.";
            }
            return View(cat);
        }

        [HttpGet]
        public IActionResult UpdateDog()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateDog(string DogBreed, int DogAge, string DogColor, decimal DogPrice, IFormFile DogPicture)
        {
            int petId = Convert.ToInt32(TempData["PetId"]);
            DogEntity dog = new DogEntity
            {
                DogBreed = DogBreed,
                DogColor = DogColor,
                DogPrice = DogPrice,
                DogAge = DogAge,
                DogId = petId
            };

            if (ModelState.IsValid)
            {
                if (DogPicture != null && DogPicture.Length > 0)
                {
                    string wwwPath = _environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "images/dogs");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var fileName = Path.GetFileName(DogPicture.FileName);
                    var filePath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        DogPicture.CopyTo(stream);
                    }

                    dog.DogPicturePath = $"/images/dogs/{fileName}";
                }

                bool success = _dogs.UpdateDog(dog);
                if (success)
                {
                    return RedirectToAction("MyPets");
                }

                ViewBag.Message = "Failed to update the pet.";
            }
            return View(dog);
        }

        public IActionResult DeleteCat()
        {
            int petId = Convert.ToInt32(TempData["PetId"]);
            bool deleted = _cats.DeleteCat(petId);
            if (deleted)
            {
                ViewBag.Message = "Pet Deleted Successfully";
            }
            else
            {
                ViewBag.Message = "Failed to delete pet.";
            }
            return RedirectToAction("MyPets");
        }

        public IActionResult DeleteDog()
        {
            int petId = Convert.ToInt32(TempData["PetId"]);
            bool deleted = _dogs.DeleteDog(petId);
            if (deleted)
            {
                ViewBag.Message = "Pet Deleted Successfully";
            }
            else
            {
                ViewBag.Message = "Failed to delete pet.";
            }
            return RedirectToAction("MyPets");
        }
    }
}
