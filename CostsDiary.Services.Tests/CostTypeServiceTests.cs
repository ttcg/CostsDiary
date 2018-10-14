using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System;

namespace CostsDiary.Services.Tests
{
    public class CostTypeServiceTests
    {
        private readonly ICostTypeService _costTypeService;
        public CostTypeServiceTests()
        {
            _costTypeService = new Services.CostTypeService(new Data.Repositories.CostTypeRepositoryInMemory());
        }

        [Fact]
        public async Task GetAllShouldReturnList()
        {
            var result = await _costTypeService.GetAll();
            result.Count.Should().Be(7);
        }

        [Fact]
        public async Task GetByIdShouldSucceed()
        {
            var result = await _costTypeService.GetById(1);
            result.CostTypeDescription.Should().BeEquivalentTo("TW Food");
        }

        [Fact]
        public async Task AddShouldSucceed()
        {
            var itemToAdd = new Domain.Entities.CostType() { CostTypeDescription = "new item" };
            var result = await _costTypeService.Add(itemToAdd);
            result.CostTypeDescription.Should().BeEquivalentTo(itemToAdd.CostTypeDescription);
        }

        [Fact]
        public async Task UpdateShouldSucceed()
        {
            var itemToUpdate = new Domain.Entities.CostType() { CostTypeId = 2, CostTypeDescription = "new desc" };
            var result = await _costTypeService.Update(itemToUpdate);
            result.CostTypeDescription.Should().BeEquivalentTo(itemToUpdate.CostTypeDescription);
        }

        [Fact]
        public void UpdateShouldThrowError()
        {
            var itemToUpdate = new Domain.Entities.CostType() { CostTypeId = 9, CostTypeDescription = "new desc" };
            Func<Task> func = async () => await _costTypeService.Update(itemToUpdate);
            func.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public async Task DeleteShouldSucceed()
        {
            await _costTypeService.Delete(3);
            var result = await _costTypeService.GetById(3);
            result.Should().BeNull();
        }

        [Fact]
        public void DeleteShouldThrowError()
        {
            Func<Task> func = async () => await _costTypeService.Delete(999);
            func.Should().Throw<KeyNotFoundException>();
        }
    }
}
