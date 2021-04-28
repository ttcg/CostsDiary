using CostsDiary.Api.Web.GraphQL.Schemas;
using CostsDiary.Api.Web.GraphQL.Security;
using GraphQL.Authorization;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.Types;
using GraphQL.Validation;
using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace CostsDiary.Api.Web.Configurations
{
    public static class GraphQLServiceCollectionExtension
    {
        public static IGraphQLBuilder AddGraphQLService(this IServiceCollection services)
        {
            services.AddSingleton<ISchema, CostDiarySchema>(services => new CostDiarySchema(new SelfActivatingServiceProvider(services)));

            services.AddGraphQLAuth((settings, provider) => {
                settings.AddPolicy(Policies.Authenticated, x => x.RequireAuthenticatedUser());
                settings.AddPolicy(Policies.M2MShort, x => x.RequireClaim(JwtClaimTypes.ClientId, "m2m.short"));
            });

            return services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;                
            })
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                .AddSystemTextJson()
                .AddUserContextBuilder(context => new GraphQLUserContext { User = context.User }); ;
        }

        /// <summary>
        /// Adds all necessary classes into provided <paramref name="services"/>
        /// and provides a delegate to configure authorization settings.
        /// </summary>
        public static void AddGraphQLAuth(this IServiceCollection services, Action<AuthorizationSettings, IServiceProvider> configure)
        {
            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();

            services.TryAddTransient(s =>
            {
                var authSettings = new AuthorizationSettings();
                configure(authSettings, s);
                return authSettings;
            });
        }

        /// <summary>
        /// Adds all necessary classes into provided <paramref name="services"/>
        /// and provides a delegate to configure authorization settings.
        /// </summary>
        public static void AddGraphQLAuth(this IServiceCollection services, Action<AuthorizationSettings> configure)
        {
            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();

            services.TryAddTransient(s =>
            {
                var authSettings = new AuthorizationSettings();
                configure(authSettings);
                return authSettings;
            });
        }
    }
}
