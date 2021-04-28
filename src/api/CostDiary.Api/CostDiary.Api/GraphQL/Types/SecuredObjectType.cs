using CostsDiary.Api.Web.GraphQL.Security;
using GraphQL;
using GraphQL.Types;

namespace CostsDiary.Api.Web.GraphQL.Types
{
    public class SecuredDataGraphType : ObjectGraphType<SecuredDataType>
    {
        public SecuredDataGraphType()
        {
            this.AuthorizeWith(Policies.Authenticated);
            Field(x => x.Id);
            Field(x => x.Description);
            Field(x => x.Code).AuthorizeWith(Policies.M2MShort);
        }        
    }

    public class SecuredDataType
    {
        public string Id { get; set; } 
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
