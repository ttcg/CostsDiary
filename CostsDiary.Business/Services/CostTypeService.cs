using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostsDiary.Business.Entities;
using CostsDiary.Data.Repositories;
using CostsDiary.Business.Entities.Mapping;

namespace CostsDiary.Business
{
    public class CostTypeService : ICostTypeService
    {
        private readonly ICostTypeRepository _costTypeRepository;

        public CostTypeService(ICostTypeRepository costTypeRepository)
        {
            _costTypeRepository = costTypeRepository;
        }

        public async Task<CostType> Add(CostType item)
        {
            var dto = await _costTypeRepository.Add(item.ToDto());

            return dto.ToEntity();
        }

        public async Task Delete(int id)
        {
            await _costTypeRepository.Delete(id);
        }

        public async Task<IList<CostType>> GetAll()
        {
            var results = await _costTypeRepository.GetAll();

            return results?.Select(r =>
                r.ToEntity()
            ).ToList();
        }

        public async Task<CostType> GetById(int id)
        {
            var dto = await _costTypeRepository.GetById(id);

            return dto.ToEntity();
        }

        public async Task<CostType> Update(CostType item)
        {
            var dto = await _costTypeRepository.Update(item.ToDto());

            return dto.ToEntity();
        }
    }
}
