using GraphQL.Types;
using System;
using GraphQL.Utilities;
using CostsDiary.Api.Web.GraphQL.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace CostsDiary.Api.Web.GraphQL.Schemas
{
    public class CostDiarySchema : Schema
    {
        public CostDiarySchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<RootQuery>();
        }
    }
}
