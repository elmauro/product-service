using MC.ProductService.API.Data;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using MC.ProductService.API.Validators;
using MC.ProductService.API.Data.Repositories;
using MC.ProductService.API.Services.v1;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using MC.ProductService.API.Infrastructure;
using MC.ProductService.API.Options;
using System.Reflection;
using MediatR;
using MC.ProductService.API.Services.v1.Queries;

var builder = WebApplication.CreateBuilder(args);

// Configure the app and set up services

// Setting up logging with Serilog using settings from the app's configuration
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Configures HTTP logging to record the duration of HTTP requests
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.Duration;
});

// Adds support for controllers
builder.Services.AddControllers();

// Adds caching services to store data in memory
builder.Services.AddMemoryCache();

// Configures the database context for ProductDB using PostgreSQL
builder.Services.AddDbContext<ProductDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgresdb")));

// Registers repository and service classes for dependency injection
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddHttpClient<IHttpClientMockApi, HttpClientMockApiService>();
builder.Services.AddSingleton<IStatusCacheService, StatusCacheService>();

// Registers AutoMapper to manage object-object mapping
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Adds API documentation toolsv
builder.Services.AddEndpointsApiExplorer();

// Sets up Fluent Validation for validating models
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();

// Configures Swagger to help create interactive API documentation
ConfigureSwaggerOptions.AddSwagger(builder.Services);

// Adds MediatR for implementing mediator pattern in handling requestsv
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

var app = builder.Build();

// Middleware to enable HTTP logging
app.UseHttpLogging();

// HTTP request pipeline setup

// If the app is not in production, enable Swagger UI
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
        c.RoutePrefix = "product";  // Serve Swagger UI under '/product'
    });

    var option = new RewriteOptions();
    option.AddRedirect("^$", "product/index.html"); // Redirect root URL to Swagger UI
    app.UseRewriter(option);
}

// Logs HTTP requests using Serilog
app.UseSerilogRequestLogging();

// Forces HTTPS for secure connections
app.UseHttpsRedirection();

// Middleware for authorization
app.UseAuthorization();

// Maps controller endpoints
app.MapControllers();

// Starts the application
app.Run();

// Indicates the part of the program to exclude from code coverage measurement
[ExcludeFromCodeCoverage]
public partial class Program
{
}
