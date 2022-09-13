using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmsWebApp.Controllers
{
    public class CargoOrderDetails : Controller
    {
        // GET: CargoOrderDetails
        public ActionResult Index()
        {
            return View();
        }

        // GET: CargoOrderDetails/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CargoOrderDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CargoOrderDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CargoOrderDetails/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CargoOrderDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CargoOrderDetails/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CargoOrderDetails/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
