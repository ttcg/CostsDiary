using GraphQL.Types;
using System;
using GraphQL.Utilities;
using CostsDiary.Api.Web.GraphQL.Queries;
using Microsoft.Extensions.DependencyInjection;
using CostsDiary.Api.Web.GraphQL.Mutations;

namespace CostsDiary.Api.Web.GraphQL.Schemas
{
    public class CostDiarySchema : Schema
    {
        public CostDiarySchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<RootQuery>();
            Mutation = provider.GetRequiredService<RootMutation>();
        }
    }
}
