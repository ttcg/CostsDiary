using System.Collections.Generic;
using System.Threading.Tasks;
using CostsDiary.Domain.Entities;
using CostsDiary.Services.Repositories;
using System.Linq;

namespace CostsDiary.Data.Repositories
{
    public class CostTypeRepositoryInMemory : ICostTypeRepository
    {
        private IList<CostType> _costTypes;
        private int _currentId;

        public CostTypeRepositoryInMemory()
        {
            _costTypes = new List<CostType>()
            {
                new CostType()
                {
                    CostTypeId = 1,
                    CostTypeDescription = "TW Food"
                },
                new CostType()
                {
                    CostTypeId = 2,
                    CostTypeDescription = "TW Others"
                },
                new CostType()
                {
                    CostTypeId = 3,
                    CostTypeDescription = "AP"
                },
                new CostType()
                {
                    CostTypeId = 4,
                    CostTypeDescription = "Others"
                },
                new CostType()
                {
                    CostTypeId = 5,
                    CostTypeDescription = "Groceries"
                },
                new CostType()
                {
                    CostTypeId = 6,
                    CostTypeDescription = "Diesel"
                },
                new CostType()
                {
                    CostTypeId = 7,
                    CostTypeDescription = "NA"
                }
            };

            _currentId = _costTypes.Count;
        }

        public async Task<CostType> Add(CostType item)
        {
            return await Task.Run(() =>
            {                
                item.CostTypeId = ++_currentId;
                _costTypes.Add(item);
                return item;
            });
        }

        public async Task Delete(int id)
        {
            await Task.Run(() =>
            {
                var item = _costTypes.SingleOrDefault(c => c.CostTypeId == id);
                if (item == null)
                    throw new KeyNotFoundException($"The given id: {id} is not found in the list.");

                _costTypes.Remove(item);
            });
        }

        public async Task<IList<CostType>> GetAll()
        {
            return await Task.Run(() =>
            {
                return _costTypes;
            });
        }

        public async Task<CostType> GetById(int id)
        {
            return await Task.Run(() =>
            {
                return _costTypes.SingleOrDefault(c => c.CostTypeId == id);
            });
        }

        public async Task<CostType> Update(CostType item)
        {
            return await Task.Run(() =>
            {
                var oldItem = _costTypes.SingleOrDefault(c => c.CostTypeId == item.CostTypeId);
                if (oldItem == null)
                    throw new KeyNotFoundException($"The given id: {item.CostTypeId} is not found in the list.");

                oldItem.CostTypeDescription = item.CostTypeDescription;

                return oldItem;
            });
        }
    }
}
