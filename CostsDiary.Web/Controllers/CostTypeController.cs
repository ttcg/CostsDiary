using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostsDiary.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CostsDiary.Web.Controllers
{
    public class CostTypeController : Controller
    {
        private readonly ICostTypeService _costTypeService; 
        public CostTypeController(ICostTypeService costTypeService)
        {
            _costTypeService = costTypeService;
        }
        // GET: CostType
        public async Task<IActionResult> Index()
        {
            var results = await _costTypeService.GetAll();
            return View(results);
        }

        // GET: CostType/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CostType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CostType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {
                var newItem = await _costTypeService.Add(new Business.Entities.CostType()
                    {
                        CostTypeDescription = collection[nameof(Business.Entities.CostType.CostTypeDescription)]
                    });

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CostType/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _costTypeService.GetById(id);
            return View(model);
        }

        // POST: CostType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CostType/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CostType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}