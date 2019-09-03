using System.Reflection;
using JsonFlatFileDataStore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Employee.Api
{
    /// <summary>
    /// Application Bootstrap
    /// </summary>
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the hosting environment.
        /// </summary>
        /// <value>
        /// The hosting environment.
        /// </value>
        public IHostingEnvironment HostingEnvironment { get; }

        /// <summary>
        /// Gets the XML comments file path.
        /// </summary>
        /// <value>
        /// The XML comments file path.
        /// </value>
        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return System.IO.Path.Combine(basePath, fileName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDataStore(services);
            services.AddSingleton<V1.Application.Queries.IEmployeeQueries, V1.Application.Queries.EmployeeQueries>();
            services.AddSingleton<V2.Application.Queries.IEmployeeQueries, V2.Application.Queries.EmployeeQueries>();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
            .AddControllersAsServices()
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                });
            services.AddVersionedApiExplorer(options => { options.GroupNameFormat = "'v'V"; options.SubstituteApiVersionInUrl = true; });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(XmlCommentsFilePath);
            });
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="provider">The provider.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = $"public,max-age={durationInSeconds}";
                }
            });

            app.UseMvc();
            app.UseSwagger(options => {
                //options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                //{
                //    swaggerDoc.BasePath = $"/v{swaggerDoc.Info.Version}";
                //});
                //options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                //{
                //    foreach(var path in swaggerDoc.Paths)
                //    {
                //        var operations = new[] { path.Value.Delete, path.Value.Get, path.Value.Patch, path.Value.Post, path.Value.Put };
                //        foreach(var op in operations)
                //        {
                //            var apiVersionParam = op?.Parameters?.FirstOrDefault(p => p.Name == "api-version");
                //            if (apiVersionParam != null)
                //            {
                //                op.Parameters.Remove(apiVersionParam);
                //            }
                //        }
                //    }
                //});
            });
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                    options.DocumentTitle = "Employee API";
                    options.InjectStylesheet("/swagger-ui/custom.css");
                    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                });
        }

        /// <summary>
        /// Configures the data store.
        /// </summary>
        /// <param name="services">The services.</param>
        private static void ConfigureDataStore(IServiceCollection services)
        {
            var dataStoreFilePath = System.IO.Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "datastore.json");
            var dataStore = new DataStore(dataStoreFilePath, keyProperty: "id", reloadBeforeGetCollection: true);

            var collection = dataStore.GetCollection<Employee.Infrastructure.Data.Entities.Employee>();

            collection.ReplaceOne("Test123",
                new Infrastructure.Data.Entities.Employee
                {
                    IsOrganization = false,
                    Title = "Mrs.",
                    GivenName = "Jane",
                    MiddleName = "Lane",
                    FamilyName = "Doe",
                    DisplayName = "Jane Lane Doe",
                    PrintOnCheckName = "Jane Lane Doe",
                    IsActive = true,
                    PrimaryPhone = new Infrastructure.Data.Entities.Phone { FreeFormNumber = "505.555.9999" },
                    PrimaryEmailAddress = new Infrastructure.Data.Entities.EmailAddress { Address = "user@example.com" },
                    EmployeeType = "Regular",
                    Status = "Active",
                    Id = "Test123"
                }, upsert: true
                );

            collection.ReplaceOne("Test456",
                new Infrastructure.Data.Entities.Employee
                {
                    IsOrganization = false,
                    Title = "Mr.",
                    GivenName = "Joshua",
                    MiddleName = "Keith",
                    FamilyName = "Allen",
                    DisplayName = "Joshua Keith Allen",
                    PrintOnCheckName = "Joshua K. Allen",
                    IsActive = true,
                    PrimaryPhone = new Infrastructure.Data.Entities.Phone { FreeFormNumber = "03 5365 9595" },
                    PrimaryEmailAddress = new Infrastructure.Data.Entities.EmailAddress { Address = "joshuaallen@armyspy.com" },
                    EmployeeType = "Temporary",
                    Status = "Active",
                    Id = "Test456"
                }, upsert: true
                );

            collection.ReplaceOne("Test789",
                new Infrastructure.Data.Entities.Employee
                {
                    IsOrganization = false,
                    Title = "Dr.",
                    GivenName = "Alana",
                    MiddleName = "Mactier",
                    FamilyName = "McGuinness",
                    DisplayName = "Alana Mactier McGuinness",
                    PrintOnCheckName = "Alana M. McGuinness",
                    IsActive = false,
                    PrimaryPhone = new Infrastructure.Data.Entities.Phone { FreeFormNumber = "08 9067 3927" },
                    PrimaryEmailAddress = new Infrastructure.Data.Entities.EmailAddress { Address = "alanamcguinness@jourrapide.com" },
                    EmployeeType = "Regular",
                    Status = "Active",
                    Id = "Test789"
                }, upsert: true
                );

            dataStore.Reload();

            services.AddSingleton<IDataStore>(dataStore);
        }
    }
}
