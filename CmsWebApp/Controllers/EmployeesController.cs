using CmsClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text;
using System.Diagnostics;
using CmsClassLibrary.Dtos;

namespace CmsWebApp.Controllers
{
    public class EmployeesController : Controller
    {
        private string apiBaseUrl;
        public EmployeesController(IConfiguration config)
        {
            apiBaseUrl = config["ApiBaseUrl"];
        }

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


        public ActionResult CargoIndex()
        {
            List<Cargo> cargoList = new List<Cargo>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //https://localhost:44326/api/cargoes
                var response = client.GetAsync("cargoes").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    cargoList = JsonConvert.DeserializeObject<List<Cargo>>(responseString);
                    return View(cargoList);
                }
                else
                {
                    ModelState.AddModelError("", "Error while calling API");
                }
            }
            return View(cargoList);
        }

        // GET: CargoesController/Edit/5
        public ActionResult Edit3(int id)
        {
            Cargo carg = GetCargoById(id);
            if (carg == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(carg);
        }

        private Cargo GetCargoById(int id)
        {
            Cargo carg = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //GET: https://localhost:44326/api/cargoes/{id}
                var response = client.GetAsync($"cargoes/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    carg = JsonConvert.DeserializeObject<Cargo>(responseString);
                }
            }
            return carg;
        }

        // POST: CargoesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit3(int id, Cargo carg)
        {
            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("", "Validation failed");
            //    return View(carg);
            //}
            //if (id != carg.CargoId)
            //{
            //    ModelState.AddModelError("", "Ids do not match");
            //    return View(carg);
            //}

            //var baseUrl = config["ApiBaseUrl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //Put: https://localhost:44326/api/cargoes/{id}
                var response = client.PutAsJsonAsync($"cargoes/{id}", carg).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(carg);
        }


        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Employee emp)
        {
            if (!ModelState.IsValid)
            {
                return View(emp);
            }
            //emp.EmpName = "Ranu";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //POST https://localhost:44357/api/users/login
                var response = client.PostAsJsonAsync("Employees/login", emp).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var LoginDto = JsonConvert.DeserializeObject<LoginDto>(responseString);
                    //todo
                    //save jwt token
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "LoginDto", LoginDto);
                    return RedirectToAction("Dashboard", "Employees");
                }
                else
                {
                    ModelState.AddModelError("", "Error while login");
                }
            }
            return View(emp);
        }

        // GET: EmployeeController
        public ActionResult Index()
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

        public ActionResult Index1()
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

        public ActionResult Dashboard()
        {
            return View();
        }


        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
                var response = client.PostAsync("employees", new StringContent(JsonConvert.SerializeObject(emp), Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    //var responseString = response.Content.ReadAsStringAsync().Result;
                    //product = JsonConvert.DeserializeObject<Product>(responseString);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error while calling API");
                }
            }
            return View(emp);
            
        }

        // GET: CargoesController/Edit/5
        public ActionResult Edit(int id)
        {
            Debug.Write("Id = "+id);
            Employee emp = GetEmployeeById(id);
            Debug.Write("Id = "+id);
            if (emp == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(emp);
        }

        private Employee GetEmployeeById(int id)
        {
            Employee emp = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //GET: https://localhost:44326/api/cargoes/{id}
                var response = client.GetAsync($"employees/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    emp = JsonConvert.DeserializeObject<Employee>(responseString);
                }
            }
            return emp;
        }

        // POST: CargoesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Employee emp)
        {
            //Debug.Write("Gender = " + emp.Gender);
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

            //var baseUrl = config["ApiBaseUrl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //Put: https://localhost:44326/api/cargoes/{id}
                var response = client.PutAsJsonAsync($"employees/{id}", emp).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(emp);
        }

        // GET: CargoesController/Edit/5
        public ActionResult Edit1(int id)
        {
            Debug.Write("Id = " + id);
            Employee emp = GetEmployeeById(id);
            Debug.Write("Id = " + id);
            if (emp == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(emp);
        }

        //private Employee GetEmployeebyId(int id)
        //{
        //    Employee emp = null;
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(apiBaseUrl);
        //        //GET: https://localhost:44326/api/cargoes/{id}
        //        var response = client.GetAsync($"employees/{id}").Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseString = response.Content.ReadAsStringAsync().Result;
        //            emp = JsonConvert.DeserializeObject<Employee>(responseString);
        //        }
        //    }
        //    return emp;
        //}

        // POST: CargoesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit1(int id, Employee emp)
        {
            //Debug.Write("Gender = " + emp.Gender);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Validation failed");
                return View(emp);
            }
            //if (id != emp.EmpId)
            //{
            //    ModelState.AddModelError("", "Ids do not match");
            //    return View(emp);
            //}

            //var baseUrl = config["ApiBaseUrl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                //Put: https://localhost:44326/api/cargoes/{id}
                var response = client.PutAsJsonAsync($"employees/{id}", emp).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
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
                var response = client.DeleteAsync($"employees/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

