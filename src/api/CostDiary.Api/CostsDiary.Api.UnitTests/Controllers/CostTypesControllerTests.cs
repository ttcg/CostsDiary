using CostDiary.Api.Data.Repositories;
using CostsDiary.Api.Data.Entities;
using CostsDiary.Api.Web.Controllers;
using CostsDiary.Api.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Bogus;

namespace CostsDiary.Api.UnitTests.Controllers
{
    public class CostTypesControllerTests
    {
        private CostTypesController controller;
        private Mock<ICostTypesRepository> costTypeRepositoryMock;
        private List<CostType> costTypes;

        public CostTypesControllerTests()
        {
            costTypeRepositoryMock = new Mock<ICostTypesRepository>();

            controller = new CostTypesController(costTypeRepositoryMock.Object);

            var costTypesfaker = new Faker<CostType>()
                .RuleFor(x => x.CostTypeId, f => f.Random.Guid())
                .RuleFor(x => x.CostTypeName, f => f.Name.FirstName());

            costTypes = costTypesfaker.Generate(3);
        }

        [Fact]
        public async Task GetAll_ShouldReturnCostTypes()
        {
            // arrange            
            costTypeRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(costTypes);

            // act
            var result = await controller.Get();

            // assert
            costTypeRepositoryMock.Verify(s => s.GetAll(), Times.Once());

            var okObjectResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var viewModel = okObjectResult.Value.Should().BeOfType<List<CostTypeViewModel>>().Subject;

            viewModel.Count.Should().Be(costTypes.Count);

            var firstItem = viewModel.First();
            firstItem.CostTypeId.Should().Be(costTypes.First().CostTypeId);
            firstItem.CostTypeName.Should().Be(costTypes.First().CostTypeName);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFoundForNonExistingId()
        {
            // act
            var result = await controller.Get(Guid.NewGuid());

            // assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Get_ShouldReturnResultForExistingId()
        {
            // arrange            
            var costTypeId = costTypes.First().CostTypeId;
            costTypeRepositoryMock.Setup(x => x.GetById(costTypeId)).ReturnsAsync(costTypes.First);

            // act
            var result = await controller.Get(costTypeId);

            // assert
            costTypeRepositoryMock.Verify(s => s.GetById(costTypeId), Times.Once());
            var okObjectResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;

            var viewModel = okObjectResult.Value.Should().BeOfType<CostTypeViewModel>().Subject;
            viewModel.CostTypeId.Should().Be(costTypes.First().CostTypeId);
            viewModel.CostTypeName.Should().Be(costTypes.First().CostTypeName);
        }

        [Fact]
        public void GetOptions_ShouldReturnCorrectHeaders()
        {
            // arrange
            var httpContext = new DefaultHttpContext();

            controller = new CostTypesController(costTypeRepositoryMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
            
            // act
            var result = controller.GetOptions();

            // assert
            result.Should().BeOfType<OkResult>();

            controller.Response.Headers.TryGetValue("allow", out var val);
            val.ToString().ToLower().Should().BeEquivalentTo("get");
        }
    }
}
