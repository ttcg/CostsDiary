using CostsDiary.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostDiary.Api.Data.Repositories
{
    public interface ICostItemsRepository
    {
        Task<IEnumerable<CostItem>> GetAll();
        Task<CostItem> GetById(Guid id);
        Task<CostItem> Add(CostItem costItem);
        Task Update(CostItem costItem);
    }
}
