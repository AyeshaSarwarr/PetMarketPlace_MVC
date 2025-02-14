using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using WebProject.Models.Entities;
using WebProject.Models.Interfaces;

namespace WebProject.Controllers
{
    public class BuyerController : Controller
    {
        private readonly IBuyerInterface _buyerRepository;
        private readonly ITransactionInterface _transactionInterface;
        private readonly ICatInterface _catInterface;
        private readonly IDogInterface _dogInterface;

        public BuyerController(IBuyerInterface buyerRepository, ITransactionInterface transactionInterface,ICatInterface catInterface, IDogInterface dogInterface)
        {
            _buyerRepository = buyerRepository;
            _transactionInterface = transactionInterface;
            _catInterface = catInterface;
            _dogInterface = dogInterface;
        }
        // Action for login view
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Action to handle login form submission
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var buyer = _buyerRepository.Login(username, password);
            HttpContext.Session.SetInt32("BuyerID", buyer.UserId);
            if (buyer != null)
            {
                return RedirectToAction("Index","AllPets");
            }

            ViewData["Error"] = "Invalid username or password.";
            return View();
        }

        // Action for signup view
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        // Action to handle signup form submission
        [HttpPost]
        public IActionResult Signup(string username, string password, string fullName, string email, string phoneNumber)
        {
            if (_buyerRepository.UserExists(username))
            {
                ViewData["Error"] = "Username already exists. Please choose a different username.";
                return View();
            }

            if (!(_buyerRepository.ValidatePhoneNumber(phoneNumber)))
            {
                ViewData["Error"] = "Please enter correct Phone number.";
                return View();
            }
            BuyerEntity newBuyer = new BuyerEntity
            {
                Username = username,
                Password = password,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            _buyerRepository.Signup(newBuyer);

            return RedirectToAction("Login", "Buyer");
        }

        // Action for the index view
        public IActionResult Index()
        {
            string data = string.Empty;

            if (HttpContext.Request.Cookies.ContainsKey("First_Request"))
            {
                string firstVisitedDateTime = HttpContext.Request.Cookies["First_Request"];
                data = "Welcome back! You visited on: " + firstVisitedDateTime;
            }
            else
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true
                };
                HttpContext.Response.Cookies.Append("First_Request", DateTime.Now.ToString(), cookieOptions);
                data = "You are visiting for the first time!";
            }

            ViewData["Message"] = data;
            return RedirectToAction("BuyPet", "Pet");
        }

        [HttpGet]
        public IActionResult BuyPet()
        {
            // Initialize an empty Transactions model to populate the form
            return View(new TransactionsEntity());
        }

        [HttpPost]
        public IActionResult BuyPet(string PhoneNumber, string Address, decimal PaymentAmount)
        {
            TransactionsEntity transactions = new TransactionsEntity();

            if (ModelState.IsValid)
            {
                // Retrieve buyer and seller IDs from session
                var buyerId = HttpContext.Session.GetInt32("BuyerID");
                var sellerId = HttpContext.Session.GetInt32("UserId");
                var petType = TempData["PetType"] as string;
                var petIdString = TempData["PetId"] as string;

                if (petType == null || petIdString == null)
                {
                    ModelState.AddModelError("", "Required data is missing.");
                    //return View(transactions);
                }

                if (!int.TryParse(petIdString, out int petId))
                {
                    ModelState.AddModelError("", "Invalid Pet ID.");
                    //return View(transactions);
                }

                transactions.PhoneNumber = PhoneNumber;
                transactions.Address = Address;
                transactions.PaymentAmount = PaymentAmount;
                transactions.PetType = petType;
                transactions.PetId = petId;

                bool isTransactionSaved = _transactionInterface.BuyPet(transactions);

                        if (transactions.PetType == "Cat")
                            _catInterface.DeleteCat(transactions.PetId);
                        else
                            _dogInterface.DeleteDog(transactions.PetId);

                        return RedirectToAction("ThankYou");
                
                
            }

            // If model state is not valid or transaction could not be saved, return the form with errors
            return View(transactions);
        }
        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
