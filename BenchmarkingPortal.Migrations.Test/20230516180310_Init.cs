#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace BenchmarkingPortal.Migrations.Base.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "AspNetRoles",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });

        migrationBuilder.CreateTable(
            "ComputerGroups",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Description = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_ComputerGroups", x => x.Id); });

        migrationBuilder.CreateTable(
            "Configurations",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1")
            },
            constraints: table => { table.PrimaryKey("PK_Configurations", x => x.Id); });

        migrationBuilder.CreateTable(
            "Users",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Subscription = table.Column<bool>("bit", nullable: false),
                UserName = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: false),
                NormalizedUserName = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                Email = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>("bit", nullable: false),
                PasswordHash = table.Column<string>("nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>("nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>("nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>("nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>("bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>("bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>("datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>("bit", nullable: false),
                AccessFailedCount = table.Column<int>("int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
                table.UniqueConstraint("AK_Users_UserName", x => x.UserName);
            });

        migrationBuilder.CreateTable(
            "AspNetRoleClaims",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<int>("int", nullable: false),
                ClaimType = table.Column<string>("nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    x => x.RoleId,
                    "AspNetRoles",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ConfigurationItems",
            table => new
            {
                Key = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Value = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Scope = table.Column<string>("nvarchar(450)", nullable: false),
                ConfigurationId = table.Column<int>("int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ConfigurationItems", x => new { x.Key, x.Value, x.Scope, x.ConfigurationId });
                table.ForeignKey(
                    "FK_ConfigurationItem_Configuration",
                    x => x.ConfigurationId,
                    "Configurations",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "Constraints",
            table => new
            {
                Premise = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Consequence = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                ConfigurationId = table.Column<int>("int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Constraints", x => new { x.Premise, x.Consequence, x.ConfigurationId });
                table.ForeignKey(
                    "FK_Constraint_Configuration",
                    x => x.ConfigurationId,
                    "Configurations",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "AspNetUserClaims",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>("int", nullable: false),
                ClaimType = table.Column<string>("nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                table.ForeignKey(
                    "FK_AspNetUserClaims_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUserLogins",
            table => new
            {
                LoginProvider = table.Column<string>("nvarchar(450)", nullable: false),
                ProviderKey = table.Column<string>("nvarchar(450)", nullable: false),
                ProviderDisplayName = table.Column<string>("nvarchar(max)", nullable: true),
                UserId = table.Column<int>("int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    "FK_AspNetUserLogins_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUserRoles",
            table => new
            {
                UserId = table.Column<int>("int", nullable: false),
                RoleId = table.Column<int>("int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    x => x.RoleId,
                    "AspNetRoles",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_AspNetUserRoles_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUserTokens",
            table => new
            {
                UserId = table.Column<int>("int", nullable: false),
                LoginProvider = table.Column<string>("nvarchar(450)", nullable: false),
                Name = table.Column<string>("nvarchar(450)", nullable: false),
                Value = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    "FK_AspNetUserTokens_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Executables",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                OwnerTool = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                ToolVersion = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Path = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Name = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Version = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                UploadedDate = table.Column<DateTime>("datetime", nullable: false),
                UserName = table.Column<string>("nvarchar(256)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Executables", x => x.Id);
                table.ForeignKey(
                    "FK_Executable_User",
                    x => x.UserName,
                    "Users",
                    "UserName");
            });

        migrationBuilder.CreateTable(
            "SourceSets",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Path = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Version = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                UploadedDate = table.Column<DateTime>("datetime", nullable: false),
                UserName = table.Column<string>("nvarchar(256)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SourceSets", x => x.Id);
                table.ForeignKey(
                    "FK_SourceSet_User",
                    x => x.UserName,
                    "Users",
                    "UserName");
            });

        migrationBuilder.CreateTable(
            "Workers",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Ram = table.Column<int>("int", nullable: false),
                Cpu = table.Column<int>("int", nullable: false),
                Login = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Password = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Storage = table.Column<int>("int", nullable: false),
                Address = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Port = table.Column<int>("int", nullable: false),
                ComputerGroupId = table.Column<int>("int", nullable: false),
                AddedDate = table.Column<DateTime>("datetime", nullable: false),
                UserName = table.Column<string>("nvarchar(256)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Workers", x => x.Id);
                table.ForeignKey(
                    "FK_Worker_ComputerGroup",
                    x => x.ComputerGroupId,
                    "ComputerGroups",
                    "Id");
                table.ForeignKey(
                    "FK_Worker_User",
                    x => x.UserName,
                    "Users",
                    "UserName");
            });

        migrationBuilder.CreateTable(
            "Benchmarks",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Priority = table.Column<int>("int", nullable: false),
                Status = table.Column<string>("nvarchar(max)", nullable: false),
                Ram = table.Column<int>("int", nullable: false),
                Cpu = table.Column<int>("int", nullable: false),
                Result = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                TimeLimit = table.Column<int>("int", nullable: false),
                HardTimeLimit = table.Column<int>("int", nullable: false),
                ComputerGroupId = table.Column<int>("int", nullable: false),
                ExecutableId = table.Column<int>("int", nullable: false),
                SourceSetId = table.Column<int>("int", nullable: false),
                SetFilePath = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                PropertyFilePath = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                StartedDate = table.Column<DateTime>("datetime", nullable: false),
                ConfigurationId = table.Column<int>("int", nullable: false),
                UserName = table.Column<string>("nvarchar(256)", nullable: false),
                UserId = table.Column<int>("int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Benchmarks", x => x.Id);
                table.ForeignKey(
                    "FK_Benchmark_ComputerGroup",
                    x => x.ComputerGroupId,
                    "ComputerGroups",
                    "Id");
                table.ForeignKey(
                    "FK_Benchmark_Configuration",
                    x => x.ConfigurationId,
                    "Configurations",
                    "Id");
                table.ForeignKey(
                    "FK_Benchmark_Executable",
                    x => x.ExecutableId,
                    "Executables",
                    "Id");
                table.ForeignKey(
                    "FK_Benchmark_SourceSet",
                    x => x.SourceSetId,
                    "SourceSets",
                    "Id");
                table.ForeignKey(
                    "FK_Benchmark_User",
                    x => x.UserName,
                    "Users",
                    "UserName");
            });

        migrationBuilder.CreateIndex(
            "IX_AspNetRoleClaims_RoleId",
            "AspNetRoleClaims",
            "RoleId");

        migrationBuilder.CreateIndex(
            "RoleNameIndex",
            "AspNetRoles",
            "NormalizedName",
            unique: true,
            filter: "[NormalizedName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserClaims_UserId",
            "AspNetUserClaims",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserLogins_UserId",
            "AspNetUserLogins",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserRoles_RoleId",
            "AspNetUserRoles",
            "RoleId");

        migrationBuilder.CreateIndex(
            "IX_Benchmarks_ComputerGroupId",
            "Benchmarks",
            "ComputerGroupId");

        migrationBuilder.CreateIndex(
            "IX_Benchmarks_ConfigurationId",
            "Benchmarks",
            "ConfigurationId");

        migrationBuilder.CreateIndex(
            "IX_Benchmarks_ExecutableId",
            "Benchmarks",
            "ExecutableId");

        migrationBuilder.CreateIndex(
            "IX_Benchmarks_Name",
            "Benchmarks",
            "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_Benchmarks_SourceSetId",
            "Benchmarks",
            "SourceSetId");

        migrationBuilder.CreateIndex(
            "IX_Benchmarks_UserName",
            "Benchmarks",
            "UserName");

        migrationBuilder.CreateIndex(
            "IX_ConfigurationItems_ConfigurationId",
            "ConfigurationItems",
            "ConfigurationId");

        migrationBuilder.CreateIndex(
            "IX_Constraints_ConfigurationId",
            "Constraints",
            "ConfigurationId");

        migrationBuilder.CreateIndex(
            "IX_Executables_UserName",
            "Executables",
            "UserName");

        migrationBuilder.CreateIndex(
            "IX_SourceSets_UserName",
            "SourceSets",
            "UserName");

        migrationBuilder.CreateIndex(
            "EmailIndex",
            "Users",
            "NormalizedEmail");

        migrationBuilder.CreateIndex(
            "UserNameIndex",
            "Users",
            "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            "IX_Workers_ComputerGroupId",
            "Workers",
            "ComputerGroupId");

        migrationBuilder.CreateIndex(
            "IX_Workers_UserName",
            "Workers",
            "UserName");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "AspNetRoleClaims");

        migrationBuilder.DropTable(
            "AspNetUserClaims");

        migrationBuilder.DropTable(
            "AspNetUserLogins");

        migrationBuilder.DropTable(
            "AspNetUserRoles");

        migrationBuilder.DropTable(
            "AspNetUserTokens");

        migrationBuilder.DropTable(
            "Benchmarks");

        migrationBuilder.DropTable(
            "ConfigurationItems");

        migrationBuilder.DropTable(
            "Constraints");

        migrationBuilder.DropTable(
            "Workers");

        migrationBuilder.DropTable(
            "AspNetRoles");

        migrationBuilder.DropTable(
            "Executables");

        migrationBuilder.DropTable(
            "SourceSets");

        migrationBuilder.DropTable(
            "Configurations");

        migrationBuilder.DropTable(
            "ComputerGroups");

        migrationBuilder.DropTable(
            "Users");
    }
}