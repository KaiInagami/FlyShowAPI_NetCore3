using FlyshowVegetablesAPI.Interfaces;
using FlyshowVegetablesAPI.MiddleWare;
using FlyshowVegetablesAPI.Models;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;
using NLog.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlyshowVegetablesAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("TokenManagement");
            services.Configure<TokenManagement>(appSettingsSection);

            //Add JWT
            var appSettings = appSettingsSection.Get<TokenManagement>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // requires using Microsoft.Extensions.Options
            services.Configure<MonoConnectionSettings>(Configuration.GetSection("MongoConnection"));

            services.AddSingleton<IMonoConnectionSettings>(sp =>
                sp.GetRequiredService<IOptions<MonoConnectionSettings>>().Value);

            services.AddSingleton<TokenRepository>();

            // Add DbContext 
            services.AddDbContext<AccessDBContext>(cfg =>
            {
                cfg.UseSqlServer(
                    Configuration
                    .GetConnectionString("DefaultConnection"));
            });

            // Add Repository
            services.AddScoped<IAdvertiseRepository, AdvertiseRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISpecialContractStoreRepository, SpecialContractStoreRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Add Service
            services.AddScoped<IAdvertiseService, Services.AdvertiseService>();
            services.AddScoped<IProductService, Services.ProductService>();
            services.AddScoped<IUserService, Services.UserService>();
            services.AddScoped<ISpecialContractStoreService, Services.SpecialContractStoreService>();


            // Add Filter
            //services.AddMvc(config =>
            //{
            //   //// config.Filters.Add(new AuthorizationFilter());
            //   // config.Filters.Add(new ActionFilter(_logger));
            //   // config.Filters.Add(new ExceptionFilter());
            //});

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fly show Vegetables API", Version = "v1" });
                c.OperationFilter<HeaderTokenOperationFilter>();
            });

            services.AddMvc(options =>
             {
                 options.FormatterMappings.SetMediaTypeMappingForFormat
                     ("xml", MediaTypeHeaderValue.Parse("application/xml"));
                 options.FormatterMappings.SetMediaTypeMappingForFormat
                     ("config", MediaTypeHeaderValue.Parse("application/xml"));
                 options.FormatterMappings.SetMediaTypeMappingForFormat
                     ("js", MediaTypeHeaderValue.Parse("application/json"));
             })
           .AddXmlSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(x => x
                     .AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fly show Vegetables API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
