using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using ServiceCommission.Exceptions;
using ServiceCommission.Models;
using ServiceCommission.Repositories;
using ServiceCommission.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission.Utils
{
    public static class SettingsExtensions
    {
        public static void AddDependencyInjection(this IServiceCollection service)
        {
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<ICommissionRepository, CommissionRepository>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            service.AddSingleton(mapper);

        }

        public static void ExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature.Error;
                var result = string.Empty;

                if (feature?.Error is HttpException)
                {
                    result = JsonConvert.SerializeObject(((HttpException)feature.Error).Value);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = ((HttpException)feature.Error).Status;
                    await context.Response.WriteAsync(result);
                }

                result = JsonConvert.SerializeObject(new { error = "Error not specified. Contact your system administrator!" });
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(result);
            }));

        }


        public static void SetInitialDatas(this IApplicationBuilder app)
        {
            using var context = RepositoryContext.Build();

            var user = context.Users.FirstOrDefault(f => f.Login == "demo@demo");
            if (user != null) return;

            user = new User()
            {
                Login = "admin",
                Password = "123456",
                Email = "jaelsonrc@ymail.com",

            };
            user.Password = user.GetHash();
            context.Add(user);
            context.SaveChanges();
        }

        public static void SetRouteStatic(this IApplicationBuilder app)
        {

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = new PathString("/scommission")
            });


        }

    }
}
