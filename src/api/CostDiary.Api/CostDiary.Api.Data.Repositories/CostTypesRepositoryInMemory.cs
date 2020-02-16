using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CostDiary.Api.Data.Repositories;
using System;
using CostsDiary.Api.Data.Entities;

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
                    CostTypeId = new Guid("e401b4e5-b3c6-4855-8846-9f83005391a7"),
                    CostTypeName = "TW Food"
                },
                new CostType()
                {
                    CostTypeId = new Guid("7a9e65c0-2a68-49c9-9038-1c5e033908a8"),
                    CostTypeName = "TW Others"
                },
                new CostType()
                {
                    CostTypeId = new Guid("7d7483a0-73d4-421c-821f-1ac8f68ad579"),
                    CostTypeName = "AP"
                },
                new CostType()
                {
                    CostTypeId = new Guid("55337e4b-7392-489e-a633-17bc7cf8e9db"),
                    CostTypeName = "Others"
                },
                new CostType()
                {
                    CostTypeId = new Guid("a10b83aa-795a-460a-b0a1-0b051871f46c"),
                    CostTypeName = "Groceries"
                },
                new CostType()
                {
                    CostTypeId = new Guid("4c5cedd5-c144-4225-92ff-2e472ed6c274"),
                    CostTypeName = "Diesel"
                },
                new CostType()
                {
                    CostTypeId = new Guid("30f4ecf3-fd76-4022-bc1b-598a7cdb6179"),
                    CostTypeName = "NA"
                },
                new CostType()
                {
                    CostTypeId = new Guid("a828a85d-916d-4a83-bab6-8ef5457214d4"),
                    CostTypeName = "Dinner"
                }
            };
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
    }
}
