using GraphQL.Authorization;
using System.Collections.Generic;
using System.Security.Claims;

namespace CostsDiary.Api.Web.GraphQL.Security
{
    /// <summary>
    /// Custom context class that implements
    /// 
    public class GraphQLUserContext : Dictionary<string, object>, IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }
}
