using Bogus;
using CostDiary.Api.Data.Repositories;
using CostsDiary.Api.Data.Entities;
using CostsDiary.Api.Web.Controllers;
using CostsDiary.Api.Web.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CostsDiary.Api.UnitTests.Controllers
{
    public class CostItemsControllerTests
    {
        private CostItemsController controller;
        private Mock<ICostTypesRepository> costTypeRepositoryMock;
        private Mock<ICostItemsRepository> costItemRepositoryMock;
        private List<CostType> costTypes;
        private List<CostItem> costItems;

        public CostItemsControllerTests()
        {
            costTypeRepositoryMock = new Mock<ICostTypesRepository>();
            costItemRepositoryMock = new Mock<ICostItemsRepository>();

            controller = new CostItemsController(costItemRepositoryMock.Object, costTypeRepositoryMock.Object);

            var costTypesFaker = new Faker<CostType>()
                .RuleFor(x => x.CostTypeId, f => f.Random.Guid())
                .RuleFor(x => x.CostTypeName, f => f.Name.FirstName());

            costTypes = costTypesFaker.Generate(3);

            var costItemsFaker = new Faker<CostItem>()
                .RuleFor(x => x.CostItemId, f => f.Random.Guid())
                .RuleFor(x => x.CostTypeId, f => f.PickRandom(costTypes).CostTypeId) // costTypes[y.Random.Number(0, 2)].CostTypeId);
                .RuleFor(x => x.DateUsed, f => f.Date.Past(1, DateTime.Now))
                .RuleFor(x => x.ItemName, f => f.Lorem.Word())
                .RuleFor(x => x.Amount, f => f.Finance.Amount(max: 50));

            costItems = costItemsFaker.Generate(10).OrderBy(x => x.DateUsed).ToList();

            costTypeRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(costTypes);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllItems()
        {
            // arrange            
            costItemRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(costItems);

            // act
            var result = await controller.Get();

            // assert
            costTypeRepositoryMock.Verify(s => s.GetAll(), Times.Once());
            costItemRepositoryMock.Verify(s => s.GetAll(), Times.Once());

            var okObjectResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var viewModel = okObjectResult.Value.Should().BeOfType<List<CostItemViewModel>>().Subject;

            viewModel.Count.Should().Be(costItems.Count);

            var firstItem = viewModel.First();
            firstItem.CostItemId.Should().Be(costItems.First().CostItemId);
            firstItem.Amount.Should().Be(costItems.First().Amount);
        }
    }
}
