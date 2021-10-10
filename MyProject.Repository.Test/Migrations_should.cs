using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MyProject.Repository.Data;
using MyProject.Repository.Test.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace MyProject.Repository.Test
{
    [Collection("Database Test Collection")]
    public class Migrations_should
    {
        [SkipDbMigrationIntegrationTheory]
        [shouldRollbackTestData]
        public void shouldRollback(KeyValuePair<string, AppSettings> appSettings)
        {
            var success = false;
            var assertMsg = "";
            try
            {
                // don't use the same database as the main tests.
                appSettings.Value.connectionStrings.DefaultConnection = appSettings.Value.connectionStrings.DefaultConnection.Replace("mockDb", "rollback_mockDb");
                var context = new TestDbContext(appSettings.Value.connectionStrings, seedData: false);
                context.ValidSetup();
                var migrations = context.dbContext.Database.GetPendingMigrations();
                Assert.True(migrations.Count() == 0);
                // context.dbContext.Database.MigrateAsync();
                context.dbContext.GetService<IMigrator>().Migrate("0");
                migrations = context.dbContext.Database.GetPendingMigrations();
                Assert.True(migrations.Count() > 0);
                // ontext.dbContext.Database.EnsureCreated();
                success = context.dbContext.Database.EnsureDeleted();
            }
            catch (System.Exception e)
            {
                assertMsg = "Failed for " + appSettings.Value.connectionStrings.Provider + "\n" +
                    e.Message;
                Console.WriteLine(assertMsg);
                success = false;
            }
            Assert.True(success, assertMsg);

        }
    }
    public class SkipDbIntegrationTheory : TheoryAttribute
    {
        public SkipDbIntegrationTheory()
        {
            var skipReason = DataLoader.DataExists<AppSettings>("");
            if (skipReason == true.ToString())
            {
                var array = DataLoader.loadJsonDictonary<AppSettings>("");
                // skip mssql in gitlab
                skipReason = (!(array.Count == 1 && array.ContainsKey("mssql"))).ToString();
                if (skipReason != true.ToString())
                {
                    skipReason = "mssql tests are skipped";
                }
            }
            else
            {
                skipReason = "Missing" + skipReason;
            }
            if (skipReason != true.ToString())
            {
                var msg = "Database tests skipped: " + skipReason;
                Console.WriteLine(msg);
                Skip = msg;
            }
        }
    }

    public class SkipDbMigrationIntegrationTheory : TheoryAttribute
    {
        public SkipDbMigrationIntegrationTheory()
        {
            var skipReason = DataLoader.DataExists<AppSettings>("");
            if (skipReason != true.ToString())
            {
                var msg = "Database Migration tests skipped: Missing" + skipReason;
                Console.WriteLine(msg);
                Skip = msg;
            }
        }
    }

    public class SkipDbIntegrationFact : FactAttribute
    {
        public SkipDbIntegrationFact()
        {
            var skipReason = DataLoader.DataExists<AppSettings>("");

            if (skipReason != true.ToString())
            {
                var msg = "Database tests skipped: Missing" + skipReason;
                Console.WriteLine(msg);
                Skip = msg;
            }
        }
    }

    public class shouldRollbackTestData : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (DataLoader.DataExists<AppSettings>("") == true.ToString())
            {
                var array = DataLoader.loadJsonDictonary<AppSettings>("");
                foreach (var appSettings in array)
                {
                    yield return new object[] {
                        appSettings
                    };
                }
            }
            else
            {
                yield return new object[] {
                   null
                };
            }
        }
    }

    public class AppsettingsArrayAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (DataLoader.DataExists<AppSettings>("") == true.ToString())
            {
                var array = DataLoader.loadJsonDictonary<AppSettings>("");
                array.Remove("mssql");
                if (array.Count > 0)
                    foreach (var appSettings in array)
                    {
                        yield return new object[] {
                            appSettings
                        };
                    }
                else
                {
                    yield return new object[] {
                        null
                    };
                }
            }
            else
            {
                yield return new object[] {
                   null
                };
            }
        }
    }
}