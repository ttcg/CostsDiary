using Bogus;
using CostDiary.Api.Data.Repositories;
using CostsDiary.Api.Data.Entities;
using CostsDiary.Api.Web.Controllers;
using CostsDiary.Api.Web.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        private Faker<CostItem> costItemsFaker;

        public CostItemsControllerTests()
        {
            costTypeRepositoryMock = new Mock<ICostTypesRepository>();
            costItemRepositoryMock = new Mock<ICostItemsRepository>();

            controller = new CostItemsController(costItemRepositoryMock.Object, costTypeRepositoryMock.Object);

            var costTypesFaker = new Faker<CostType>()
                .RuleFor(x => x.CostTypeId, f => f.Random.Guid())
                .RuleFor(x => x.CostTypeName, f => f.Name.FirstName());

            costTypes = costTypesFaker.Generate(3);

            costItemsFaker = new Faker<CostItem>()
                .RuleFor(x => x.CostItemId, f => f.Random.Guid())
                .RuleFor(x => x.CostTypeId, f => f.PickRandom(costTypes).CostTypeId) // costTypes[y.Random.Number(0, 2)].CostTypeId);
                .RuleFor(x => x.DateUsed, f => f.Date.Past(1, DateTime.Now))
                .RuleFor(x => x.ItemName, f => f.Commerce.ProductName())
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

        [Fact]
        public async Task GetById_ShouldReturnNotFoundForNonExistingId()
        {
            // act
            var result = await controller.GetById(Guid.NewGuid());

            // assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetById_ShouldReturnResultForExistingId()
        {
            // arrange            
            var costItem = costItems.First();
            costItemRepositoryMock.Setup(x => x.GetById(costItem.CostItemId)).ReturnsAsync(costItem);

            // act
            var result = await controller.GetById(costItem.CostItemId);

            // assert
            costItemRepositoryMock.Verify(s => s.GetById(costItem.CostItemId), Times.Once());
            var okObjectResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;

            var viewModel = okObjectResult.Value.Should().BeOfType<CostItemViewModel>().Subject;
            viewModel.CostItemId.Should().Be(costItem.CostItemId);
            viewModel.ItemName.Should().Be(costItem.ItemName);
            viewModel.CostType.CostTypeId.Should().Be(costItem.CostTypeId);
            viewModel.Amount.Should().Be(costItem.Amount);
            viewModel.DateUsed.Should().Be(costItem.DateUsed);
        }

        [Fact]
        public async Task Add_ShouldReturnNotFoundForNonExistingCostTypeId()
        {
            // arrange            
            var costItem = costItemsFaker.Generate();

            var costItemViewModel = new CostItemCreateViewModel
            {
                CostItemId = costItem.CostItemId,
                Amount = costItem.Amount,
                CostTypeId = Guid.NewGuid(),
                DateUsed = costItem.DateUsed,
                ItemName = costItem.ItemName
            };

            // act
            var result = await controller.Add(costItemViewModel);

            // assert
            var notFoundObjectResult = result.Result.Should().BeOfType<NotFoundObjectResult>();
            notFoundObjectResult.Subject.Value.ToString().Should().Contain("CostTypeId");
        }

        [Fact]
        public async Task Add_ShouldReturnResult()
        {
            // arrange            
            var costItem = costItemsFaker.Generate();

            var costItemCreateViewModel = new CostItemCreateViewModel
            {
                Amount = costItem.Amount,
                CostTypeId = costItem.CostTypeId,
                DateUsed = costItem.DateUsed,
                ItemName = costItem.ItemName
            };

            costItemRepositoryMock.Setup(x => x.Add(It.IsAny<CostItem>())).ReturnsAsync(costItem);

            // act
            var result = await controller.Add(costItemCreateViewModel);

            // assert
            costItemRepositoryMock.Verify(s => s.Add(It.IsAny<CostItem>()), Times.Once());

            var createdAtRouteResult = result.Result.Should().BeOfType<CreatedAtRouteResult>().Subject;

            var viewModel = createdAtRouteResult.Value.Should().BeOfType<CostItemViewModel>().Subject;
            viewModel.CostItemId.Should().NotBeEmpty();
            viewModel.ItemName.Should().Be(costItemCreateViewModel.ItemName);
            viewModel.CostType.CostTypeId.Should().Be(costItemCreateViewModel.CostTypeId);
            viewModel.Amount.Should().Be(costItemCreateViewModel.Amount);
            viewModel.DateUsed.Should().Be(costItemCreateViewModel.DateUsed);

            createdAtRouteResult.RouteName.Should().Be(nameof(controller.GetById));
            createdAtRouteResult.RouteValues.First().Key.Should().Be("Id");
            createdAtRouteResult.RouteValues.First().Value.Should().Be(viewModel.CostItemId);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFoundForNonExistingCostItemId()
        {
            // arrange
            var costItemUpdateViewModel = new JsonPatchDocument<CostItemPatchViewModel>();

            // act
            var result = await controller.Update(Guid.NewGuid(), costItemUpdateViewModel);

            // assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnNotFoundForNonExistingCostTypeId()
        {
            // arrange
            var costItem = costItems.First();
            costItemRepositoryMock.Setup(x => x.GetById(costItem.CostItemId)).ReturnsAsync(costItem);

            var patch = new JsonPatchDocument<CostItemPatchViewModel>();
            patch.Replace(x => x.CostTypeId, Guid.NewGuid());

            var validatorMock = new Mock<IObjectModelValidator>();

            validatorMock.Setup(x => x.Validate(It.IsAny<ActionContext>(),
                                          It.IsAny<ValidationStateDictionary>(),
                                          It.IsAny<string>(),
                                          It.IsAny<Object>()));

            controller.ObjectValidator = validatorMock.Object;

            // act
            var result = await controller.Update(costItem.CostItemId, patch);

            // assert
            var notFoundObjectResult = result.Should().BeOfType<NotFoundObjectResult>();
            notFoundObjectResult.Subject.Value.ToString().Should().Contain("CostTypeId");
        }

        [Fact]
        public async Task Update_ShouldReturnNotFoundForBadUpdate()
        {
            // arrange
            var costItem = costItems.First();
            costItemRepositoryMock.Setup(x => x.GetById(costItem.CostItemId)).ReturnsAsync(costItem);

            var patch = new JsonPatchDocument<CostItemPatchViewModel>();
            patch.Remove(x => x.CostTypeId);
            patch.Remove(x => x.Amount);
            patch.Remove(x => x.ItemName);

            var validatorMock = new Mock<IObjectModelValidator>();

            validatorMock.Setup(x => x.Validate(It.IsAny<ActionContext>(),
                                          It.IsAny<ValidationStateDictionary>(),
                                          It.IsAny<string>(),
                                          It.IsAny<Object>()));

            controller.ObjectValidator = validatorMock.Object;

            // act
            var result = await controller.Update(costItem.CostItemId, patch);

            // assert
            var notFoundObjectResult = result.Should().BeOfType<NotFoundObjectResult>();
            notFoundObjectResult.Subject.Value.ToString().Should().Contain("CostTypeId");
        }

        [Fact]
        public async Task Update_ShouldReturnNoContentResultForSuccessfulUpdate()
        {
            // arrange
            var costItem = costItems.First();
            costItemRepositoryMock.Setup(x => x.GetById(costItem.CostItemId)).ReturnsAsync(costItem);
            costItemRepositoryMock.Setup(r => r.Update(It.IsAny<CostItem>()));

            var faker = new Faker();

            var patch = new JsonPatchDocument<CostItemPatchViewModel>();
            patch.Replace(x => x.Amount, decimal.Parse(faker.Commerce.Price()));
            patch.Replace(x => x.ItemName, faker.Commerce.ProductName());
            patch.Replace(x => x.DateUsed, faker.Date.Past());

            var validatorMock = new Mock<IObjectModelValidator>();

            validatorMock.Setup(x => x.Validate(It.IsAny<ActionContext>(),
                                          It.IsAny<ValidationStateDictionary>(),
                                          It.IsAny<string>(),
                                          It.IsAny<Object>()));

            controller.ObjectValidator = validatorMock.Object;

            // act
            var result = await controller.Update(costItem.CostItemId, patch);

            // assert
            costItemRepositoryMock.Verify(r => r.Update(It.IsAny<CostItem>()), Times.Once);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFoundForNonExistingCostItemId()
        {
            // act
            var result = await controller.Delete(Guid.NewGuid());

            // assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContentResultForSuccessfulAction()
        {
            // arrange
            var costItem = costItems.First();
            costItemRepositoryMock.Setup(x => x.GetById(costItem.CostItemId)).ReturnsAsync(costItem);

            costItemRepositoryMock.Setup(r => r.Delete(costItem.CostItemId));

            // act
            var result = await controller.Delete(costItem.CostItemId);

            // assert
            costItemRepositoryMock.Verify(r => r.Delete(costItem.CostItemId), Times.Once);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void GetOptions_ShouldReturnCorrectHeaders()
        {
            // arrange
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };

            // act
            var result = controller.GetOptions();

            // assert
            result.Should().BeOfType<OkResult>();

            controller.Response.Headers.TryGetValue("allow", out var val);
            val.ToString().ToLower().Should().BeEquivalentTo("GET,OPTIONS,POST,PUT,DELETE".ToLower());
        }

        [Fact]
        public async Task Reset_ShouldReturnNoContentResultForSuccessfulAction()
        {
            // arrange
            costItemRepositoryMock.Setup(x => x.Reset());

            // act
            var result = await controller.Reset();

            // assert
            costItemRepositoryMock.Verify(x => x.Reset(), Times.Once);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Filter_ShouldReturnRecordsForCriteria()
        {
            // arrange
            var firstItem = costItems.First();
            var itemsToReturn = costItems.Where(x => x.DateUsed.Year == firstItem.DateUsed.Year && x.DateUsed.Month == firstItem.DateUsed.Month).ToList();
            costItemRepositoryMock.Setup(x => x.GetRecordsByFilter(firstItem.DateUsed.Year, firstItem.DateUsed.Month)).ReturnsAsync(itemsToReturn);

            // act
            var result = await controller.Filter(firstItem.DateUsed.Year, firstItem.DateUsed.Month);

            // assert
            costItemRepositoryMock.Verify(x => x.GetRecordsByFilter(firstItem.DateUsed.Year, firstItem.DateUsed.Month), Times.Once);
            var okObjectResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var viewModel = okObjectResult.Value.Should().BeOfType<List<CostItemViewModel>>().Subject;

            viewModel.Count.Should().Be(itemsToReturn.Count);

            viewModel.Single(x => x.CostItemId == firstItem.CostItemId).Should().NotBeNull();
        }
    }
}
