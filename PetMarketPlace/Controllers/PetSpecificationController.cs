using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WebProject.Models.Entities;
using WebProject.Models.Interfaces;
using System;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using System.Drawing;
using WebProject.Models.Repositories;
using WebProject.Hubs;

namespace WebProject.Controllers
{
    public class PetSpecificationController : Controller
    {
        private readonly ICatInterface _cat;
        private readonly IDogInterface _dog;
        private readonly IWebHostEnvironment _environment;
        private readonly IHubContext<NotificationHub> _hubContext;
        public PetSpecificationController(ICatInterface cat, IDogInterface dog, IWebHostEnvironment environment, IHubContext<NotificationHub> hubContext)
        {
            _cat = cat;
            _dog = dog;
            _environment = environment;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult AddCatSpecification()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCatSpecification(string CatType, int CatAge, string CatColor, decimal CatPrice, IFormFile catPicture)
        {
            CatEntity model = new CatEntity();
            model.CatType = CatType;
            model.CatPrice = CatPrice;
            model.CatAge = CatAge;
            model.CatColor = CatColor;

            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    ViewBag.Message = "User not logged in.";
                    return View(model);
                }

                // Save the picture to the server and set the picture path
                if (catPicture != null && catPicture.Length > 0)
                {
                    string wwwPath = _environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "images/cats");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var fileName = Path.GetFileName(catPicture.FileName);
                    var filePath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        catPicture.CopyTo(stream);
                    }

                    model.CatPicturePath = $"/images/cats/{fileName}";
                }

                _cat.AddCat(model, userId.Value);
                ViewBag.Message = "Cat specification has been successfully added!";
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", "A new pet Cat has been added. ");

                return RedirectToAction("Success");
            }

            ViewBag.Message = "There was an error in adding the cat specification.";
            return View(model);
        }

        [HttpGet]
        public IActionResult AddDogSpecification()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDogSpecification(string DogBreed, int DogAge, string DogColor, decimal DogPrice, IFormFile dogPicture)
        {
            DogEntity model = new DogEntity();
            model.DogBreed = DogBreed;
            model.DogColor = DogColor;
            model.DogPrice = DogPrice;
            model.DogAge = DogAge;

            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    ViewBag.Message = "User not logged in.";
                    return View(model);
                }

                if (dogPicture != null && dogPicture.Length > 0)
                {
                    string wwwPath = _environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "images/dogs");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var fileName = Path.GetFileName(dogPicture.FileName);
                    var filePath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        dogPicture.CopyTo(stream);
                    }

                    model.DogPicturePath = $"/images/dogs/{fileName}";
                }

                _dog.AddDog(model, userId.Value);
                ViewBag.Message = "Dog specification has been successfully added!";
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", "A new pet Dog has been added. " );
                return RedirectToAction("Success");    
            }

            ViewBag.Message = "There was an error in adding the dog specification.";
            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
