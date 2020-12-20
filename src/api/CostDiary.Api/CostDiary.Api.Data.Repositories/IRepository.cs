using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CostDiary.Api.Data.Repositories
{
    public interface IRepository<T> 
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> Fetch(Expression<Func<T, bool>> expression);
        Task<T> GetById(Guid id);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(Guid id);
    }
}
