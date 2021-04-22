using CostsDiary.Api.Web.ViewModels;
using GraphQL.Types;

namespace CostsDiary.Api.Web.GraphQL.Types
{
    public class CostItemType : ObjectGraphType<CostItemViewModel>
    {
        public CostItemType()
        {
            Field(x => x.CostItemId, type: typeof(GuidGraphType));
            Field(x => x.ItemName);
            Field(x => x.Amount, type: typeof(DecimalGraphType));
            Field(x => x.DateUsed, type: typeof(DateGraphType));
            Field<CostTypeType>("costType");
        }
    }
}
