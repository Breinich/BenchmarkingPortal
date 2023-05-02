﻿// <auto-generated />

#nullable disable

using BenchmarkingPortal.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BenchmarkingPortal.Migrations.Test
{
    [DbContext(typeof(BenchmarkingDbContext))]
    [Migration("20230407220755_AddIdentity")]
    partial class AddIdentity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.Benchmark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ComputerGroupId")
                        .HasColumnType("int");

                    b.Property<int>("ConfigurationId")
                        .HasColumnType("int");

                    b.Property<int>("Cpu")
                        .HasColumnType("int");

                    b.Property<int>("ExecutableId")
                        .HasColumnType("int");

                    b.Property<int>("HardTimeLimit")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("PropertyFilePath")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Ram")
                        .HasColumnType("int");

                    b.Property<string>("Result")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SetFilePath")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("SourceSetId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TimeLimit")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ComputerGroupId");

                    b.HasIndex("ConfigurationId");

                    b.HasIndex("ExecutableId");

                    b.HasIndex("SourceSetId");

                    b.HasIndex("UserId");

                    b.ToTable("Benchmark", (string)null);
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.ComputerGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ComputerGroup", (string)null);
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.Configuration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Configuration", (string)null);
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.ConfigurationItem", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Scope")
                        .HasColumnType("int");

                    b.Property<int>("ConfigurationId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Key", "Scope", "ConfigurationId");

                    b.HasIndex("ConfigurationId");

                    b.ToTable("ConfigurationItem", (string)null);
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.Constraint", b =>
                {
                    b.Property<string>("Premise")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Consequence")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ConfigurationId")
                        .HasColumnType("int");

                    b.HasKey("Premise", "Consequence", "ConfigurationId");

                    b.HasIndex("ConfigurationId");

                    b.ToTable("Constraint", (string)null);
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.Executable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OwnerTool")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ToolVersion")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("UploadedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Executable", (string)null);
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.SourceSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("UploadedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SourceSet", (string)null);
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("GitHubUserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Subscription")
                        .HasColumnType("bit");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ComputerGroupId")
                        .HasColumnType("int");

                    b.Property<int>("Cpu")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<int>("Ram")
                        .HasColumnType("int");

                    b.Property<int>("Storage")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ComputerGroupId");

                    b.ToTable("Worker", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.Benchmark", b =>
                {
                    b.HasOne("BenchmarkingPortal.Dal.Entities.ComputerGroup", "ComputerGroup")
                        .WithMany("Benchmarks")
                        .HasForeignKey("ComputerGroupId")
                        .IsRequired()
                        .HasConstraintName("FK_Benchmark_ComputerGroup");

                    b.HasOne("BenchmarkingPortal.Dal.Entities.Configuration", "Configuration")
                        .WithMany("Benchmarks")
                        .HasForeignKey("ConfigurationId")
                        .IsRequired()
                        .HasConstraintName("FK_Benchmark_Configuration");

                    b.HasOne("BenchmarkingPortal.Dal.Entities.Executable", "Executable")
                        .WithMany("Benchmarks")
                        .HasForeignKey("ExecutableId")
                        .IsRequired()
                        .HasConstraintName("FK_Benchmark_Executable");

                    b.HasOne("BenchmarkingPortal.Dal.Entities.SourceSet", "SourceSet")
                        .WithMany("Benchmarks")
                        .HasForeignKey("SourceSetId")
                        .IsRequired()
                        .HasConstraintName("FK_Benchmark_SourceSet");

                    b.HasOne("BenchmarkingPortal.Dal.Entities.User", "User")
                        .WithMany("Benchmarks")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Benchmark_User");

                    b.Navigation("ComputerGroup");

                    b.Navigation("Configuration");

                    b.Navigation("Executable");

                    b.Navigation("SourceSet");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.ConfigurationItem", b =>
                {
                    b.HasOne("BenchmarkingPortal.Dal.Entities.Configuration", "Configuration")
                        .WithMany("ConfigurationItems")
                        .HasForeignKey("ConfigurationId")
                        .IsRequired()
                        .HasConstraintName("FK_ConfigurationItem_Configuration");

                    b.Navigation("Configuration");
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.Constraint", b =>
                {
                    b.HasOne("BenchmarkingPortal.Dal.Entities.Configuration", "Configuration")
                        .WithMany("Constraints")
                        .HasForeignKey("ConfigurationId")
                        .IsRequired()
                        .HasConstraintName("FK_Constraint_Configuration");

                    b.Navigation("Configuration");
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.Executable", b =>
                {
                    b.HasOne("BenchmarkingPortal.Dal.Entities.User", "User")
                        .WithMany("Executables")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Executable_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.SourceSet", b =>
                {
                    b.HasOne("BenchmarkingPortal.Dal.Entities.User", "User")
                        .WithMany("SourceSets")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_SourceSet_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.Worker", b =>
                {
                    b.HasOne("BenchmarkingPortal.Dal.Entities.ComputerGroup", "ComputerGroup")
                        .WithMany("Workers")
                        .HasForeignKey("ComputerGroupId")
                        .IsRequired()
                        .HasConstraintName("FK_Worker_ComputerGroup");

                    b.Navigation("ComputerGroup");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("BenchmarkingPortal.Dal.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("BenchmarkingPortal.Dal.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BenchmarkingPortal.Dal.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("BenchmarkingPortal.Dal.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.ComputerGroup", b =>
                {
                    b.Navigation("Benchmarks");

                    b.Navigation("Workers");
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.Configuration", b =>
                {
                    b.Navigation("Benchmarks");

                    b.Navigation("ConfigurationItems");

                    b.Navigation("Constraints");
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.Executable", b =>
                {
                    b.Navigation("Benchmarks");
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.SourceSet", b =>
                {
                    b.Navigation("Benchmarks");
                });

            modelBuilder.Entity("BenchmarkingPortal.Dal.Entities.User", b =>
                {
                    b.Navigation("Benchmarks");

                    b.Navigation("Executables");

                    b.Navigation("SourceSets");
                });
#pragma warning restore 612, 618
        }
    }
}
