using CostsDiary.Business.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostsDiary.Services
{
    public interface ICostTypeService
    {
        Task<IList<CostType>> GetAll();
        Task<CostType> GetById(int id);
        Task<CostType> Add(CostType item);
        Task<CostType> Update(CostType item);
        Task Delete(int id);
    }
}
