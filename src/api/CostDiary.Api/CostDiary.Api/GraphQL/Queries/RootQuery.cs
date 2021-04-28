using CostDiary.Api.Data.Repositories;
using CostsDiary.Api.Data.Entities;
using CostsDiary.Api.Web.GraphQL.Types;
using CostsDiary.Api.Web.ViewModels;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CostsDiary.Api.Web.GraphQL.Queries
{
    public class RootQuery : ObjectGraphType
    {

        public RootQuery(
            ICostTypesRepository costTypesRepository,
            ICostItemsRepository costItemsRepository)
        {
            Name = "Query";

            SetUpCostTypeQueries(costTypesRepository);
            SetUpCostItemQueries(costItemsRepository, costTypesRepository);
            SetUpSecuredQueries();

        }

        private void SetUpCostTypeQueries(ICostTypesRepository costTypesRepository)
        {
            FieldAsync<CostTypeType>(
                "costType",
                arguments: new QueryArguments(
                    new QueryArgument<GuidGraphType>
                    {
                        Name = "id"
                    }
                ),
                resolve: async (context) =>
                {
                    var costTypeId = context.GetArgument<Guid?>("id");

                    return ConvertEntityToViewModel(await costTypesRepository.GetById(costTypeId.Value));
                });

            FieldAsync<ListGraphType<CostTypeType>>(
                "costTypes",
                arguments: new QueryArguments(
                    new QueryArgument<GuidGraphType>
                    {
                        Name = "id"
                    }
                ),
                resolve: async (context) =>
                {
                    var costTypeId = context.GetArgument<Guid?>("id");

                    if (costTypeId.HasValue)
                    {
                        var costType = await costTypesRepository.GetById(costTypeId.Value);

                        var costTypes = new List<CostType>();

                        if (costType != null)
                        {
                            costTypes.Add(costType);
                        }

                        return costTypes.Select(ConvertEntityToViewModel);
                    }

                    return (await costTypesRepository.GetAll()).Select(ConvertEntityToViewModel);
                });
        }

        private void SetUpCostItemQueries(ICostItemsRepository costItemsRepository, ICostTypesRepository costTypesRepository)
        {
            FieldAsync<CostItemType>(
                "costItem",
                arguments: new QueryArguments(
                    new QueryArgument<GuidGraphType>
                    {
                        Name = "id"
                    }
                ),
                resolve: async (context) =>
                {
                    var costItemId = context.GetArgument<Guid?>("id");

                    var costItem = await costItemsRepository.GetById(costItemId.Value);
                    var costType = await costTypesRepository.GetById(costItem.CostTypeId);

                    return new CostItemViewModel
                    {
                        CostItemId = costItem.CostItemId,
                        ItemName = costItem.ItemName,
                        CostType = ConvertEntityToViewModel(costType),
                        Amount = costItem.Amount,
                        DateUsed = costItem.DateUsed
                    };
                });

            FieldAsync<ListGraphType<CostItemType>>(
                "costItems",
                arguments: new QueryArguments(new List<QueryArgument>
                {
                    new QueryArgument<IntGraphType>
                    {
                        Name = "year"
                    },
                    new QueryArgument<IntGraphType>
                    {
                        Name = "month"
                    }
                }),
                resolve: async (context) =>
                {
                    var year = context.GetArgument<int?>("year");
                    var month = context.GetArgument<int?>("month");

                    if (year.HasValue && month.HasValue)
                    {
                        var results = await costItemsRepository.GetRecordsByFilter(year.Value, month.Value);

                        var costTypes = (await costTypesRepository.GetAll()).ToList();

                        return results.Select(x => ConvertEntityToViewModel(x, costTypes)).OrderByDescending(x => x.DateUsed);
                    }

                    //throw new ExecutionError("year and month must be provided");

                    context.Errors.Add(new ExecutionError($"{string.Join('/', context.Path)}: year and month must be provided"));
                    return new List<CostItemType>();
                });
        }

        CostTypeViewModel ConvertEntityToViewModel(CostType model)
        {
            return new CostTypeViewModel
            {
                CostTypeId = model.CostTypeId,
                CostTypeName = model.CostTypeName
            };
        }

        CostItemViewModel ConvertEntityToViewModel(CostItem model, List<CostType> costTypes)
        {
            return new CostItemViewModel
            {
                CostItemId = model.CostItemId,
                ItemName = model.ItemName,
                CostType = ConvertEntityToViewModel(costTypes.Single(c => c.CostTypeId == model.CostTypeId)),
                Amount = model.Amount,
                DateUsed = model.DateUsed
            };
        }

        private void SetUpSecuredQueries()
        {
            Field<SecuredDataGraphType>(                
                "secured",
                resolve: (context) =>
                {
                    return new SecuredDataType
                    {
                        Id = "Id123",
                        Description = "Some Description",
                        Code = "Secured Special Code"
                    };
                });
        }
}

}
