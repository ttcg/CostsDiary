﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CostsDiary.Data.Entities;

namespace CostsDiary.Data.Repositories
{
    public interface ICostTypeRepository
    {
        Task<IList<CostType>> GetAll();
        Task<CostType> GetById(int id);
        Task<CostType> Add(CostType item);
        Task<CostType> Update(CostType item);
        Task Delete(int id);
    }
}
