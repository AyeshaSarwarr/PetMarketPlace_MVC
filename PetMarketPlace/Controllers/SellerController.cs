using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using WebProject.Models.Entities;
using WebProject.Models.Interfaces;

namespace WebProject.Controllers
{
    public class SellerController : Controller
    {
        private readonly ISellerInterface _sellerRepo;

        public SellerController(ISellerInterface sellerRepo)
        {
            _sellerRepo = sellerRepo;
        }

        // Action for login view
        public IActionResult Login()
        {
            return View();
        }

        // Action to handle login form submission
        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            var seller = _sellerRepo.Login(Username,Password);

            if (seller.Item1 != null)
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
                var userId = seller.Item2;
                HttpContext.Session.SetInt32("UserId", userId);
                return RedirectToAction("SelectOptions","Seller");
                //return View("SelectAction");
            }

            ViewData["Error"] = "Invalid username or password.";
            return View();
        }

        // Action for signup view
        public IActionResult Signup()
        {
            return View();
        }

        // Action to handle signup form submission
        [HttpPost]
        public IActionResult Signup(string username, string password, string businessName, string businessAddress, string contactNumber, string website)
        {
            var result = _sellerRepo.UserNotExists(username);

            if (result)
            {
                SellerEntity seller1 = new SellerEntity();
                seller1.Username = username;
                seller1.Password = password;
                seller1.BusinessName = businessName;
                seller1.BusinessAddress = businessAddress; 
                seller1.ContactNumber = contactNumber;
                seller1.Website = website;
                _sellerRepo.Signup(seller1);
                return RedirectToAction("Login", "Seller");
            }

            ViewData["Error"] = "Username already Exists";
            return View();
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
            return View();
        }

        [HttpGet]
        public IActionResult SelectOptions()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SelectOptions(string actionn)
        {
            if (actionn == "StartSelling")
            {
                // Redirect to the Start Selling page
                return RedirectToAction("SellPet", "Pet");

            }
            else if (actionn == "ViewMyPets")
            {
                // Redirect to the View My Pets page
                return RedirectToAction("MyPets", "AllPets");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
