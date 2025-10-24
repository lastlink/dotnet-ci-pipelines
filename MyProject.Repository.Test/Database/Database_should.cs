using MyProject.Repository.Data;
using MyProject.Repository.Data.Models;
using MyProject.Repository.Test.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace MyProject.Repository.Test.Database
{
    [Collection("Database Test Collection")]
    public class Database_should : IClassFixture<RepositoryTestCache>
    {
        private DateTime DefaultDate { get; set; }
        //private DateTime DefaultJournalDate { get; set; }

        private Dictionary<string, TestDbContext> DbContexts { get; }

        public Database_should(RepositoryTestCache fixture)
        {
            // get db and run initial migrations
            // DbContexts = fixture.DbContexts;
            DefaultDate = fixture.DefaultDate;
            DbContexts = fixture.DbContexts;
            //DefaultJournalDate = (DateTime.Parse("2020/09/28")).RemoveTime();
        }

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            // FloatParseHandling = FloatParseHandling.Double, //hint
            // PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            NullValueHandling = NullValueHandling.Ignore,
            // Converters = new List<JsonConverter> { new DecimalJsonConverter() }
        };

        /// <summary>
        /// test blog data
        /// </summary>
        /// <param name="appSettingsKey"></param>
        /// <param name="stage"></param>
        [SkipDbIntegrationTheory]
        [AppsettingsArrayAttribute]
        public void HasBlogs(KeyValuePair<string, AppSettings> appSettings)
        {
            DbContexts[appSettings.Key].ValidSetup();
            var testData = "Accounts";
            var expectedResult = DataLoader.LoadJsonArray<Blog>(IntegratedUtils.DatabaseTestDataPath, testData);

            // get data from db
            var result = DbContexts[appSettings.Key].dbContext.Blog
                .OrderBy(x => x.Id)
                .ToList();
            // cleanup nested data
            CleanupBlogss(result);
            try
            {
                if (JsonConvert.SerializeObject(expectedResult) != JsonConvert.SerializeObject(result))
                {
                    var dataDir = Directory.GetCurrentDirectory() + DataLoader.rootPath + IntegratedUtils.DatabaseTestDataPath + testData + ".json";

                    File.WriteAllText(dataDir, JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented,
                              _jsonSerializerSettings));
                }
            }
            catch (System.Exception e)
            {
                // helps with cleaning up nested data
                var dataErrorMessage = "Error:";
                if (e.InnerException != null)
                    dataErrorMessage += e.InnerException.Message;
                else
                    dataErrorMessage += e.Message;
                Assert.Fail(dataErrorMessage);
            }


            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));


        }

        private void CleanupBlogss(List<Blog> result)
        {
            foreach (var blog in result)
            {
                if (blog.UpdatedAt.HasValue)
                    blog.UpdatedAt = DefaultDate;
            }
        }
    }
}