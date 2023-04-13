using BenchmarkingPortal.Dal.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenchmarkingPortal.Dal.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurationItemwScopeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_ConfigurationItem", "ConfigurationItem");

            migrationBuilder.AlterColumn<string>(
                name: "Scope",
                table: "ConfigurationItem",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey("PK_ConfigurationItem", "ConfigurationItem",
                new string[] { "Key", "Scope", "ConfigurationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Scope",
                table: "ConfigurationItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
