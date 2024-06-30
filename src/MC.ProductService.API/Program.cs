using MC.ProductService.API.Data;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using MC.ProductService.API.Options;
using MC.ProductService.API.Data.Repositories;
using MC.ProductService.API.Application.v1;
using FluentValidation.AspNetCore;
using FluentValidation;
using MyAccount.Web.Validators;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using MC.ProductService.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.Duration;
});

builder.Services.AddControllers();

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<ProductDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgresdb")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddHttpClient<IHttpClientMockApi, HttpClientMockApiService>();
builder.Services.AddSingleton<IStatusCacheService, StatusCacheService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

// Configure Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();

ConfigureSwaggerOptions.AddSwagger(builder.Services);

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
        c.RoutePrefix = "product";  // Serve Swagger UI at the root under 'product'
    });

    var option = new RewriteOptions();
    option.AddRedirect("^$", "product/index.html"); // Adjusted redirection
    app.UseRewriter(option);
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
}
