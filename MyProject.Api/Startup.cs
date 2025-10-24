using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MyProject.Repository.Data;
using System;

namespace MyProject.Api
{
    public class Startup
    {
        public const string DEFAULT_DATABASE_CONNECTIONSTRING = "oyb.db";
        public const string DEFAULT_DATABASE_PROVIDER = "sqlite";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);
            var appSettings = new AppSettings();
            Configuration.Bind(appSettings);
            var connectionString = Configuration.GetConnectionString("DefaultConnection") ??
                                          DEFAULT_DATABASE_CONNECTIONSTRING;
            // take the database provider from the environment variable or use hard-coded database provider
            var databaseProvider = Configuration.GetConnectionString("Provider") ?? DEFAULT_DATABASE_PROVIDER;

            if (databaseProvider.ToLower().Trim().Equals("sqlite"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(connectionString));
            }
            else if (databaseProvider.ToLower().Trim().Equals("sqlserver"))
            {
                // only works in windows container
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection")));
            }
            else if (databaseProvider.ToLower().Trim().Equals("mysql"))
            {
                // migrate the table
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseMySql(Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(connectionString));
                var context = new ApplicationDbContext(optionsBuilder.Options);
                context.Database.ExecuteSqlRaw(@"
                CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
                `MigrationId` VARCHAR(150) NOT NULL,
                `ProductVersion` VARCHAR(32) NOT NULL,
                PRIMARY KEY (`MigrationId`));
                ");
            }
            else if (databaseProvider.ToLower().Trim().Equals("postgress"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(
                        Configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                Console.WriteLine("Database provider:" + databaseProvider.ToLower().Trim() + " is unsupported. Try postgress, mysql or sqlite.");
                System.Environment.Exit(0);
            }
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            var _appSettings = appSettings.Value;
            dbContext.Database.Migrate(); //could run this in production
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
