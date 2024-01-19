using Assist.Lunch._4.Core.Helpers.Encrypter;
using Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles;
using Assist.Lunch._4.Core.Interfaces;
using Assist.Lunch._4.Core.Models;
using Assist.Lunch._4.Core.Security.Utils;
using Assist.Lunch._4.Core.Services;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Assist.Lunch._4.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Assist.Lunch._4.Extensions
{
    public static class ApplicationExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<IDestinationService, DestinationService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IDestinationRepository, DestinationRepository>();
            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddHelpers(this IServiceCollection services)
        {
            services.AddScoped<IPasswordManager, PasswordManager>();
            services.AddAutoMapper(Assembly.GetAssembly(typeof(UserProfileMapper)));
        }

        public static void AddTokenGenerator(this IServiceCollection services)
        {
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<TokenSettings>();
        }

        public static void AddLogger(this WebApplicationBuilder builder)
        {
            builder.Logging.AddFile("logger/logger.txt");
        }

        public static void AddJwtAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = bool.Parse(builder.Configuration["JsonWebTokenKeys:ValidateIssuer"]),
                    ValidateAudience = bool.Parse(builder.Configuration["JsonWebTokenKeys:ValidateAudience"]),
                    ValidateLifetime = bool.Parse(builder.Configuration["JsonWebTokenKeys:ValidateLifetime"]),
                    ValidateIssuerSigningKey = bool.Parse(builder.Configuration["JsonWebTokenKeys:ValidateIssuerSigningKey"]),
                    RequireExpirationTime = bool.Parse(builder.Configuration["JsonWebTokenKeys:RequireExpirationTime"]),
                    ValidIssuer = builder.Configuration["JsonWebTokenKeys:ValidIssuer"],
                    ValidAudience = builder.Configuration["JsonWebTokenKeys:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JsonWebTokenKeys:IssuerSigningKey"]))
                };
            });
        }

        public static void AddSwaggerGen(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Assist.Lunch.4",
                    Version = "v1"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
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
                            Scheme = "ApiKey",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
