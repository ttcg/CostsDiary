using CostsDiary.Api.Web.ViewModels;
using GraphQL.Types;

namespace CostsDiary.Api.Web.GraphQL.Types
{
    public class CostTypeType : ObjectGraphType<CostTypeViewModel>
    {
        public CostTypeType()
        {
            Field(x => x.CostTypeId, type: typeof(GuidGraphType));
            Field(x => x.CostTypeName);
        }
    }
}
