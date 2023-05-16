using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenchmarkingPortal.Migrations.Base.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Benchmarks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Benchmarks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
