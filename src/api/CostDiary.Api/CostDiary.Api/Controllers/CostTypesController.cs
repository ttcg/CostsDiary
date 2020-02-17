using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostDiary.Api.Data.Repositories;
using CostsDiary.Api.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CostsDiary.Api.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostTypesController : ControllerBase
    {
        private readonly ICostTypesRepository _costTypesRepository;
        public CostTypesController(ICostTypesRepository costTypesRepository)
        {
            _costTypesRepository = costTypesRepository;
        }

        // GET: api/CostTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostTypeViewModel>>> Get()
        {
            var results = await _costTypesRepository.GetAll();

            var viewModels = results.Select(item => new CostTypeViewModel
            {
                CostTypeId = item.CostTypeId,
                CostTypeName = item.CostTypeName
            }).ToList();

            return Ok(viewModels);
        }

        // GET: api/CostTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CostTypeViewModel>> Get(Guid id)
        {
            var record = await _costTypesRepository.GetById(id);

            if (record == null)
                return NotFound($"The given id: {id} is not found");

            return Ok(new CostTypeViewModel
            {
                CostTypeId = record.CostTypeId,
                CostTypeName = record.CostTypeName
            });
        }

        [HttpOptions]
        public IActionResult GetOptions()
        {
            Response.Headers.Add("allow", "GET");
            return Ok();
        }
    }
}
