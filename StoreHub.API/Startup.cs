using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreHub.API.Common.Data;
using StoreHub.API.CommonUtility;
using StoreHub.API.Repositories;
using StoreHub.API.Services;
using System.Text;

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

            // EF Core DbContext with MySQL
            var connectionString = Configuration.GetConnectionString("MySqlDBConnectionString");
            services.AddDbContext<StoreHubDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            #region Dependency Injection
            //services.AddScoped<IStoreRepository, StoreRepository>();
            //services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILoginRepository, LoginRepository>();

            // Add your login/auth services here
            services.AddScoped<ILoginService, LoginService>();
            services.AddSingleton<JwtUtil>();
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
                    policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                                                 //"http://127.0.0.1:4200", "https://127.0.0.1:4200")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });
            #endregion

            #region Add authentication & authorization services 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "BettingHub.API",
                        ValidAudience = "BettingHub.Client",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]))
                    };

                });
            #endregion

            services.AddAuthorization();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
