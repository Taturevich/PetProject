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

            var pet7 = new Pet
            {
                Name = "Шарик",
                Description = "Мне нужна помощь.",
                PetStatus = petStatusAdoptionReady
            };

            var pet8 = new Pet
            {
                Name = "Котя",
                Description = "Бушарик.",
                PetStatus = petStatusAdoptionReady
            };

            var pet9 = new Pet
            {
                Name = "Леся",
                Description = "Старая мудрая кошка.",
                PetStatus = petStatusAdopted
            };

            var pet10 = new Pet
            {
                Name = "Майя",
                Description = "Пёсель или собакен?",
                PetStatus = petStatusAdopted
            };

            var pet11 = new Pet
            {
                Name = "Шуберт",
                Description = "Гений.",
                PetStatus = petStatusAdoptionReady
            };

            var pet12 = new Pet
            {
                Name = "Одя",
                Description = "Глуповатый.",
                PetStatus = petStatusLost
            };

            var pet13 = new Pet
            {
                Name = "Мисти",
                Description = "Дымная кошечка.",
                PetStatus = petStatusAdoptionReady
            };

            AddIfNotExists(context.Pets, pet1, pet => pet.Name == "Барсик");
            AddIfNotExists(context.Pets, pet2, pet => pet.Name == "Бэтмен");
            AddIfNotExists(context.Pets, pet3, pet => pet.Name == "Догго");
            AddIfNotExists(context.Pets, pet4, pet => pet.Name == "Лопес");
            AddIfNotExists(context.Pets, pet5, pet => pet.Name == "Тина");
            AddIfNotExists(context.Pets, pet6, pet => pet.Name == "Тузик");
            AddIfNotExists(context.Pets, pet7, pet => pet.Name == "Шарик");
            AddIfNotExists(context.Pets, pet8, pet => pet.Name == "Котя");
            AddIfNotExists(context.Pets, pet9, pet => pet.Name == "Леся");
            AddIfNotExists(context.Pets, pet10, pet => pet.Name == "Майя");
            AddIfNotExists(context.Pets, pet11, pet => pet.Name == "Шуберт");
            AddIfNotExists(context.Pets, pet12, pet => pet.Name == "Одя");
            AddIfNotExists(context.Pets, pet13, pet => pet.Name == "Мисти");

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
            var image7 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Шарик"),
                ImagePath = "images/sharik.png"
            };
            var image8 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Котя"),
                ImagePath = "images/kotja.png"
            };
            var image9 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Леся"),
                ImagePath = "images/lesja.png"
            };
            var image10 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Майя"),
                ImagePath = "images/maya.png"
            };
            var image11 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Шуберт"),
                ImagePath = "images/shubert.png"
            };
            var image12 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Одя"),
                ImagePath = "images/odja.png"
            };
            var image13 = new Image
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Мисти"),
                ImagePath = "images/misty.png"
            };

            AddIfNotExists(context.Images, image1, image => image.ImagePath == "images/barsik.jpg");
            AddIfNotExists(context.Images, image2, image => image.ImagePath == "images/batman.png");
            AddIfNotExists(context.Images, image3, image => image.ImagePath == "images/doggo.png");
            AddIfNotExists(context.Images, image4, image => image.ImagePath == "images/lopes.png");
            AddIfNotExists(context.Images, image5, image => image.ImagePath == "images/tina.png");
            AddIfNotExists(context.Images, image6, image => image.ImagePath == "images/tuzik.png");
            AddIfNotExists(context.Images, image7, image => image.ImagePath == "images/sharik.png");
            AddIfNotExists(context.Images, image8, image => image.ImagePath == "images/kotja.png");
            AddIfNotExists(context.Images, image9, image => image.ImagePath == "images/lesja.png");
            AddIfNotExists(context.Images, image10, image => image.ImagePath == "images/maya.png");
            AddIfNotExists(context.Images, image11, image => image.ImagePath == "images/shubert.png");
            AddIfNotExists(context.Images, image12, image => image.ImagePath == "images/odja.png");
            AddIfNotExists(context.Images, image13, image => image.ImagePath == "images/misty.png");

            var petFeature1 = new PetFeature { Category = "Кто?", Characteristic = "Пёс" };
            var petFeature2 = new PetFeature { Category = "Кто?", Characteristic = "Кот" };
            var petFeature3 = new PetFeature { Category = "Кто?", Characteristic = "Девочка" };
            var petFeature4 = new PetFeature { Category = "Кто?", Characteristic = "Мальчик" };
            var petFeature5 = new PetFeature { Category = "Возраст", Characteristic = "До года" };
            var petFeature6 = new PetFeature { Category = "Возраст", Characteristic = "До 3 лет" };
            var petFeature7 = new PetFeature { Category = "Возраст", Characteristic = "До 10 лет" };
            var petFeature8 = new PetFeature { Category = "Шуба", Characteristic = "Длинная" };
            var petFeature9 = new PetFeature { Category = "Шуба", Characteristic = "Короткая" };
            var petFeature10 = new PetFeature { Category = "Шуба", Characteristic = "Светлая" };
            var petFeature11 = new PetFeature { Category = "Шуба", Characteristic = "Темная" };
            var petFeature12 = new PetFeature { Category = "Шуба", Characteristic = "Цветная" };

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


            var petFeatureAssignment31 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Шарик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Пёс")
            };
            var petFeatureAssignment32 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Шарик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Мальчик")
            };
            var petFeatureAssignment33 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Шарик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До 3 лет")
            };
            var petFeatureAssignment34 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Шарик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment35 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Шарик"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Светлая")
            };


            var petFeatureAssignment36 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Котя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Кот")
            };
            var petFeatureAssignment37 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Котя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Мальчик")
            };
            var petFeatureAssignment38 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Котя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До 10 лет")
            };
            var petFeatureAssignment39 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Котя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment40 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Котя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Цветная")
            };


            var petFeatureAssignment41 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Леся"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Кот")
            };
            var petFeatureAssignment42 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Леся"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Девочка")
            };
            var petFeatureAssignment43 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Леся"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До 10 лет")
            };
            var petFeatureAssignment44 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Леся"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment45 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Леся"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Темная")
            };


            var petFeatureAssignment46 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Майя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Пёс")
            };
            var petFeatureAssignment47 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Майя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Девочка")
            };
            var petFeatureAssignment48 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Майя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До 10 лет")
            };
            var petFeatureAssignment49 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Майя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment50 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Майя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Цветная")
            };


            var petFeatureAssignment51 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Шуберт"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Кот")
            };
            var petFeatureAssignment52 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Шуберт"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Мальчик")
            };
            var petFeatureAssignment53 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Шуберт"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До 3 лет")
            };
            var petFeatureAssignment54 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Шуберт"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment55 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Шуберт"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Светлая")
            };


            var petFeatureAssignment56 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Одя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Кот")
            };
            var petFeatureAssignment57 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Одя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Мальчик")
            };
            var petFeatureAssignment58 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Одя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До 3 лет")
            };
            var petFeatureAssignment59 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Одя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment60 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Одя"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Цветная")
            };


            var petFeatureAssignment61 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Мисти"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Кот")
            };
            var petFeatureAssignment62 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Мисти"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Девочка")
            };
            var petFeatureAssignment63 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Мисти"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "До 3 лет")
            };
            var petFeatureAssignment64 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Мисти"),
                PetFeature = FetchEntity(context.PetFeatures, feature => feature.Characteristic == "Короткая")
            };
            var petFeatureAssignment65 = new PetFeatureAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Мисти"),
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
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment31);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment32);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment33);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment34);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment35);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment36);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment37);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment38);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment39);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment40);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment41);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment42);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment43);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment44);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment45);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment46);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment47);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment48);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment49);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment50);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment51);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment52);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment53);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment54);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment55);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment56);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment57);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment58);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment59);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment60);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment61);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment62);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment63);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment64);
            AddIfNotExists(context.PetFeatureAssignments, petFeatureAssignment65);

            var userVolunteer = new User
            {
                Name = "Светлана",
                LastName = "Лаппо",
                IsBlackListed = false,
                Password = "111",
                PetPoints = 50,
                Phone = "+375 93 279-47-21",
                Role = UserRole.Volunteer
            };
            
            var userUser = new User
            {
                Name = "Мария",
                LastName = "Коцупалова",
                IsBlackListed = false,
                Password = "12345678",
                PetPoints = 50,
                Phone = "+375 11 668-45-89",
                Role = UserRole.User
            };

            AddIfNotExists(context.Users, userVolunteer, user => user.Name == "Светлана");
            AddIfNotExists(context.Users, userUser, user => user.Name == "Мария");

            var userFeature1 = new UserFeature
            {
                Category = "Accommodation",
                Characteristic = "Частный дом"
            };
            var userFeature2 = new UserFeature
            {
                Category = "Accommodation",
                Characteristic = "Квартира"
            };
            var userFeature3 = new UserFeature
            {
                Category = "Accommodation",
                Characteristic = "Общежитие"
            };
            var userFeature4 = new UserFeature
            {
                Category = "HouseOwnership",
                Characteristic = "Собственность"
            };
            var userFeature5 = new UserFeature
            {
                Category = "HouseOwnership",
                Characteristic = "Аренда"
            };
            var userFeature6 = new UserFeature
            {
                Category = "Exprience",
                Characteristic = "Никогда не было питомца"
            };
            var userFeature7 = new UserFeature
            {
                Category = "Exprience",
                Characteristic = "Был питомец"
            };
            var userFeature8 = new UserFeature
            {
                Category = "Exprience",
                Characteristic = "Питомец есть сейчас"
            };
            var userFeature9 = new UserFeature
            {
                Category = "Readiness",
                Characteristic = "Есть сетки на окнах"
            };
            var userFeature10 = new UserFeature
            {
                Category = "Readiness",
                Characteristic = "Есть миска"
            };
            var userFeature11 = new UserFeature
            {
                Category = "Readiness",
                Characteristic = "Есть лоток"
            };
            var userFeature12 = new UserFeature
            {
                Category = "Readiness",
                Characteristic = "Есть поводок / ошейник"
            };

            AddIfNotExists(context.UserFeatures, userFeature1, userFeature => userFeature.Characteristic == "Частный дом");
            AddIfNotExists(context.UserFeatures, userFeature2, userFeature => userFeature.Characteristic == "Квартира");
            AddIfNotExists(context.UserFeatures, userFeature3, userFeature => userFeature.Characteristic == "Общежитие");
            AddIfNotExists(context.UserFeatures, userFeature4, userFeature => userFeature.Characteristic == "Собственность");
            AddIfNotExists(context.UserFeatures, userFeature5, userFeature => userFeature.Characteristic == "Аренда");
            AddIfNotExists(context.UserFeatures, userFeature6, userFeature => userFeature.Characteristic == "Никогда не было питомца");
            AddIfNotExists(context.UserFeatures, userFeature7, userFeature => userFeature.Characteristic == "Был питомец");
            AddIfNotExists(context.UserFeatures, userFeature8, userFeature => userFeature.Characteristic == "Питомец есть сейчас");
            AddIfNotExists(context.UserFeatures, userFeature9, userFeature => userFeature.Characteristic == "Есть сетки на окнах");
            AddIfNotExists(context.UserFeatures, userFeature10, userFeature => userFeature.Characteristic == "Есть миска");
            AddIfNotExists(context.UserFeatures, userFeature11, userFeature => userFeature.Characteristic == "Есть лоток");
            AddIfNotExists(context.UserFeatures, userFeature12, userFeature => userFeature.Characteristic == "Есть поводок / ошейник");

            var userFeatureAssignment1 = new UserFeatureAssignment
            {
                User = FetchEntity(context.Users, user => user.Name == "Мария"),
                UserFeature = FetchEntity(context.UserFeatures, userFeature => userFeature.Characteristic == "Квартира")
            };
            var userFeatureAssignment2 = new UserFeatureAssignment
            {
                User = FetchEntity(context.Users, user => user.Name == "Мария"),
                UserFeature = FetchEntity(context.UserFeatures, userFeature => userFeature.Characteristic == "Аренда")
            };
            var userFeatureAssignment3 = new UserFeatureAssignment
            {
                User = FetchEntity(context.Users, user => user.Name == "Мария"),
                UserFeature = FetchEntity(context.UserFeatures, userFeature => userFeature.Characteristic == "Питомец есть сейчас")
            };
            var userFeatureAssignment4 = new UserFeatureAssignment
            {
                User = FetchEntity(context.Users, user => user.Name == "Мария"),
                UserFeature = FetchEntity(context.UserFeatures, userFeature => userFeature.Characteristic == "Есть сетки на окнах")
            };
            var userFeatureAssignment5 = new UserFeatureAssignment
            {
                User = FetchEntity(context.Users, user => user.Name == "Мария"),
                UserFeature = FetchEntity(context.UserFeatures, userFeature => userFeature.Characteristic == "Есть миска")
            };
            var userFeatureAssignment6 = new UserFeatureAssignment
            {
                User = FetchEntity(context.Users, user => user.Name == "Мария"),
                UserFeature = FetchEntity(context.UserFeatures, userFeature => userFeature.Characteristic == "Есть лоток")
            };
            var userFeatureAssignment7 = new UserFeatureAssignment
            {
                User = FetchEntity(context.Users, user => user.Name == "Мария"),
                UserFeature = FetchEntity(context.UserFeatures, userFeature => userFeature.Characteristic == "Есть поводок / ошейник")
            };

            context.UserFeatureAssignments.RemoveRange(context.UserFeatureAssignments);

            AddIfNotExists(context.UserFeatureAssignments, userFeatureAssignment1);
            AddIfNotExists(context.UserFeatureAssignments, userFeatureAssignment2);
            AddIfNotExists(context.UserFeatureAssignments, userFeatureAssignment3);
            AddIfNotExists(context.UserFeatureAssignments, userFeatureAssignment4);
            AddIfNotExists(context.UserFeatureAssignments, userFeatureAssignment5);
            AddIfNotExists(context.UserFeatureAssignments, userFeatureAssignment6);
            AddIfNotExists(context.UserFeatureAssignments, userFeatureAssignment7);

            var taskType1 = new TaskType
            {
                Name = "Стать PR агентом питомца",
                Description = "Испытайте свои таланты и отзывчивость ваших знакомых - возьмитесь за задачу приютить отыскать питомцу дом.",
                PetPoints = 100,
                DefaultDurationDays = 30
            };
            var taskType2 = new TaskType
            {
                Name = "Прокормить питомца",
                Description = "Волонтёры содержат сотни животных, в этом задании всё просто - нужно купить животному корм на срок от недели.",
                PetPoints = 50,
                DefaultDurationDays = 7
            };
            var taskType3 = new TaskType
            {
                Name = "Свозить питомца в вет. клинику",
                Description = "Бездомным животным часто нужна мед. помощь - от стерилизации до травм от жестокого обращения.",
                PetPoints = 100,
                DefaultDurationDays = 14
            };

            AddIfNotExists(context.TaskTypes, taskType1, taskType => taskType.Name == "Стать PR агентом питомца");
            AddIfNotExists(context.TaskTypes, taskType2, taskType => taskType.Name == "Прокормить питомца");
            AddIfNotExists(context.TaskTypes, taskType3, taskType => taskType.Name == "Свозить питомца в вет. клинику");

            var petTaskTypeAssignment1 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Барсик"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Стать PR агентом питомца")
            };
            var petTaskTypeAssignment2 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Барсик"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Прокормить питомца")
            };
            var petTaskTypeAssignment3 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Бэтмен"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Стать PR агентом питомца")
            };
            var petTaskTypeAssignment4 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Бэтмен"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Свозить питомца в вет. клинику")
            };
            var petTaskTypeAssignment5 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Догго"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Стать PR агентом питомца")
            };
            var petTaskTypeAssignment6 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Догго"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Прокормить питомца")
            };
            var petTaskTypeAssignment7 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Догго"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Свозить питомца в вет. клинику")
            };
            var petTaskTypeAssignment8 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Лопес"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Стать PR агентом питомца")
            };
            var petTaskTypeAssignment9 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Лопес"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Свозить питомца в вет. клинику")
            };
            var petTaskTypeAssignment10 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тина"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Стать PR агентом питомца")
            };
            var petTaskTypeAssignment11 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тина"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Свозить питомца в вет. клинику")
            };
            var petTaskTypeAssignment12 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тузик"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Стать PR агентом питомца")
            };
            var petTaskTypeAssignment13 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тузик"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Прокормить питомца")
            };
            var petTaskTypeAssignment14 = new PetTaskTypeAssignment
            {
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тузик"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Свозить питомца в вет. клинику")
            };

            context.PetTaskTypeAssignments.RemoveRange(context.PetTaskTypeAssignments);

            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment1);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment2);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment3);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment4);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment5);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment6);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment7);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment8);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment9);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment10);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment11);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment12);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment13);
            AddIfNotExists(context.PetTaskTypeAssignments, petTaskTypeAssignment14);

            var task1 = new Domain.Task
            {
                User = FetchEntity(context.Users, user => user.Name == "Мария"),
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Барсик"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Стать PR агентом питомца"),
                Status = TaskStatus.InProgress,
                StartDate = DateTime.UtcNow.AddDays(-5),
                EndDate = null
            };
            var task2 = new Domain.Task
            {
                User = FetchEntity(context.Users, user => user.Name == "Мария"),
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Барсик"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Прокормить питомца"),
                Status = TaskStatus.Completed,
                StartDate = DateTime.UtcNow.AddDays(-10),
                EndDate = DateTime.UtcNow.AddDays(-9)
            };
            var task3 = new Domain.Task
            {
                User = FetchEntity(context.Users, user => user.Name == "Мария"),
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Догго"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Стать PR агентом питомца"),
                Status = TaskStatus.InProgress,
                StartDate = DateTime.UtcNow.AddDays(-15),
                EndDate = null
            };
            var task4 = new Domain.Task
            {
                User = FetchEntity(context.Users, user => user.Name == "Мария"),
                Pet = FetchEntity(context.Pets, pet => pet.Name == "Тина"),
                TaskType = FetchEntity(context.TaskTypes, taskType => taskType.Name == "Стать PR агентом питомца"),
                Status = TaskStatus.Completed,
                StartDate = DateTime.UtcNow.AddDays(-20),
                EndDate = DateTime.UtcNow.AddDays(-5)
            };

            AddIfNotExists(context.Tasks, task1, 
                task => 
                    task.User.Name == "Мария" 
                    && task.Pet.Name == "Барсик" 
                    && task.TaskType.Name == "Стать PR агентом питомца");
            AddIfNotExists(context.Tasks, task2, 
                task => 
                    task.User.Name == "Мария" 
                    && task.Pet.Name == "Барсик" 
                    && task.TaskType.Name == "Прокормить питомца");
            AddIfNotExists(context.Tasks, task3, 
                task => 
                    task.User.Name == "Мария" 
                    && task.Pet.Name == "Догго"
                    && task.TaskType.Name == "Стать PR агентом питомца");
            AddIfNotExists(context.Tasks, task4, 
                task => 
                    task.User.Name == "Мария" 
                    && task.Pet.Name == "Тина"
                    && task.TaskType.Name == "Стать PR агентом питомца");


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
            var exists = predicate != null && dbSet.Any(predicate);
            if (!exists)
            {
                dbSet.Add(entity);
            }
        }
    }
}
