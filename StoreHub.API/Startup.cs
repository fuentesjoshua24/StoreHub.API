using StoreHub.API.Repositories;
using StoreHub.API.Services;

namespace StoreHub.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region Dependency Injection
            // Register application services and repositories
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IStoreService, StoreService>();
            #endregion

            #region Swagger Configuration
            // Swagger (Swashbuckle)
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            #endregion

            #region CORS 
            services.AddCors(options => 
            { 
                options.AddPolicy("AllowAngular", 
                    policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200") // Angular dev server
                                    .AllowAnyHeader() 
                                    .AllowAnyMethod()); 
            }); 
            #endregion
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store API V1");
                });
            }

            //app.UseHttpsRedirection();

            app.UseCors("AllowAngular"); // <-- Add this before Authorization

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
