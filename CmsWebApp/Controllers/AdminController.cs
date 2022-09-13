using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System;
using CmsClassLibrary;
using System.Net.Http.Json;
using CmsClassLibrary.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CmsWebApp.Controllers
{
    public class AdminController : Controller
    {
        private string apiBaseUrl;
        public AdminController(IConfiguration config)
        {
            apiBaseUrl = config["ApiBaseUrl"];
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: AuthController/Create
        public ActionResult Register()
        {
            return View();
        }
        // POST: AuthController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Admin admin)
        {
            admin.Name = "Admin";
            if (!ModelState.IsValid)
            {
                return View(admin);
            }

            //post, user api controller

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //POST https://localhost:44357/api/users
                var response = client.PostAsJsonAsync("Admin", admin).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ModelState.AddModelError("", "Error while registering admin");
                }
            }
            return View(admin);
        }

        // GET: AuthController/Create
        public ActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin admin)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(user);
            //}
            admin.Name = "Admin";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //POST https://localhost:44357/api/users/login
                var response = client.PostAsJsonAsync("Admins/login", admin).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var AdminLoginDto = JsonConvert.DeserializeObject<AdminLoginDto>(responseString);
                    //todo
                    //save jwt token
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "AdminLoginDto", AdminLoginDto);
                    return RedirectToAction("Index", "admin");
                }
                else
                {
                    ModelState.AddModelError("", "Error while login");
                }
            }
            return View(admin);
        }
        public ActionResult EmpIndex()
        {
            List<Employee> empList = new List<Employee>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //https://localhost:44326/api/cargoes
                var response = client.GetAsync("Employees").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    empList = JsonConvert.DeserializeObject<List<Employee>>(responseString);
                    return View(empList);
                }
                else
                {
                    ModelState.AddModelError("", "Error while calling API");
                }
            }
            return View(empList);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            //get dept list
            return View();
        }
        // POST: EmployeesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee emp)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Validation Failed");
                return View(emp);
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //set token
                //var loginDto = SessionHelper.GetObjectFromJson<LoginDto>(
                //HttpContext.Session, "loginDto");
                //client.DefaultRequestHeaders.Authorization =
                //new AuthenticationHeaderValue(
                //JwtBearerDefaults.AuthenticationScheme, loginDto.Token);

                //POST https://localhost:44326/api/employees
                var response = client.PostAsync("Admins/EmployeeAdd", new StringContent(JsonConvert.SerializeObject(emp), Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    //var responseString = response.Content.ReadAsStringAsync().Result;
                    //product = JsonConvert.DeserializeObject<Product>(responseString);
                    return RedirectToAction(nameof(EmpIndex));
                }
                else
                {
                    ModelState.AddModelError("", "Error while calling API");
                }
            }
            return View(emp);

        }

        // GET: EmployeesController/Edit/5
        public ActionResult Edit(int id)
        {
            Employee emp = GetEmployeeById(id);
            if (emp == null)
            {
                return RedirectToAction(nameof(EmpIndex));
            }
            return View(emp);

        }
        private Employee GetEmployeeById(int id)
        {
            Employee emp = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //GET: https://localhost:44326/api/emplyees/{id}
                var response = client.GetAsync($"Admins/GetEmployee/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    emp = JsonConvert.DeserializeObject<Employee>(responseString);
                }
            }
            return emp;
        }

        // POST: EmployeesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Employee emp)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Validation failed");
                return View(emp);
            }
            if (id != emp.EmpId)
            {
                ModelState.AddModelError("", "Ids do not match");
                return View(emp);
            }


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //Put: https://localhost:44326/api/employees/{id}
                var response = client.PutAsJsonAsync($"admins/EmployeeUpdate/{id}", emp).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(EmpIndex));
                }

            }
            return View(emp);
        }

        // GET: CargoesController/Delete/5
        public ActionResult Delete(int id)
        {
            Employee emp = GetEmployeeById(id);
            if (emp == null)
            {
                return RedirectToAction(nameof(EmpIndex));
            }
            return View(emp);
        }

        // POST: CargoesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //delete: https://localhost:44326/api/cargoes/{id}
                var response = client.DeleteAsync($"admins/EmployeeDelete/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(EmpIndex));
                }
            }
            return RedirectToAction(nameof(EmpIndex));
        }
    



// GET: EmployeesController/Delete/5
//public ActionResult Delete(int id)
//        {
//            Employee emp = GetEmployeeById(id);
//            using (var client = new HttpClient())
//            {
//                client.BaseAddress = new Uri(apiBaseUrl);
//                //Put: https://localhost:44326/api/employees/{id}
//                var response = client.DeleteAsync($"Admins/Employee/Delete/{id}").Result;
//                if (response.IsSuccessStatusCode)
//                {
//                    return RedirectToAction(nameof(EmpIndex));
//                }

//            }
//            return View(emp);

//        }

//        // POST: EmployeesController/Delete/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Delete(int id, Employee emp)
//        {
//            using (var client = new HttpClient())
//            {
//                client.BaseAddress = new Uri(apiBaseUrl);
//                //Put: https://localhost:44326/api/employees

//                var response = client.PutAsJsonAsync($"Admins/Employee/Update/{id}", emp).Result;
//                if (response.IsSuccessStatusCode)
//                {
//                    return RedirectToAction(nameof(EmpIndex));
//                }

//            }
//            return View(emp);

//        }

        // GET: CustomersController
        public ActionResult CustIndex()
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

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("HomePage","Home");
        }

    }
}
