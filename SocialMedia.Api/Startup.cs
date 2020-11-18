using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Filters;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;
using System;
using System.IO;
using System.Reflection;

namespace SocialMedia.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //se mapea con automapper para validar los datos
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                //ayuda frenar las referencias circulares se instala Newtonsoft.Json. 
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            }).ConfigureApiBehaviorOptions(options => 
            {
                //options.SuppressModelStateInvalidFilter = true;//quita la validacion del modelo y se puede validar manual
            });

            services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));

            //services.AddControllers();
            //hacemos la conexion a SocialMediaContext para que use conexion que esta en appsettings para obtener los datos
            services.AddDbContext<SocialMediaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SocialMedia"))
            );

            //inyectar dependecias
            //concecta la inerfaz con el repositorio
            //services.AddTransient<IPostRepository, PostMongoRepository>(); //cambiar el repositorio
            services.AddTransient<IPostService, PostService>();
            //services.AddTransient<IPostRepository, PostRepository>(); al crearse uno generico que es tipo CRUD se elimina
            //services.AddTransient<IUserRepository, UserRepository>();
            //se registra el repositorio generico para la implementación generica
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            //añadimos el filtro de manera global

            //documentación, genera un archivo para comentarios
            services.AddSwaggerGen(doc => 
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                doc.IncludeXmlComments(xmlPath);
            });

            services.AddMvc(options =>
            {
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });//registramos las validaciones creadas
            //unica instancia
            services.AddSingleton<IUriService>(provider =>
            {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme,"://",request.Host.ToUriComponent());
                return new UriService(absoluteUri);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();//ayuda generar la docs
            //muestra la estructura del API
            app.UseSwaggerUI(options=>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json","Social Media API");
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
