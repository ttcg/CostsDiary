using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostDiary.Api.Data.Repositories;
using CostsDiary.Api.Data.Entities;
using CostsDiary.Api.Web.ViewModels;
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
        public async Task<ActionResult<IEnumerable<CostTypeViewModel>>> Get()
        {
            var results = await _costItemsRepository.GetAll();

            var costTypes = await _costTypesRepository.GetAll();

            var viewModels = results.Select(item => new CostItemViewModel
            {
                CostItemId = item.CostItemId,
                ItemName = item.ItemName,
                CostType = GetCostTypeViewModel(costTypes, item.CostTypeId),
                Amount = item.Amount,
                DateUsed = item.DateUsed
            })
                .OrderBy(x => x.DateUsed)
                .ToList();

            return Ok(viewModels);
        }

        // GET: api/CostItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CostTypeViewModel>> Get(Guid id)
        {
            var record = await _costItemsRepository.GetById(id);

            if (record == null)
                return NotFound();

            var costTypes = await _costTypesRepository.GetAll();

            return Ok(new CostItemViewModel
            {
                CostItemId = record.CostItemId,
                ItemName = record.ItemName,
                CostType = GetCostTypeViewModel(costTypes, record.CostTypeId),
                Amount = record.Amount,
                DateUsed = record.DateUsed
            });
        }

        [HttpOptions]
        public IActionResult GetOptions()
        {
            Response.Headers.Add("allow", "GET,OPTIONS,POST,PUT,DELETE");
            return Ok();
        }

        private CostTypeViewModel GetCostTypeViewModel(IEnumerable<CostType> costTypes, Guid costTypeId)
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
