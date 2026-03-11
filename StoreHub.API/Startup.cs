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

            #region Swagger Configuration
            // Swagger (Swashbuckle)
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
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

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
