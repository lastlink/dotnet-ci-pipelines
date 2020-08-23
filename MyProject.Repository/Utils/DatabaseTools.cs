using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MyProject.Repository.Utils
{
    public class DatabaseTools
    {
        public class DatabaseKeys
        {
            public string ValueGenerationStrategy;
            public object SerialColumn;
            public string DEFAULTDATE_CREATE;
            public string DEFAULTDATE_UPDATE;
            public string dateTime;
            public bool UpdateDateTrigger = true;
            public string ProviderName { get; set; }
        }
        public static string getDefaultDate(string databaseProvider)
        {
            string DEFAULTDATE = "(getdate())";

            switch (databaseProvider)
            {
                case "Microsoft.EntityFrameworkCore.Sqlite":
                case "sqlite":
                    DEFAULTDATE = "date('now')";
                    break;
                case "postgress":
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    DEFAULTDATE = "CURRENT_TIMESTAMP";
                    break;
                case "mysql":
                case "MySql.Data.EntityFrameworkCore":
                    DEFAULTDATE = "UTC_TIMESTAMP()";
                    break;
            }
            return DEFAULTDATE;
        }
        /*
        Notes:
        can check if updateDateTrigger needed for dataprovider and add update triggers
        if (dbKeys.UpdateDateTrigger)
            {
                migrationBuilder.Sql(DatabaseTools.getUpdateDateTrigger(dbKeys.ProviderName, "TG_roles_updated_at", "roles", "updated_at"));
            }
        if postgress don't forget to drop the generated functions for each of the different named update columns (updated_at, modified_date) at the end of the migration E.g.
        protected override void Down(MigrationBuilder migrationBuilder){...
        if (dbKeys.ProviderName == "postgress")
            {
                migrationBuilder.Sql(@"
                    DROP FUNCTION update_" + "updated_at" + @"_column();
                    ");
            }
        postgres doesn't support parameters in triggers for new and old keywords.
         */
        /// <summary>
        /// needed for non mysql implementation to update a datetime on a record update
        /// </summary>
        /// <param name="databaseProvider"></param>
        /// <param name="name"></param>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="schema"></param>
        /// <param name="id_column"></param>
        /// <returns></returns>
        public static string getUpdateDateTrigger(string databaseProvider, string name, string table, string column, string schema = "", string id_column = "id")
        {
            string updateDateTrigger = null;
            if (!string.IsNullOrEmpty(schema))
                schema = schema + ".";
            switch (databaseProvider)
            {
                case "sqlite":
                    updateDateTrigger = @"
                    CREATE TRIGGER [" + name + @"]  
                        AFTER   
                        UPDATE  
                        ON " + table + @"
                        FOR EACH ROW   
                        WHEN NEW." + column + @" <= OLD." + column + @"  
                    BEGIN  
                        update " + table + @" set " + column + @"=CURRENT_TIMESTAMP where " + id_column + @"=OLD." + id_column + @";  
                    END  
                    ";
                    break;
                case "postgress":
                    updateDateTrigger = @"
                    CREATE OR REPLACE FUNCTION update_" + column + @"_column() RETURNS TRIGGER AS
                    $$
                    BEGIN
                        NEW." + column + @" = now();
                        RETURN NEW;  
                    END;
                    $$ LANGUAGE plpgsql;
                    CREATE TRIGGER " + name + @" 
                    BEFORE UPDATE ON " + schema + @"""" + table + @"""
                    FOR EACH ROW EXECUTE PROCEDURE update_" + column + @"_column('" + column + @"');
                    ";
                    break;
                case "sqlserver":
                    updateDateTrigger = @"
                    CREATE TRIGGER " + name + @" 
                    ON " + table + @"
                    AFTER UPDATE AS
                        UPDATE " + table + @"
                        SET " + column + @" = GETDATE()
                        WHERE " + id_column + @" IN (SELECT DISTINCT " + id_column + @" FROM Inserted);
                    ";
                    break;
            }

            return updateDateTrigger;
        }
        /*
        Notes:
        if in OnModelCreating can call as var dbKeys = DatabaseTools.getDatabaseDefaults(this.Database.ProviderName);
        if in a migration call as var dbKeys = DatabaseTools.getDatabaseDefaults(migrationBuilder.ActiveProvider);
         */
        /// <summary>
        /// Can get specific formats for different databases. Supports mssql, postgress, sqlite, and mysql.
        /// </summary>
        /// <param name="databaseProvider"></param>
        /// <returns></returns>
        public static DatabaseKeys getDatabaseDefaults(string databaseProvider)
        {
            var dbKeys = new DatabaseKeys();
            dbKeys.dateTime = "datetime";

            switch (databaseProvider)
            {
                case "Microsoft.EntityFrameworkCore.Sqlite":
                case "sqlite":
                    dbKeys.DEFAULTDATE_UPDATE = "datetime('now')";
                    dbKeys.SerialColumn = true;
                    dbKeys.ProviderName = "sqlite";
                    dbKeys.ValueGenerationStrategy = "Sqlite:Autoincrement";
                    break;
                case "postgress":
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    dbKeys.DEFAULTDATE_UPDATE = "CURRENT_TIMESTAMP";
                    dbKeys.SerialColumn = NpgsqlValueGenerationStrategy.SerialColumn;
                    dbKeys.ProviderName = "postgress";
                    dbKeys.ValueGenerationStrategy = "Npgsql:ValueGenerationStrategy";
                    dbKeys.dateTime = "timestamp";
                    break;
                case "mysql":
                case "MySql.Data.EntityFrameworkCore":
                    dbKeys.DEFAULTDATE_UPDATE = "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP";
                    dbKeys.DEFAULTDATE_CREATE = "CURRENT_TIMESTAMP";
                    dbKeys.UpdateDateTrigger = false;
                    dbKeys.SerialColumn = true;
                    dbKeys.ProviderName = "mysql";
                    dbKeys.ValueGenerationStrategy = "MySQL:AutoIncrement";
                    break;
                case "sqlserver":
                default:
                    dbKeys.DEFAULTDATE_UPDATE = "(getdate())";
                    dbKeys.SerialColumn = SqlServerValueGenerationStrategy.IdentityColumn;
                    dbKeys.ProviderName = "sqlserver";
                    dbKeys.ValueGenerationStrategy = "SqlServer:ValueGenerationStrategy";
                    break;
            }
            if (string.IsNullOrEmpty(dbKeys.DEFAULTDATE_CREATE))
                dbKeys.DEFAULTDATE_CREATE = dbKeys.DEFAULTDATE_UPDATE;
            return dbKeys;
        }
    }

}