using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyProject.Repository.Data;
using MyProject.Repository.Data.Models;
using Xunit;

namespace MyProject.Repository.Test.Helper
{
    public class RepositoryTestCache : IDisposable
    {
        public RepositoryTestCache()
        {
            var appSettignsDict = DataLoader.loadJsonDictonary<AppSettings>("");
            dbContexts = new Dictionary<string, TestDbContext>();
            foreach (var appSettings in appSettignsDict)
            {
                try
                {
                    var context = new TestDbContext(appSettings.Value.connectionStrings);
                    dbContexts.Add(appSettings.Key, context);
                }
                catch (System.Exception e)
                {
                    var assertMsg = "Failed for " + appSettings.Value.connectionStrings.Provider + "\n" +
                     e.Message;
                    Console.WriteLine(assertMsg);
                    // dbContexts.Add(appSettings.Key, null);
                }
            }
        }

        public Dictionary<string, TestDbContext> dbContexts { get; }

        public void Dispose()
        {
            // clean-up code
        }

    }
    public class TestDbContext
    {
        public ApplicationDbContext dbContext;
        private const string DEFAULT_DATABASE_CONNECTIONSTRING = "mock.db";
        private const string DEFAULT_DATABASE_PROVIDER = "sqlite";
        private string _exceptionMessage;
        /// <summary>
        /// validate that database connected and migrated properly
        /// </summary>
        public void ValidSetup()
        {
            Assert.True(string.IsNullOrWhiteSpace(_exceptionMessage), _exceptionMessage);
        }

        private class _tmpContext : DbContext
        {
            public _tmpContext(DbContextOptions<_tmpContext> options)
          : base(options)
            {
                // this.Configuration.ValidateOnSaveEnabled = false;
            }
        }



        private void cleanupBlogs(List<Blog> blogs)
        {
            var count = 0;
            foreach (var blog in blogs)
            {
                // can't insert by default w/ identity insert
                if (dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer")
                {
                    blog.Id = 0;
                }
                count++;
            }
        }

        /// <summary>
        /// default mock seeder
        /// </summary>
        private void SeedDb()
        {
            var blogC = 5;
            var blogs = new List<Blog>();
            for (int i = 1; i <= blogC; i++)
            {
                var b = new Blog()
                {
                    Title = "test blog" + i,
                    Content = "some content" + i
                };
                blogs.Add(b);
            }
            // var blogs = DataLoader.loadJsonArray<Blog>("../../MyProject.Repository.Test/Data/Repository/");
            cleanupBlogs(blogs);
            try
            {
                dbContext.Blog.AddRange(blogs);
                dbContext.SaveChanges();

            }
            catch (System.Exception e)
            {
                var assertMsg = "Failed SeedDb for " + dbContext.Database.ProviderName + "\n";
                if (e.InnerException != null)
                    assertMsg += e.InnerException.Message;
                else
                    assertMsg += e.Message;
                Console.WriteLine(assertMsg);
                _exceptionMessage = assertMsg;
            }
        }

        public TestDbContext(ConnectionStrings connectionStrings = null, bool seedData = true)
        {
            try
            {

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                var dbSettings = connectionStrings ?? new ConnectionStrings();

                var connectionString = dbSettings.DefaultConnection ??
                                              DEFAULT_DATABASE_CONNECTIONSTRING;
                // take the database provider from the environment variable or use hard-coded database provider
                var databaseProvider = dbSettings.Provider ?? DEFAULT_DATABASE_PROVIDER;

                if (databaseProvider.ToLower().Trim().Equals("sqlite"))
                {
                    optionsBuilder.UseSqlite("DataSource=" + connectionString);
                }
                else if (databaseProvider.ToLower().Trim().Equals("sqlserver"))
                {
                    // only works in windows container
                    optionsBuilder.UseSqlServer(connectionString);
                }
                else if (databaseProvider.ToLower().Trim().Equals("mysql"))
                {
                    optionsBuilder.UseMySQL(connectionString);
                }
                else if (databaseProvider.ToLower().Trim().Equals("postgress"))
                {
                    optionsBuilder.UseNpgsql(connectionString);
                }
                else
                {
                    throw new System.Exception("Database provider:" + databaseProvider.ToLower().Trim() + " is unsupported. Try postgress, mysql or sqlite.");
                }
                dbContext = new ApplicationDbContext(optionsBuilder.Options);
                // always delete before, may need proper rights
                dbContext.Database.EnsureDeleted();
                if (databaseProvider.ToLower().Trim().Equals("mysql"))
                {
                    var tmpOptionsBuilder = new DbContextOptionsBuilder<_tmpContext>();
                    tmpOptionsBuilder.UseMySQL(connectionString);
                    var tmpContext = new _tmpContext(tmpOptionsBuilder.Options);
                    // need to create w/out migrations
                    tmpContext.Database.EnsureCreated();
                    dbContext.Database.ExecuteSqlCommand(@"
                CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
                `MigrationId` VARCHAR(150) NOT NULL,
                `ProductVersion` VARCHAR(32) NOT NULL,
                PRIMARY KEY (`MigrationId`));
                ");
                }
                dbContext.Database.Migrate();

                // mock the managers and seed the data
                // var userStore = new UserStore<Users>(dbContext);
                // var roleStore = new RoleStore<IdentityRole>(dbContext);

                // IPasswordHasher<Users> hasher = new PasswordHasher<Users>();
                // var validator = new UserValidator<Users>();
                // var validators = new List<UserValidator<Users>> { validator };
                // var roleValidator = new RoleValidator<IdentityRole>();
                // var roleValidators = new List<RoleValidator<IdentityRole>> { roleValidator };
                // var _mockLogger = new Mock<ILogger<UserManager<Users>>>();
                // var logger = _mockLogger.Object;
                // userManager = new UserManager<Users>(userStore, null, hasher, validators, null, null, null, null, logger);
                // var roleLogger = new Mock<ILogger<RoleManager<IdentityRole>>>().Object;

                // roleManager = new RoleManager<IdentityRole>(roleStore, roleValidators, null, null, roleLogger);

                // default db
                if (seedData)
                {
                    // SeedUsers();
                    SeedDb();
                }

            }
            catch (System.Exception e)
            {
                var assertMsg = "Failed SeedDb for " + dbContext.Database.ProviderName + "\n";
                if (e.InnerException != null)
                    assertMsg += e.InnerException.Message;
                else
                    assertMsg += e.Message;
                _exceptionMessage = assertMsg;
            }
        }
    }
}