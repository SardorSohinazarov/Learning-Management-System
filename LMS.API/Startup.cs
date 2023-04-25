using LMS.API.Context;
using LMS.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using TelegramSink;

namespace LMS.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.DefaultPolicyName = "AllOrigin";
                options.AddPolicy("AllOrigin",corsPolicyBuilder =>
                {
                    corsPolicyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            AddLoggingOptions(this.Configuration);
            AddDbContextOptions(services);
            AddIdentityOptions(services);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LMS.API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LMS.API v1"));
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddDbContextOptions(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options
                            => options.UseLazyLoadingProxies()
                                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        private static void AddIdentityOptions(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        private static void AddLoggingOptions(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
                .WriteTo.File(path: "LogFiles/log-.txt", rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Message:lj}{NewLine}{Exception}")
                .WriteTo.TeleSink(telegramApiKey: configuration.GetValue<string>("TelegramValues:telegramToken"),
                    telegramChatId: configuration.GetValue<string>("TelegramValues:telegramChatId"))
            .CreateLogger();

            Log.Logger.Information("Application Starting");
        }
    }
}
