using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using PetProject.DataAccess;
using PetProject.Domain;
using Task = System.Threading.Tasks.Task;

namespace PetProject
{
    public class Startup
    {
        private static readonly Lazy<string> XmlCommentsFilePathLazy = new Lazy<string>(XmlDocumentationPath(typeof(Startup)));
        private static string XmlCommentsFilePath => XmlCommentsFilePathLazy.Value;

        public static string XmlDocumentationPath(Type currentClass)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var fileName = currentClass.GetTypeInfo().Assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PetContext>(options => options
                .UseSqlite("Data Source=Pet.db"));
            SeedTestData(services).GetAwaiter().GetResult();

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pet API", Version = "v1"
                });
                c.IncludeXmlComments(XmlCommentsFilePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pet API V1");
                // c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        private async Task SeedTestData(IServiceCollection serviceCollection)
        {
            var buildServiceProvider = serviceCollection.BuildServiceProvider();
            using (var petScope = buildServiceProvider.CreateScope())
            {
                await using var scope = petScope.ServiceProvider.GetService<PetContext>();
                var barsik = new Pet
                {
                    Name = "Barsik",
                    Description = "Adopted 10 years ago.",
                    PetStatus = new PetStatus
                    {
                        Status = "Adoption Ready"
                    }
                };
                AddIfNotExists(scope.Pets, barsik, pet => pet.Name != "Barsik" && pet.Description != "Adopted 10 years ago.");
                await scope.SaveChangesAsync();
            }

            using var imageScope = buildServiceProvider.CreateScope();
            await using var imagesContext = imageScope.ServiceProvider.GetService<PetContext>();
            var barsikFromDb = imagesContext.Pets.FirstOrDefault(x => x.Name == "Barsik" && x.Description == "Adopted 10 years ago.");
            var barsikImage = new Image
            {
                PetId = barsikFromDb?.PetId ?? 0,
                Pet = barsikFromDb,
                ImagePath = "images/barsik.jpg",
            };
            AddIfNotExists(imagesContext.Images, barsikImage, image => image.ImagePath == "images/barsik.jpg");
            await imagesContext.SaveChangesAsync();
        }

        private static EntityEntry<T> AddIfNotExists<T>(
            DbSet<T> dbSet,
            T entity,
            Expression<Func<T, bool>> predicate = null)
            where T : class, new()
        {
            var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
            return !exists ? dbSet.Add(entity) : null;
        }
    }
}
