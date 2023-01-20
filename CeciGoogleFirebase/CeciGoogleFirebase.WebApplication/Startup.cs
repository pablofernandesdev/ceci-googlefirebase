using AutoMapper;
using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.Mapping;
using CeciGoogleFirebase.Infra.CrossCutting.Settings;
using CeciGoogleFirebase.Infra.Data.Context;
using CeciGoogleFirebase.Service.Validators.User;
using CeciGoogleFirebase.WebApplication.Attribute;
using CeciGoogleFirebase.WebApplication.Dependencys;
using CeciGoogleFirebase.WebApplication.Extensions;
using CeciGoogleFirebase.WebApplication.Filters;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Text;

namespace CeciGoogleFirebase.WebApplication
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //dotnet ef migrations add InitialMigration --project CeciGoogleFirebase.Infra.Data --startup-project CeciGoogleFirebase.WebApplication
            //dotnet ef database update --project CeciGoogleFirebase.Infra.Data --startup-project CeciGoogleFirebase.WebApplication
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("CeciDatabase");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddCors();

            services.AddRepository();
            services.AddService(Configuration);
            services.AddControllers();

            services.AddSingleton(Configuration);

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(
                    new MySqlStorage(
                        Configuration.GetConnectionString("HangfireDb"),
                        new MySqlStorageOptions
                        {
                            QueuePollInterval = TimeSpan.FromSeconds(10),
                            JobExpirationCheckInterval = TimeSpan.FromHours(1),
                            CountersAggregateInterval = TimeSpan.FromMinutes(5),
                            PrepareSchemaIfNecessary = true,
                            DashboardJobListLimit = 25000,
                            TransactionTimeout = TimeSpan.FromMinutes(1),
                            TablesPrefix = "Hangfire",
                        }
                    )
                ));

            // Add the processing server as IHostedService
            services.AddHangfireServer(options => options.WorkerCount = 1);

            //settings
            services.Configure<SwaggerSettings>(Configuration.GetSection("SwaggerSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<ExternalProviders>(Configuration.GetSection("ExternalProviders"));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<RoleSettings>(Configuration.GetSection("RoleSettings"));

            //auth
            var key = Encoding.ASCII.GetBytes(Configuration["JwtToken:Secret"]);
            var firebaseProjectId = Configuration.GetSection("ExternalProviders:Firebase:ProjectId").Value;
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
                            ValidateAudience = true,
                            ValidAudience = firebaseProjectId,
                            ValidateLifetime = true
                        };
                    }).AddJwtBearer("ceciAuth", options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme, "ceciAuth");
                defaultAuthorizationPolicyBuilder =
                defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            services.AddAuthorization(options =>
             {
                 options.AddPolicy("Administrator", policy =>
                       policy.RequireRole("administrator"));
             });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Basic", policy =>
                      policy.RequireRole("basic","administrator"));
            });

            //auto mapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserAddValidator>());

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Ceci API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("http://pablodeveloper.rf.gd/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Pablo Fernandes",
                        Email = string.Empty,
                        Url = new Uri("http://pablodeveloper.rf.gd"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("http://pablodeveloper.rf.gd/license"),
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                                   Enter 'Bearer' [space] and then your token in the text input below. 
                                   Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                    new List<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                // get solution documentation xmls
                var xmls = new List<string>(new string[] { Path.Combine(AppContext.BaseDirectory, "CeciGoogleFirebase.WebApplication.xml"),
                    Path.Combine(AppContext.BaseDirectory, "CeciGoogleFirebase.Domain.xml") });

                foreach (var item in xmls)
                {
                    options.IncludeXmlComments(item, includeControllerXmlComments: true);
                }
            });
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

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                //AppPath = "" //The path for the Back To Site link. Set to null in order to hide the Back To  Site link.
                DashboardTitle = "Ceci Hangfire Dashboard",
                Authorization = new[]
                {
                    new HangfireAuthorizationFilter(
                        Configuration.GetSection("HangfireSettings:HangfireUserAuthorized").Value,
                        Configuration.GetSection("HangfireSettings:HangfireAuthorizedPassword").Value
                    )
                }
            });

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(new ResultResponse()
                {
                    Message = exception.Message,
                    Details = exception.ToString()
                }.ToString());
            }));

            // Unauthorized (401) and Forbidden(403) MiddleWare
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) // 401
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(new ResultResponse()
                    {
                        Message = "Token validation has failed. Request access denied"
                    }.ToString());
                }
                else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden) // 403
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(new ResultResponse()
                    {
                        Message = "User does not have permission to access this functionality"
                    }.ToString());
                }
            });

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

            app.UseSwaggerAuthorized();
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ceci API v1"));
        }
    }
}
