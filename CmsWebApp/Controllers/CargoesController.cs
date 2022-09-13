using CmsClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Net.Http.Json;
using CmsClassLibrary.Dtos;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace CmsWebApp.Controllers
{
    public class CargoesController : Controller
    {
        private string apiBaseUrl;

        public CargoesController(IConfiguration Config)
        {
            apiBaseUrl = Config["ApiBaseUrl"];
        }

        // GET: CargoesController
        public ActionResult Index()
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

        // GET: CargoesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CargoesController/Create
        public ActionResult Create()
        {
            //get cargo list
            return View();
        }

        // POST: CargoesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cargo carg)
        {
            //SSV(Server Side Valiation)
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Validation failed");
                return View(carg);
            }
            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new Uri(apiBaseUrl);
                //POST https://localhost:44326/api/cargoes
                var response = client.PostAsync("cargoes", new StringContent(JsonConvert.SerializeObject(carg), Encoding.UTF8, "application/json")).Result;
                //var response = client.PostAsync("cargoes", new StringContent(JsonConvert.SerializeObject(carg),Encoding.UTF8,"application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error while Calling API");
                }
            }
            return View(carg);
        }

        // GET: CargoesController/Edit/5
        public ActionResult Edit(int id)
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
        public ActionResult Edit(int id, Cargo carg)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Validation failed");
                return View(carg);
            }
            if (id != carg.CargoId)
            {
                ModelState.AddModelError("", "Ids do not match");
                return View(carg);
            }

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

        // GET: CargoesController/Delete/5
        public ActionResult Delete(int id)
        {
            Cargo carg = GetCargoById(id);
            if (carg == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(carg);
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
                var response = client.DeleteAsync($"cargoes/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

