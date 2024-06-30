using Microsoft.OpenApi.Models;
using System.Reflection;

namespace MC.ProductService.API.Options
{
    /// <summary>
    /// Provides configuration for Swagger documentation.
    /// </summary>
    public static class ConfigureSwaggerOptions
    {
        /// <summary>
        /// Adds Swagger services to the specified service collection.
        /// </summary>
        /// <param name="services">The service collection to add the Swagger services to.</param>
        public static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Product {groupName}",
                    Version = groupName,
                    Description = "Product API",
                    Contact = new OpenApiContact
                    {
                        Name = "Personal Company",
                        Email = "elmauro@gmail.com",
                    }
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
    }
}
