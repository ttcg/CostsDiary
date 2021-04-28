using GraphQL.Types;
using Microsoft.AspNetCore.Builder;

namespace CostsDiary.Api.Web.Configurations
{
    public static class GraphQLApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGraphQLApplication(this IApplicationBuilder app)
        {
            app.UseGraphQL<ISchema>();
            app.UseGraphQLPlayground();

            return app;
        }
    }
}
