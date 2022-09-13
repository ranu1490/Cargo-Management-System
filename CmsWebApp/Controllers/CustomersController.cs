using CmsClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System;
using CmsClassLibrary.Dtos;
using System.Net.Http.Json;

namespace CmsWebApp.Controllers
{
    public class CustomersController : Controller
    {
        private string apiBaseUrl;

        public CustomersController(IConfiguration Config)
        {
            apiBaseUrl = Config["ApiBaseUrl"];
        }
        // GET: CustomersController


        public ActionResult Index1()
        {
            return View();
        }

        public ActionResult Index()
        {
            List<Customer> customerList = new List<Customer>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //https://localhost:44326/api/cargoes
                var response = client.GetAsync("customers").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    customerList = JsonConvert.DeserializeObject<List<Customer>>(responseString);
                    return View(customerList);
                }
                else
                {
                    ModelState.AddModelError("", "Error while calling API");
                }
            }
            return View(customerList);
        }

        // GET: AuthController/Create
        public ActionResult Register()
        {
            return View();
        }
        // POST: AuthController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer customer)
        {
            //customer.CustName = "Ranu";
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            //post, user api controller

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //POST https://localhost:44357/api/users
                var response = client.PostAsJsonAsync("Customers", customer).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ModelState.AddModelError("", "Error while registering customer");
                }
            }
            return View(customer);
        }

        // GET: AuthController/Create
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer customer)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(user);
            //}
            //admin.Name = "Admin";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //POST https://localhost:44357/api/users/login
                var response = client.PostAsJsonAsync("Customers/login", customer).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var CustLoginDto = JsonConvert.DeserializeObject<CustLoginDto>(responseString);
                    //todo
                    //save jwt token
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "CustLoginDto", CustLoginDto);
                    return RedirectToAction("Dashboard", "customers");
                }
                else
                {
                    ModelState.AddModelError("", "Error while login");
                }
            }
            return View(customer);
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("HomePage", "Home");


        }
    }
}
