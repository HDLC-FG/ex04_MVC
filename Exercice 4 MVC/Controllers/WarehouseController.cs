using Exercice_4_MVC.Models;
using Exercice_4_MVC.Service;
using Microsoft.AspNetCore.Mvc;

namespace Exercice_4_MVC.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly WarehouseService warehouseService;

        public WarehouseController()
        {
            warehouseService = new WarehouseService();
        }

        // GET: WarehouseController
        public ActionResult Index()
        {
            return View(warehouseService.GetWarehouses());
        }

        // GET: WarehouseController/Details/5
        public ActionResult Details(int id)
        {
            // Search throught warehouses list the target warehouse by id
            var foundWarehouse = warehouseService.GetWarehouses().FirstOrDefault(w => w.Id == id);

            return View(foundWarehouse);
        }

        // GET: WarehouseController/Create
        public ActionResult Create()
        {
            return View(new Warehouse());
        }

        // POST: WarehouseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                //int newId = WarehouseController.Warehouses.Select(w => w.Id).Aggregate((previusMax, current) => { return Math.Max(previusMax, current); }) + 1;
                Warehouse warehouse = new Warehouse();
                ApplyFormCollectionToWarehouse(collection, warehouse);
                warehouseService.Add(warehouse);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WarehouseController/Edit/5
        public ActionResult Edit(int id)
        {
            // Search throught warehouses list the target warehouse by id
            var foundWarehouse = warehouseService.GetWarehouses().FirstOrDefault(w => w.Id == id);
            if (foundWarehouse == null)
            {
                return NotFound();
            }

            return View(foundWarehouse);
        }

        // POST: WarehouseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // Search throught warehouses list the target warehouse by id
                var foundWarehouse = warehouseService.GetWarehouses().FirstOrDefault(w => w.Id == id);
                if (foundWarehouse is null)
                {
                    throw new Exception();
                }
                ApplyFormCollectionToWarehouse(collection, foundWarehouse);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
        public IActionResult GenerateCode(int id)
        {
            var code = warehouseService.GenerateCode(id);
            TempData["Edit"] = code;
            return RedirectToAction(nameof(Edit), nameof(Warehouse), new { id });
        }

        public IActionResult ViewCode()
        {
            ViewBag.IsVeryfied = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ViewCode(int id, string code)
        {
            var foundWarehouse = warehouseService.GetWarehouses().FirstOrDefault(w => w.Id == id);
            if (foundWarehouse == null)
            {
                throw new Exception("L'entrepôt n'existe pas");
            }
            ViewBag.IsVeryfied = warehouseService.VerifyCode(id, code);
            return View();
        }

        private void ApplyFormCollectionToWarehouse(IFormCollection collection, Warehouse foundWarehouse)
        {
            foreach (var field in collection)
            {
                if (field.Key == nameof(foundWarehouse.Id))
                {
                    foundWarehouse.Id = int.Parse(field.Value);
                }
                else if (field.Key == nameof(foundWarehouse.Name))
                {
                    foundWarehouse.Name = field.Value;
                }
                else if (field.Key == nameof(foundWarehouse.Address))
                {
                    foundWarehouse.Address = field.Value;
                }
                else if (field.Key == nameof(foundWarehouse.PostalCode))
                {
                    foundWarehouse.PostalCode = int.Parse(field.Value);
                }
            }
        }

        // GET: WarehouseController/Delete/5
        public ActionResult Delete(int id)
        {
            // Search throught warehouses list the target warehouse by id
            var foundWarehouse = warehouseService.GetWarehouses().FirstOrDefault(w => w.Id == id);

            return View(foundWarehouse);
        }

        // POST: WarehouseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // Search throught warehouses list the target warehouse by id
                var foundWarehouse = warehouseService.GetWarehouses().FirstOrDefault(w => w.Id == id);
                if(foundWarehouse != null)
                {
                    warehouseService.Remove(foundWarehouse);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
