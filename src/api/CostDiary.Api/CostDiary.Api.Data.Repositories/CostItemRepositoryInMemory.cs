using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CostDiary.Api.Data.Repositories;
using System;
using CostsDiary.Api.Data.Entities;

namespace CostsDiary.Api.Data.Repositories
{
    public class CostItemsRepositoryInMemory : ICostItemsRepository
    {
        private List<CostItem> _costItems;

        public CostItemsRepositoryInMemory()
        {
            _costItems = new List<CostItem>()
            {
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Sainsbury",
                    CostTypeId = new Guid("a10b83aa-795a-460a-b0a1-0b051871f46c"),
                    DateUsed = new DateTime(2020, 1, 2),
                    Amount = 19.24m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Sainsbury",
                    CostTypeId = new Guid("a10b83aa-795a-460a-b0a1-0b051871f46c"),
                    DateUsed = new DateTime(2020, 1, 2),
                    Amount = 19.24m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Boots",
                    CostTypeId = new Guid("a10b83aa-795a-460a-b0a1-0b051871f46c"),
                    DateUsed = new DateTime(2020, 1, 5),
                    Amount = 1.99m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Sainsbury",
                    CostTypeId = new Guid("a10b83aa-795a-460a-b0a1-0b051871f46c"),
                    DateUsed = new DateTime(2020, 1, 5),
                    Amount = 68.7m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Asda",
                    CostTypeId = new Guid("a10b83aa-795a-460a-b0a1-0b051871f46c"),
                    DateUsed = new DateTime(2020, 1, 5),
                    Amount = 85.76m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "London Tickets",
                    CostTypeId = new Guid("30f4ecf3-fd76-4022-bc1b-598a7cdb6179"),
                    DateUsed = new DateTime(2020, 1, 7),
                    Amount = 11.4m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Diesel",
                    CostTypeId = new Guid("4c5cedd5-c144-4225-92ff-2e472ed6c274"),
                    DateUsed = new DateTime(2020, 1, 15),
                    Amount = 20m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "AP Cash",
                    CostTypeId = new Guid("7d7483a0-73d4-421c-821f-1ac8f68ad579"),
                    DateUsed = new DateTime(2020, 2, 7),
                    Amount = 50m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Samsonite Luggage",
                    CostTypeId = new Guid("55337e4b-7392-489e-a633-17bc7cf8e9db"),
                    DateUsed = new DateTime(2020, 2, 10),
                    Amount = 100.75m
                }
            };
        }

        public async Task<CostItem> Add(CostItem costItem)
        {
            return await Task.Run(() =>
            {
                if (costItem.CostItemId == Guid.Empty)
                    costItem.CostItemId = Guid.NewGuid();

                _costItems.Add(costItem);
                return costItem;
            });
        }

        public async Task Update(CostItem costItem)
        {
            await Task.Run(() =>
            {
                var idx = _costItems.FindIndex(x => x.CostItemId == costItem.CostItemId);

                if (idx == -1)
                    throw new KeyNotFoundException();

                _costItems[idx] = costItem;
            });
        }

        public async Task<IEnumerable<CostItem>> GetAll()
        {
            return await Task.Run(() =>
            {
                return _costItems;
            });
        }

        public async Task<CostItem> GetById(Guid id)
        {
            return await Task.Run(() =>
            {
                return _costItems.SingleOrDefault(c => c.CostItemId == id);
            });
        }
    }
}
