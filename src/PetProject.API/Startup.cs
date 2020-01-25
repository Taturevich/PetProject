using System;
using System.Collections.Generic;
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
            await using var context = serviceCollection.BuildServiceProvider().GetService<PetContext>();

            context.Database.EnsureCreated();

            var petStatusAdoptionReady = new PetStatus {Status = "AdoptionReady"};
            var petStatusNotReady = new PetStatus { Status = "NotReady" };
            var petStatusLost = new PetStatus { Status = "Lost" };
            var petStatusAdopted = new PetStatus { Status = "Adopted" };

            AddIfNotExists(context.PetStatuses, petStatusAdoptionReady, petStatus => petStatus.Status == "AdoptionReady");
            AddIfNotExists(context.PetStatuses, petStatusNotReady, petStatus => petStatus.Status == "NotReady");
            AddIfNotExists(context.PetStatuses, petStatusLost, petStatus => petStatus.Status == "Lost");
            AddIfNotExists(context.PetStatuses, petStatusAdopted, petStatus => petStatus.Status == "Adopted");
            
            var pet1 = new Pet
            {
                Name = "Barsik",
                Description = "Sweet meow.",
                PetStatus = petStatusAdopted
            };

            var pet2 = new Pet
            {
                Name = "Murka",
                Description = "Poor homeless cat.",
                PetStatus = petStatusAdoptionReady
            };

            var pet3 = new Pet
            {
                Name = "Pinky",
                Description = "Lost boi.",
                PetStatus = petStatusLost
            };

            var pet4 = new Pet
            {
                Name = "Bayaderka",
                Description = "Dangerous cat.",
                PetStatus = petStatusAdopted
            };

            var pet5 = new Pet
            {
                Name = "Snana",
                Description = "No moustache, otherwise healthy kitty.",
                PetStatus = petStatusAdoptionReady
            };

            var pet6 = new Pet
            {
                Name = "Sekopina",
                Description = "Street cat.",
                PetStatus = petStatusNotReady
            };
            
            AddIfNotExists(context.Pets, pet1, pet => pet.Name != "Barsik");
            AddIfNotExists(context.Pets, pet2, pet => pet.Name != "Murka");
            AddIfNotExists(context.Pets, pet3, pet => pet.Name != "Pinky");
            AddIfNotExists(context.Pets, pet4, pet => pet.Name != "Bayaderka");
            AddIfNotExists(context.Pets, pet5, pet => pet.Name != "Snana");
            AddIfNotExists(context.Pets, pet6, pet => pet.Name != "Sekopina");

            var image1 = new Image
            {
                Pet = context.Pets.Local.FirstOrDefault(pet => pet.Name == "Barsik")
                    ?? context.Pets.FirstOrDefault(pet => pet.Name == "Barsik"),
                ImagePath = "images/barsik.jpg"
            };
            var image2 = new Image
            {
                Pet = context.Pets.Local.FirstOrDefault(pet => pet.Name == "Murka")
                      ?? context.Pets.FirstOrDefault(pet => pet.Name == "Murka"),
                ImagePath = "images/batman.jpg"
            };
            var image3 = new Image
            {
                Pet = context.Pets.Local.FirstOrDefault(pet => pet.Name == "Pinky")
                      ?? context.Pets.FirstOrDefault(pet => pet.Name == "Pinky"),
                ImagePath = "images/doggo.jpg"
            };
            var image4 = new Image
            {
                Pet = context.Pets.Local.FirstOrDefault(pet => pet.Name == "Bayaderka")
                      ?? context.Pets.FirstOrDefault(pet => pet.Name == "Bayaderka"),
                ImagePath = "images/lopes.jpg"
            };
            var image5 = new Image
            {
                Pet = context.Pets.Local.FirstOrDefault(pet => pet.Name == "Snana")
                      ?? context.Pets.FirstOrDefault(pet => pet.Name == "Snana"),
                ImagePath = "images/tina.jpg"
            };
            var image6 = new Image
            {
                Pet = context.Pets.Local.FirstOrDefault(pet => pet.Name == "Sekopina")
                      ?? context.Pets.FirstOrDefault(pet => pet.Name == "Sekopina"),
                ImagePath = "images/tuzik.jpg"
            };

            AddIfNotExists(context.Images, image1, image => image.ImagePath == "images/barsik.jpg");
            AddIfNotExists(context.Images, image2, image => image.ImagePath == "images/batman.jpg");
            AddIfNotExists(context.Images, image3, image => image.ImagePath == "images/doggo.jpg");
            AddIfNotExists(context.Images, image4, image => image.ImagePath == "images/lopes.jpg");
            AddIfNotExists(context.Images, image5, image => image.ImagePath == "images/tina.jpg");
            AddIfNotExists(context.Images, image6, image => image.ImagePath == "images/tuzik.jpg");

            await context.SaveChangesAsync();
        }

        private static void AddIfNotExists<T>(DbSet<T> dbSet, IEnumerable<T> entities) where T : class, new()
        {
            foreach (var entity in entities)
            {
                AddIfNotExists(dbSet, entity);
            }
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
