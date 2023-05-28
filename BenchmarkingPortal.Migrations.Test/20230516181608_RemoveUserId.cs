#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace BenchmarkingPortal.Migrations.Base.Migrations;

/// <inheritdoc />
public partial class RemoveUserId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "UserId",
            "Benchmarks");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            "UserId",
            "Benchmarks",
            "int",
            nullable: false,
            defaultValue: 0);
    }
}