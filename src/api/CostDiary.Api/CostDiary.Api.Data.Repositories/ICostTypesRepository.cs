using CostsDiary.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostDiary.Api.Data.Repositories
{
    public interface ICostTypesRepository
    {
        Task<IEnumerable<CostType>> GetAll();
        Task<CostType> GetById(Guid id);
    }
}
