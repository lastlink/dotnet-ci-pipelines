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
        private Dictionary<string, TestDbContext> _dbContexts { get; }
        public Blogs_should(RepositoryTestCache fixture)
        {
            // get db and run initial migrations
            _dbContexts = fixture.dbContexts;
        }

        private const string _userId = "test";

        [SkipDbIntegrationTheory]
        [AppsettingsArrayAttribute]
        public void hasBlog(KeyValuePair<string, AppSettings> appSettings)
        {
            _dbContexts[appSettings.Key].ValidSetup();
            Assert.NotNull(_dbContexts[appSettings.Key].dbContext.Blog.Where(b => b.Title == "test blog1").FirstOrDefault());
        }

        [SkipDbIntegrationTheory]
        [AppsettingsArrayAttribute]
        public void hasBlogs(KeyValuePair<string, AppSettings> appSettings)
        {
            _dbContexts[appSettings.Key].ValidSetup();
            Assert.True(_dbContexts[appSettings.Key].dbContext.Blog.Count() > 2);
        }
    }
}