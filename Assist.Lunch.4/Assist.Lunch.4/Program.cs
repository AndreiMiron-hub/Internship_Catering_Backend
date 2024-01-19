using Assist.Lunch._4.Core.Helpers.ExceptionHandler;
using Assist.Lunch._4.Core.Helpers.Mapper;
using Assist.Lunch._4.Core.Helpers.Validators.UserValidators;
using Assist.Lunch._4.Core.Models;
using Assist.Lunch._4.Core.Security.Utils;
using Assist.Lunch._4.Extensions;
using Assist.Lunch._4.Infrastructure.Contexts;
using FluentValidation.AspNetCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var allowSpecificOrigins = "allowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
var sqlConnectionBuilder = new SqlConnectionStringBuilder(
    builder
    .Configuration
    .GetConnectionString("SqlDbConnectionString"));

builder.Services.AddControllers()
    .AddFluentValidation(fluentValidatorConfig => fluentValidatorConfig.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(RegisterUserDtoValidator))));

builder.Services.AddEndpointsApiExplorer();

builder.AddSwaggerGen();

var jwtSettings = builder.Configuration.GetSection("TokenSettings");

builder.AddJwtAuthentication();

builder.Services.AddHttpContextAccessor();
builder.Services.AdCustomConfiguredAutoMapper();
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

builder.Services.AddServices();
builder.Services.AddHelpers();
builder.Services.AddTokenGenerator();
builder.AddLogger();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(sqlConnectionBuilder.ConnectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
                      policy =>
                      {
                          policy
                          .WithOrigins("*")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
