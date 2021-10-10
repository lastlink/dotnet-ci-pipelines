using Microsoft.EntityFrameworkCore.Migrations;
using MyProject.Repository.Utils;
using System;

namespace MyProject.Repository.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var dbKeys = DatabaseTools.GetDatabaseDefaults(migrationBuilder.ActiveProvider);

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(dbKeys.ValueGenerationStrategy, dbKeys.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blog");
        }
    }
}
