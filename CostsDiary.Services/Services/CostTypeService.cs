using System.Collections.Generic;
using System.Threading.Tasks;
using CostsDiary.Business.Entities;
using CostsDiary.Data.Repositories;

namespace CostsDiary.Services
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
            return await _costTypeRepository.Add(item);
        }

        public async Task Delete(int id)
        {
            await _costTypeRepository.Delete(id);
        }

        public async Task<IList<CostType>> GetAll()
        {
            return await _costTypeRepository.GetAll();
        }

        public async Task<CostType> GetById(int id)
        {
            return await _costTypeRepository.GetById(id);
        }

        public async Task<CostType> Update(CostType item)
        {
            return await _costTypeRepository.Update(item);
        }
    }
}
