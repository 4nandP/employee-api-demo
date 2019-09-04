using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Employee.Api
{
    /// <summary>
    /// Applies leverages api versioning for swagger doc generation
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
          this.provider = provider;

        /// <summary>
        /// Invoked to configure a TOptions instance.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                  description.GroupName,
                    new Info()
                    {
                        Title = "Employee API",
                        Version = description.ApiVersion.ToString(),
                    });
            }
        }
    }
}
