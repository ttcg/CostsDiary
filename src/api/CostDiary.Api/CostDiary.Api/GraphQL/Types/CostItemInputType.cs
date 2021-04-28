using CostsDiary.Api.Data.Entities;
using GraphQL.Types;

namespace CostsDiary.Api.Web.GraphQL.Types
{
    public class CostItemInputType : InputObjectGraphType<CostItem>
    {
        public CostItemInputType()
        {
            Name = "CostItemInput";
            Description = "Input type of Cost Item";

            Field(r => r.CostItemId, type: typeof(NonNullGraphType<GuidGraphType>)).Description("A Unique identifier of a cost item");
            Field(r => r.ItemName, type: typeof(NonNullGraphType<StringGraphType>)).Description("Name of a cost item");
            Field(x => x.Amount, type: typeof(NonNullGraphType<DecimalGraphType>)).Description("Value of a cost Item");
            Field(x => x.DateUsed, type: typeof(NonNullGraphType<DateGraphType>)).Description("Date of usage");
            Field(r => r.CostTypeId, type: typeof(NonNullGraphType<GuidGraphType>)).Description("Id of a cost type");
        }
    }
}
