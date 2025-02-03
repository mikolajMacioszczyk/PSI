using Application.Interfaces;
using Application.Requests.Orders.CreateOrder;
using Application.Requests.Purchases.CreateCheckoutSession;

using Application.Services;
using Common.Application.Interfaces;
using Common.Application.Services;
using Common.Infrastructure.AuthenticationAdapters;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Persistence;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHttpContextAccessor();
        services.AddScoped<IIdentityService, KeycloakIdentityService>();
        
        services.AddScoped<IBasketService, HttpBasketService>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IOrderPriceService, OrderPriceService>();
        services.AddScoped<IOrderNumberService, OrderNumberService>();
        services.AddScoped<IShipmentService, ShipmentService>();
        services.AddScoped<IInventoryService, HttpInventoryService>();
    }

    public static void AddFluentValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();

        services.AddScoped<IValidator<CreateCheckoutSessionCommand>, CreateCheckoutSessionCommandValidator>();

    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            AddBearerHeader(c);
            c.SupportNonNullableReferenceTypes();
        });
    }

    private static void AddBearerHeader(SwaggerGenOptions options)
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        options.OperationFilter<AddAuthorizationHeaderOperationHeader>();

        options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    }
}
