using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetProject.DataAccess;
using PetProject.Domain;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
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

            services
                .AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pet API",
                    Version = "v1"
                });
                c.IncludeXmlComments(XmlCommentsFilePath);
            });
            var audienceConfig = Configuration.GetSection("Audience");
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig["Secret"]));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, cfg =>
                {
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidateIssuer = true,
                        ValidIssuer = audienceConfig["Iss"],
                        ValidateAudience = true,
                        ValidAudience = audienceConfig["Aud"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        RequireExpirationTime = true,
                    };
                });
            services.AddCors();
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

            app.UseCors(c => c.SetIsOriginAllowed(x => _ = true)
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pet API V1");
                // c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseAuthentication();

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

            var petStatusAdoptionReady = new PetStatus { Status = "Готов к новым хозяевам" };
            var petStatusNotReady = new PetStatus { Status = "Не готов к новым хозяевам" };
            var petStatusLost = new PetStatus { Status = "Потерянное животное" };
            var petStatusAdopted = new PetStatus { Status = "У нового хозяина" };

            AddIfNotExists(context.PetStatuses, petStatusAdoptionReady, petStatus => petStatus.Status == "Готов к новым хозяевам");
            AddIfNotExists(context.PetStatuses, petStatusNotReady, petStatus => petStatus.Status == "Не готов к новым хозяевам");
            AddIfNotExists(context.PetStatuses, petStatusLost, petStatus => petStatus.Status == "Потерянное животное");
            AddIfNotExists(context.PetStatuses, petStatusAdopted, petStatus => petStatus.Status == "У нового хозяина");

            var pet1 = new Pet
            {
                Name = "Барсик",
                Description = "Пирожок.",
                PetStatus = petStatusAdopted
            };

            var pet2 = new Pet
            {
                Name = "Бэтмен",
                Description = "Бедный бездомный кот.",
                PetStatus = petStatusAdoptionReady
            };

            var pet3 = new Pet
            {
                Name = "Догго",
                Description = "Потеряшка.",
                PetStatus = petStatusLost
            };

            var pet4 = new Pet
            {
                Name = "Лопес",
                Description = "Нет усов, в остальном - здоровый кот.",
                PetStatus = petStatusAdopted
            };

            var pet5 = new Pet
            {
                Name = "Тина",
                Description = "Хорошая девочка, чудесная шерстка.",
                PetStatus = petStatusAdoptionReady
            };

            var pet6 = new Pet
            {
                Name = "Тузик",
                Description = "Цепной пес.",
                PetStatus = petStatusNotReady
            };

            AddIfNotExists(context.Pets, pet1, pet => pet.Name != "Барсик");
            AddIfNotExists(context.Pets, pet2, pet => pet.Name != "Бэтмен");
            AddIfNotExists(context.Pets, pet3, pet => pet.Name != "Догго");
            AddIfNotExists(context.Pets, pet4, pet => pet.Name != "Лопес");
            AddIfNotExists(context.Pets, pet5, pet => pet.Name != "Тина");
            AddIfNotExists(context.Pets, pet6, pet => pet.Name != "Тузик");

            var image1 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Барсик"),
                ImagePath = "images/barsik.jpg"
            };
            var image2 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Бэтмен"),
                ImagePath = "images/batman.png"
            };
            var image3 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Догго"),
                ImagePath = "images/doggo.png"
            };
            var image4 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Лопес"),
                ImagePath = "images/lopes.png"
            };
            var image5 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тина"),
                ImagePath = "images/tina.png"
            };
            var image6 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тузик"),
                ImagePath = "images/tuzik.png"
            };

            AddIfNotExists(context.Images, image1, image => image.ImagePath == "images/barsik.jpg");
            AddIfNotExists(context.Images, image2, image => image.ImagePath == "images/batman.png");
            AddIfNotExists(context.Images, image3, image => image.ImagePath == "images/doggo.png");
            AddIfNotExists(context.Images, image4, image => image.ImagePath == "images/lopes.png");
            AddIfNotExists(context.Images, image5, image => image.ImagePath == "images/tina.png");
            AddIfNotExists(context.Images, image6, image => image.ImagePath == "images/tuzik.png");

            var petFeature1 = new PetFeature { Category = "Description", Characteristic = "Пёс" };
            var petFeature2 = new PetFeature { Category = "Description", Characteristic = "Кот" };
            var petFeature3 = new PetFeature { Category = "Description", Characteristic = "Девочка" };
            var petFeature4 = new PetFeature { Category = "Description", Characteristic = "Мальчик" };
            var petFeature5 = new PetFeature { Category = "Age", Characteristic = "До года" };
            var petFeature6 = new PetFeature { Category = "Age", Characteristic = "До 3 лет" };
            var petFeature7 = new PetFeature { Category = "Age", Characteristic = "До 10 лет" };
            var petFeature8 = new PetFeature { Category = "Fur", Characteristic = "Длинная" };
            var petFeature9 = new PetFeature { Category = "Fur", Characteristic = "Короткая" };
            var petFeature10 = new PetFeature { Category = "Fur", Characteristic = "Светлая" };
            var petFeature11 = new PetFeature { Category = "Fur", Characteristic = "Темная" };
            var petFeature12 = new PetFeature { Category = "Fur", Characteristic = "Цветная" };

            AddIfNotExists(context.PetFeatures, petFeature1, petFeature => petFeature.Characteristic == "Пёс");
            AddIfNotExists(context.PetFeatures, petFeature2, petFeature => petFeature.Characteristic == "Кот");
            AddIfNotExists(context.PetFeatures, petFeature3, petFeature => petFeature.Characteristic == "Девочка");
            AddIfNotExists(context.PetFeatures, petFeature4, petFeature => petFeature.Characteristic == "Мальчик");
            AddIfNotExists(context.PetFeatures, petFeature5, petFeature => petFeature.Characteristic == "До года");
            AddIfNotExists(context.PetFeatures, petFeature6, petFeature => petFeature.Characteristic == "До 3 лет");
            AddIfNotExists(context.PetFeatures, petFeature7, petFeature => petFeature.Characteristic == "До 10 лет");
            AddIfNotExists(context.PetFeatures, petFeature8, petFeature => petFeature.Characteristic == "Длинная");
            AddIfNotExists(context.PetFeatures, petFeature9, petFeature => petFeature.Characteristic == "Короткая");
            AddIfNotExists(context.PetFeatures, petFeature10, petFeature => petFeature.Characteristic == "Светлая");
            AddIfNotExists(context.PetFeatures, petFeature11, petFeature => petFeature.Characteristic == "Темная");
            AddIfNotExists(context.PetFeatures, petFeature12, petFeature => petFeature.Characteristic == "Цветная");

            var petFeatureAssignment1 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Барсик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Кот")
            };
            var petFeatureAssignment2 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Барсик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Мальчик")
            };
            var petFeatureAssignment3 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Барсик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До 3 лет")
            };
            var petFeatureAssignment4 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Барсик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment5 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Барсик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Темная")
            };


            var petFeatureAssignment6 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Бэтмен"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Кот")
            };
            var petFeatureAssignment7 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Бэтмен"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Мальчик")
            };
            var petFeatureAssignment8 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Бэтмен"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До 3 лет")
            };
            var petFeatureAssignment9 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Бэтмен"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment10 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Бэтмен"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Цветная")
            };


            var petFeatureAssignment11 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Догго"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Пёс")
            };
            var petFeatureAssignment12 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Догго"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Мальчик")
            };
            var petFeatureAssignment13 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Догго"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До 10 лет")
            };
            var petFeatureAssignment14 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Догго"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment15 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Догго"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Темная")
            };


            var petFeatureAssignment16 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Лопес"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Кот")
            };
            var petFeatureAssignment17 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Лопес"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Мальчик")
            };
            var petFeatureAssignment18 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Лопес"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До года")
            };
            var petFeatureAssignment19 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Лопес"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment20 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Лопес"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Светлая")
            };


            var petFeatureAssignment21 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тина"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Пёс")
            };
            var petFeatureAssignment22 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тина"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Девочка")
            };
            var petFeatureAssignment23 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тина"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До 10 лет")
            };
            var petFeatureAssignment24 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тина"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment25 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тина"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Цветная")
            };


            var petFeatureAssignment26 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тузик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Пёс")
            };
            var petFeatureAssignment27 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тузик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Мальчик")
            };
            var petFeatureAssignment28 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тузик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До 3 лет")
            };
            var petFeatureAssignment29 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тузик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment30 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тузик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Темная")
            };

            context.PetFeatureAssignments.RemoveRange(context.PetFeatureAssignments);

            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment1);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment2);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment3);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment4);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment5);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment6);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment7);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment8);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment9);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment10);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment11);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment12);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment13);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment14);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment15);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment16);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment17);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment18);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment19);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment20);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment21);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment22);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment23);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment24);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment25);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment26);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment27);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment28);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment29);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment30);

            await context.SaveChangesAsync();
        }

        private T FetchEntity<T>(DbSet<T> dbSet, Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return dbSet.Local.FirstOrDefault(predicate.Compile())
                ?? dbSet.FirstOrDefault(predicate);
        }

        private static void AddIfNotExists<T>(
            DbSet<T> dbSet,
            T entity,
            Expression<Func<T, bool>> predicate = null)
            where T : class, new()
        {
            var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
            if (!exists)
            {
                dbSet.Add(entity);
            }
        }
    }
}
