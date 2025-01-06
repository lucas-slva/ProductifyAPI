using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Productify.Api.Middlewares;
using Productify.Api.Validations;
using Productify.Application.Contracts;
using Productify.Application.DTOs;
using Productify.Application.Services;
using Productify.Infrastructure.Contracts;
using Productify.Infrastructure.Data;
using Productify.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var secretKey = builder.Configuration["JwtSettings:SecretKey"]
    ?? throw new InvalidOperationException("Jwt SecretKey not configured!");
var key = Encoding.ASCII.GetBytes(secretKey);

// ✅ Database Configuration
builder.Services.AddDbContext<ProductifyDbContext>(options => options.UseSqlite("Data Source=../Productify.Infrastructure/productify.db"));

// ✅ Dependency Injection (DI)
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));

// ✅ Controllers
builder.Services.AddControllers();

// ✅ AutoMapper Setup
builder.Services.AddAutoMapper(typeof(ProductDto).Assembly, typeof(CategoryDto).Assembly);

// ✅ Token Service and validators
builder.Services.AddScoped<TokenService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();

// ✅ JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = true,
        ValidateLifetime = true  
    };
});

builder.Services.AddAuthorization();

// ✅ Swagger Configuration (com JWT)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Productify API",
        Version = "v1",
        Description = "API to manage products and categories using .NET 9 and JWT Authentication"
    });
    
    // ✅ Configuration to Authorize via JWT in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});     

var app = builder.Build();

// Error logs
app.UseMiddleware<ErrorHandlingMiddleware>();

// ✅ Development Environment - Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Productify API v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz
    });
}

// ✅ Middlewares Essentials
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
