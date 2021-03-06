﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsDiary.Web.Extensions
{
    public static class AspNetCoreGlobalExceptionHandlerExtension
    {
        public static IApplicationBuilder UseAspNetCoreExceptionHandler(this IApplicationBuilder app)
        {
            var loggerFactory = app.ApplicationServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;

            return app.UseExceptionHandler(HandleException(loggerFactory));
        }

        public static Action<IApplicationBuilder> HandleException(ILoggerFactory loggerFactory)
        {
            return appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionHandlerFeature != null)
                    {
                        var logger = loggerFactory.CreateLogger("Serilog Global exception logger");
                        logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                    }
                });
            };
        }
    }
}
