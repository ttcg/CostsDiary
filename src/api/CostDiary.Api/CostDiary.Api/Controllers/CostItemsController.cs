using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostDiary.Api.Data.Repositories;
using CostsDiary.Api.Data.Entities;
using CostsDiary.Api.Web.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CostsDiary.Api.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostItemsController : ControllerBase
    {
        private readonly ICostItemsRepository _costItemsRepository;
        private readonly ICostTypesRepository _costTypesRepository;
        public CostItemsController(
            ICostItemsRepository costItemsRepository,
            ICostTypesRepository costTypesRepository)
        {
            _costItemsRepository = costItemsRepository;
            _costTypesRepository = costTypesRepository;
        }

        // GET: api/CostItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostItemViewModel>>> Get()
        {
            var results = await _costItemsRepository.GetAll();

            var costTypes = await _costTypesRepository.GetAll();

            var viewModels = results.Select(item => new CostItemViewModel
            {
                CostItemId = item.CostItemId,
                ItemName = item.ItemName,
                CostType = AdaptToCostTypeViewModel(costTypes, item.CostTypeId),
                Amount = item.Amount,
                DateUsed = item.DateUsed
            })
                .OrderBy(x => x.DateUsed)
                .ToList();

            return Ok(viewModels);
        }

        // GET: api/CostItems/5
        [HttpGet("{id}", Name = "GetById")]
        public async Task<ActionResult<CostItemViewModel>> GetById(Guid id)
        {
            var record = await _costItemsRepository.GetById(id);

            if (record == null)
                return NotFound($"The given id: {id} is not found");

            var costTypes = await _costTypesRepository.GetAll();

            return Ok(new CostItemViewModel
            {
                CostItemId = record.CostItemId,
                ItemName = record.ItemName,
                CostType = AdaptToCostTypeViewModel(costTypes, record.CostTypeId),
                Amount = record.Amount,
                DateUsed = record.DateUsed
            });
        }

        // POST: api/CostItems/
        [HttpPost]
        public async Task<ActionResult<CostItemViewModel>> Add([FromBody] CostItemCreateViewModel model)
        {
            var entity = new CostItem
            {
                Amount = model.Amount,
                CostTypeId = model.CostTypeId,
                DateUsed = model.DateUsed,
                ItemName = model.ItemName
            };

            var costTypes = await _costTypesRepository.GetAll();

            if (costTypes.Any(x => x.CostTypeId == model.CostTypeId) == false)
                return NotFound("CostTypeId does not exist");

            var record = await _costItemsRepository.Add(entity);

            return CreatedAtRoute("GetById",
                new { Id = record.CostItemId },
                new CostItemViewModel
                {
                    CostItemId = record.CostItemId,
                    ItemName = record.ItemName,
                    CostType = AdaptToCostTypeViewModel(costTypes, record.CostTypeId),
                    Amount = record.Amount,
                    DateUsed = record.DateUsed
                });
        }

        // PATCH: api/CostItems/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] JsonPatchDocument<CostItemPatchViewModel> patchDocument)
        {
            {
                var costItem = await _costItemsRepository.GetById(id);

                if (costItem == null)
                    return NotFound();

                var model = new CostItemPatchViewModel
                {
                    Amount = costItem.Amount,
                    ItemName = costItem.ItemName,
                    CostTypeId = costItem.CostTypeId,
                    DateUsed = costItem.DateUsed
                };

                patchDocument.ApplyTo(model);

                // auto validation by using validator
                if (!TryValidateModel(model))
                    return ValidationProblem(ModelState);

                // custom validation
                var costTypes = await _costTypesRepository.GetAll();
                if (costTypes.Any(x => x.CostTypeId == model.CostTypeId) == false)
                    return NotFound("CostTypeId does not exist");

                // copy the changes back to original from patched Model
                MapPatchedModelToCostItem(costItem, model);

                await _costItemsRepository.Update(costItem);

                return NoContent();
            }

            void MapPatchedModelToCostItem(CostItem costItem, CostItemPatchViewModel model)
            {
                costItem.Amount = model.Amount;
                costItem.CostTypeId = model.CostTypeId;
                costItem.ItemName = model.ItemName;
                costItem.DateUsed = model.DateUsed;
            }
        }

        // DELETE: api/CostItems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var record = await _costItemsRepository.GetById(id);

            if (record == null)
                return NotFound();

            await _costItemsRepository.Delete(id);

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetOptions()
        {
            Response.Headers.Add("allow", "GET,OPTIONS,POST,PUT,DELETE");
            return Ok();
        }

        // POST: api/CostItems/reset
        [HttpPost("reset")]
        public async Task<ActionResult> Reset()
        {
            await _costItemsRepository.Reset();

            return NoContent();
        }

        // GET: api/CostItems/filter
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CostItemViewModel>>> Filter([FromQuery] int year, [FromQuery] int month)
        {
            var costItems = await _costItemsRepository.GetRecordsByFilter(year, month);

            var costTypes = await _costTypesRepository.GetAll();

            var viewModels = costItems.Select(item => new CostItemViewModel
            {
                CostItemId = item.CostItemId,
                ItemName = item.ItemName,
                CostType = AdaptToCostTypeViewModel(costTypes, item.CostTypeId),
                Amount = item.Amount,
                DateUsed = item.DateUsed
            })
                .OrderBy(x => x.DateUsed)
                .ToList();

            return Ok(viewModels);
        }

        private CostTypeViewModel AdaptToCostTypeViewModel(IEnumerable<CostType> costTypes, Guid costTypeId)
        {
            var costType = costTypes.SingleOrDefault(c => c.CostTypeId == costTypeId);

            if (costType == null)
                return null;

            return new CostTypeViewModel
            {
                CostTypeId = costType.CostTypeId,
                CostTypeName = costType.CostTypeName
            };
        }
    }
}
