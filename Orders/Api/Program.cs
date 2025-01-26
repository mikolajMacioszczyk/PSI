using Api.Extensions;
using Application.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Reflection;
using System.Text.Json.Serialization;

const string AllowAllCorsPolicy = "AllowAll";
const string DbConnectionString = "Db";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// swagger
builder.Services.AddSwagger();

// db context
builder.Services.AddDbContextPool<OrdersContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(DbConnectionString), builder =>
    {
        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        builder.MaxBatchSize(1);
    });
});

// migrator
builder.Services.AddHostedService<OrdersDbMigrator>();

// application dependencies
builder.Services.AddApplicationDependencies();

// mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(IUnitOfWork))!));

// autoMapper
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(IUnitOfWork)));

// fluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddFluentValidators();

builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowAllCorsPolicy, builder =>
    {
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true);
    });
});

// tokena validation
builder.Services.AddKeycloakJwtAuthentication(builder, builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllowAllCorsPolicy);

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
