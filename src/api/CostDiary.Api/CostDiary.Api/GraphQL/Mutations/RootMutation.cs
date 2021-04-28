using CostDiary.Api.Data.Repositories;
using CostsDiary.Api.Data.Entities;
using CostsDiary.Api.Web.GraphQL.Types;
using GraphQL;
using GraphQL.Types;

namespace CostsDiary.Api.Web.GraphQL.Mutations
{
    public class RootMutation : ObjectGraphType
    {
        public RootMutation(
            ICostTypesRepository costTypesRepository,
            ICostItemsRepository costItemsRepository)
        {
            Name = "Mutation";

            SetUpCostItemMutations(costItemsRepository, costTypesRepository);
        }

        private void SetUpCostItemMutations(ICostItemsRepository costItemsRepository, ICostTypesRepository costTypesRepository)
        {
            FieldAsync<GuidGraphType>(
                "addCostItem",
                "Add Cost Item to the database.",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<CostItemInputType>>
                    {
                        Name = "input",
                        Description = "Cost item detail"
                    }),
                async (context) =>
                {
                    var costItem = context.GetArgument<CostItem>("input");

                    var costType = await costTypesRepository.GetById(costItem.CostTypeId);

                    if (costType == null)
                    {
                        context.Errors.Add(new ExecutionError($"cost Type {costItem.CostTypeId}: not found"));
                        return null;
                    }

                    if (await costItemsRepository.GetById(costItem.CostItemId) != null)
                    {
                        context.Errors.Add(new ExecutionError($"cost Item Id {costItem.CostItemId}: already exists.  Please provide a new Unique Id"));
                        return null;
                    }

                    return (await costItemsRepository.Add(costItem)).CostItemId;
                });
        }
    }
}
