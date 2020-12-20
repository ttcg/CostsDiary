using CostDiary.Api.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CostsDiary.Api.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        public Task<T> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> Fetch(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
