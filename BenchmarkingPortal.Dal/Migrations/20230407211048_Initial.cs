using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BenchmarkingPortal.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComputerGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Executable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerTool = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ToolVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SourceSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Worker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ram = table.Column<int>(type: "int", nullable: false),
                    Cpu = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Storage = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    ComputerGroupId = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Worker_ComputerGroup",
                        column: x => x.ComputerGroupId,
                        principalTable: "ComputerGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationItem",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Scope = table.Column<int>(type: "int", nullable: false),
                    ConfigurationId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationItem", x => new { x.Key, x.Scope, x.ConfigurationId });
                    table.ForeignKey(
                        name: "FK_ConfigurationItem_Configuration",
                        column: x => x.ConfigurationId,
                        principalTable: "Configuration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Constraint",
                columns: table => new
                {
                    Premise = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Consequence = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConfigurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Constraint", x => new { x.Premise, x.Consequence, x.ConfigurationId });
                    table.ForeignKey(
                        name: "FK_Constraint_Configuration",
                        column: x => x.ConfigurationId,
                        principalTable: "Configuration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Benchmark",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Ram = table.Column<int>(type: "int", nullable: false),
                    Cpu = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TimeLimit = table.Column<int>(type: "int", nullable: false),
                    HardTimeLimit = table.Column<int>(type: "int", nullable: false),
                    ComputerGroupId = table.Column<int>(type: "int", nullable: false),
                    ExecutableId = table.Column<int>(type: "int", nullable: false),
                    SourceSetId = table.Column<int>(type: "int", nullable: false),
                    SetFilePath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PropertyFilePath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ConfigurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benchmark", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Benchmark_ComputerGroup",
                        column: x => x.ComputerGroupId,
                        principalTable: "ComputerGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Benchmark_Configuration",
                        column: x => x.ConfigurationId,
                        principalTable: "Configuration",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Benchmark_Executable",
                        column: x => x.ExecutableId,
                        principalTable: "Executable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Benchmark_SourceSet",
                        column: x => x.SourceSetId,
                        principalTable: "SourceSet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Benchmark_ComputerGroupId",
                table: "Benchmark",
                column: "ComputerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Benchmark_ConfigurationId",
                table: "Benchmark",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Benchmark_ExecutableId",
                table: "Benchmark",
                column: "ExecutableId");

            migrationBuilder.CreateIndex(
                name: "IX_Benchmark_SourceSetId",
                table: "Benchmark",
                column: "SourceSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationItem_ConfigurationId",
                table: "ConfigurationItem",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Constraint_ConfigurationId",
                table: "Constraint",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_ComputerGroupId",
                table: "Worker",
                column: "ComputerGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Benchmark");

            migrationBuilder.DropTable(
                name: "ConfigurationItem");

            migrationBuilder.DropTable(
                name: "Constraint");

            migrationBuilder.DropTable(
                name: "Worker");

            migrationBuilder.DropTable(
                name: "Executable");

            migrationBuilder.DropTable(
                name: "SourceSet");

            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "ComputerGroup");
        }
    }
}
