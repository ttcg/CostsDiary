using CostDiary.Api.Data.Repositories;
using CostsDiary.Api.Data.Repositories;
using CostsDiary.Api.Web.GraphQL.Schemas;
using FluentValidation.AspNetCore;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace CostDiary.Api.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICostTypesRepository, CostTypesRepositoryInMemory>();
            services.AddSingleton<ICostItemsRepository, CostItemsRepositoryInMemory>();

            services.AddLogging(builder => builder.AddConsole());

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CostDiary Apis", Version = "v1" });
                c.EnableAnnotations();
            });

            services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddFluentValidation(options => options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddHealthChecks();

            services.AddSingleton<ISchema, CostDiarySchema>(services => new CostDiarySchema(new SelfActivatingServiceProvider(services)));

            services.AddGraphQL(options =>
                {
                    options.EnableMetrics = true;
                })
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                .AddSystemTextJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseGraphQL<ISchema>();
            app.UseGraphQLPlayground();         

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cost Diary Api");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}
