using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ServiceCommission.Filters;
using ServiceCommission.Repositories;
using ServiceCommission.Utils;

namespace ServiceCommission
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LoadSetting();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDependencyInjection();

            services.AddDbContext<RepositoryContext>();

            services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilterr()));

            //.AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //});

            var key = Encoding.ASCII.GetBytes(Setting.JwtKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
               new OpenApiInfo
               {
                   Title = "Basic template Api",
                   Version = "v1",
                   Description = "API REST with ASP.NET Core",
                   Contact = new OpenApiContact
                   {
                       Name = "Jaelson Cunha",
                       Url = new Uri("https://github.com/jaelsonrc")
                   }
               });
            });


     

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseHsts();
            }


            app.UseRouting();
            app.ExceptionHandler();

            app.UseCors(x => x
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "API REST with ASP.NET Core V1"));

            app.SetInitialDatas();


            app.SetRouteStatic();

        }

        private void LoadSetting()
        {
            Setting.SetDatas(Configuration.GetValue<string>("DefaultEmail"),
                             Configuration.GetValue<string>("PasswordEmail"),
                             Configuration.GetConnectionString("DefaultConnection"),
                             Configuration.GetValue<string>("JwtKey"));
        }
    }
}
