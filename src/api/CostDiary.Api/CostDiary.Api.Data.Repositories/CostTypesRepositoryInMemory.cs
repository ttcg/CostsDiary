using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CostDiary.Api.Data.Repositories;
using System;
using CostsDiary.Api.Data.Entities;
using System.Linq.Expressions;

namespace CostsDiary.Api.Data.Repositories
{
    public class CostTypesRepositoryInMemory : ICostTypesRepository
    {
        private IList<CostType> _costTypes;

        public CostTypesRepositoryInMemory()
        {
            _costTypes = new List<CostType>()
            {
                new CostType()
                {
                    CostTypeId = new Guid(CostTypeConstants.TwFood),
                    CostTypeName = "TW Food"
                },
                new CostType()
                {
                    CostTypeId = new Guid(CostTypeConstants.TWOthers),
                    CostTypeName = "TW Others"
                },
                new CostType()
                {
                    CostTypeId = new Guid(CostTypeConstants.AP),
                    CostTypeName = "AP"
                },
                new CostType()
                {
                    CostTypeId = new Guid(CostTypeConstants.Others),
                    CostTypeName = "Others"
                },
                new CostType()
                {
                    CostTypeId = new Guid(CostTypeConstants.Groceries),
                    CostTypeName = "Groceries"
                },
                new CostType()
                {
                    CostTypeId = new Guid(CostTypeConstants.Diesel),
                    CostTypeName = "Diesel"
                },
                new CostType()
                {
                    CostTypeId = new Guid(CostTypeConstants.NA),
                    CostTypeName = "NA"
                },
                new CostType()
                {
                    CostTypeId = new Guid(CostTypeConstants.Dinner),
                    CostTypeName = "Dinner"
                }
            };
        }

        public Task<CostType> Add(CostType entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CostType>> Fetch(Expression<Func<CostType, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CostType>> GetAll()
        {
            return await Task.Run(() =>
            {
                return _costTypes;
            });
        }

        public async Task<CostType> GetById(Guid id)
        {
            return await Task.Run(() =>
            {
                return _costTypes.SingleOrDefault(c => c.CostTypeId == id);
            });
        }

        public Task Update(CostType entity)
        {
            throw new NotImplementedException();
        }
    }
}
