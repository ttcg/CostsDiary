using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostsDiary.Services;
using CostsDiary.Web.Models;
using CostsDiary.Web.Models.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CostsDiary.Web.Controllers
{
    public class CostTypeController : BaseController
    {
        private readonly ICostTypeService _costTypeService; 
        public CostTypeController(ICostTypeService costTypeService)
        {
            _costTypeService = costTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var results = await _costTypeService.GetAll();

            return View(results?.Select(r =>
                r.ToModel()
            ));
        }        

        // GET: CostType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CostType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CostTypeModel model)
        {
            try
            {
                var newItem = await _costTypeService.Add(new Domain.Entities.CostType()
                    {
                        CostTypeDescription = model.CostTypeDescription
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
            var entity = await _costTypeService.GetById(id);
            return View(entity.ToModel());
        }

        // POST: CostType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CostTypeModel model)
        {
            try
            {
                var item = await _costTypeService.Update(new Domain.Entities.CostType()
                {
                    CostTypeId = id,
                    CostTypeDescription = model.CostTypeDescription
                });

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CostType/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _costTypeService.GetById(id);
            return View(entity.ToModel());
        }

        // POST: CostType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CostTypeModel model)
        {
            try
            {
                await _costTypeService.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}