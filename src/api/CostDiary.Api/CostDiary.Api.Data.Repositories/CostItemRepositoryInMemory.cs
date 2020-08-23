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
            _costItems = GetDataSeedingItems();
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

        public async Task Delete(Guid id)
        {
            await Task.Run(() =>
            {
                var idx = _costItems.FindIndex(x => x.CostItemId == id);

                if (idx == -1)
                    throw new KeyNotFoundException();

                _costItems.Remove(_costItems[idx]);
            });
        }

        public async Task Reset()
        {
            await Task.Run(() =>
            {
                _costItems = GetDataSeedingItems();
            });
        }

        public async Task<IEnumerable<CostItem>> GetRecordsByFilter(int year, int month)
        {
            return await Task.Run(() =>
            {
                return _costItems.Where(x => x.DateUsed.Year == year && x.DateUsed.Month == month).ToList();
            });
        }

        List<CostItem> GetDataSeedingItems()
        {
            Random rnd = new Random();
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 25);
            var previousMonthDate = currentDate.AddMonths(-1).Date;
            var dateTwoMonthsAgo = currentDate.AddMonths(-2).Date;
            var previousYearDate = currentDate.AddYears(-1).Date;

            return new List<CostItem>()
            {
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Aldi",
                    CostTypeId = new Guid(CostTypeConstants.Groceries),
                    DateUsed = DateTime.Now.Date,
                    Amount = 20.25m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Sainsbury",
                    CostTypeId = new Guid(CostTypeConstants.Groceries),
                    DateUsed = DateTime.Now.AddDays(-1).Date,
                    Amount = 35.40m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Kfc",
                    CostTypeId = new Guid(CostTypeConstants.Others),
                    DateUsed = DateTime.Now.AddDays(-5).Date,
                    Amount = 11.99m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Tesco",
                    CostTypeId = new Guid(CostTypeConstants.Groceries),
                    DateUsed = DateTime.Now.Date,
                    Amount = 5m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Sports Direct",
                    CostTypeId = new Guid(CostTypeConstants.TWOthers),
                    DateUsed = DateTime.Now.Date,
                    Amount = 58.98m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Sainsbury",
                    CostTypeId = new Guid(CostTypeConstants.Groceries),
                    DateUsed = DateTime.Now.AddDays(-2).Date,
                    Amount = 22.40m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Holland & Barretts",
                    CostTypeId = new Guid(CostTypeConstants.Others),
                    DateUsed = DateTime.Now.AddDays(-5).Date,
                    Amount = 12.34m
                },
                // previous month
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Diesel",
                    CostTypeId = new Guid(CostTypeConstants.Diesel),
                    DateUsed = previousMonthDate,
                    Amount = 30m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Sainsbury",
                    CostTypeId = new Guid(CostTypeConstants.Groceries),
                    DateUsed = previousMonthDate.AddDays(-rnd.Next(1,10)).Date,
                    Amount = 19.24m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Boots",
                    CostTypeId = new Guid(CostTypeConstants.Others),
                    DateUsed = previousMonthDate.AddDays(-rnd.Next(1,10)).Date,
                    Amount = 1.99m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Sainsbury",
                    CostTypeId = new Guid(CostTypeConstants.Groceries),
                    DateUsed = previousMonthDate.AddDays(-rnd.Next(1,10)).Date,
                    Amount = 68.7m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Asda",
                    CostTypeId = new Guid(CostTypeConstants.Groceries),
                    DateUsed = previousMonthDate.AddDays(-rnd.Next(1,10)).Date,
                    Amount = 85.76m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "London Tickets",
                    CostTypeId = new Guid(CostTypeConstants.NA),
                    DateUsed = previousMonthDate.AddDays(-rnd.Next(1,10)).Date,
                    Amount = 11.4m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Plumber",
                    CostTypeId = new Guid(CostTypeConstants.Others),
                    DateUsed = previousMonthDate.AddDays(-rnd.Next(1,10)).Date,
                    Amount = 230m
                },
                // < two months
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Diesel",
                    CostTypeId = new Guid(CostTypeConstants.Diesel),
                    DateUsed = dateTwoMonthsAgo,
                    Amount = 20m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "AP Cash",
                    CostTypeId = new Guid(CostTypeConstants.AP),
                    DateUsed = dateTwoMonthsAgo.AddDays(-rnd.Next(1,10)).Date,
                    Amount = 50m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Samsonite Luggage",
                    CostTypeId = new Guid(CostTypeConstants.Others),
                    DateUsed = dateTwoMonthsAgo.AddDays(-rnd.Next(1,10)).Date,
                    Amount = 100.75m
                },
                // last year
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "M&S Jacket",
                    CostTypeId = new Guid(CostTypeConstants.AP),
                    DateUsed = previousYearDate,
                    Amount = 50m
                },
                new CostItem
                {
                    CostItemId = Guid.NewGuid(),
                    ItemName = "Bodyshop",
                    CostTypeId = new Guid(CostTypeConstants.NA),
                    DateUsed = previousYearDate,
                    Amount = 27.2m
                }
            };
        }
    }
}
