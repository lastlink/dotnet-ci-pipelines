using MyProject.Repository.Data;
using MyProject.Repository.Test.Helper;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MyProject.Repository.Test.Database
{
    [Collection("Database Test Collection")]
    public class Blogs_should : IClassFixture<RepositoryTestCache>
    {
        private Dictionary<string, TestDbContext> DbContexts { get; }
        public Blogs_should(RepositoryTestCache fixture)
        {
            // get db and run initial migrations
            DbContexts = fixture.DbContexts;
        }

        [SkipDbIntegrationTheory]
        [AppsettingsArrayAttribute]
        public void HasBlog(KeyValuePair<string, AppSettings> appSettings)
        {
            DbContexts[appSettings.Key].ValidSetup();
            Assert.NotNull(DbContexts[appSettings.Key].dbContext.Blog.Where(b => b.Title == "test blog1").FirstOrDefault());
        }

        [SkipDbIntegrationTheory]
        [AppsettingsArrayAttribute]
        public void HasBlogs(KeyValuePair<string, AppSettings> appSettings)
        {
            DbContexts[appSettings.Key].ValidSetup();
            Assert.True(DbContexts[appSettings.Key].dbContext.Blog.Count() > 2);
        }
    }
}