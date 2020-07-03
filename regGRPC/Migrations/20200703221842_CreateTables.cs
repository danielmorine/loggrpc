using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace regGRPC.Migrations
{
    public partial class CreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnvironmentType",
                columns: table => new
                {
                    EnvironmentTypeID = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    NormalizedName = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentType", x => x.EnvironmentTypeID);
                });

            migrationBuilder.CreateTable(
                name: "LevelType",
                columns: table => new
                {
                    LevelTypeID = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true),
                    NormalizedName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelType", x => x.LevelTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    ReportID = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false),
                    ReportDescription = table.Column<string>(type: "VARCHAR(3000)", maxLength: 3000, nullable: false),
                    ReportSource = table.Column<string>(type: "VARCHAR(1000)", maxLength: 1000, nullable: false),
                    LevelTypeID = table.Column<byte>(nullable: false),
                    Events = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ReportID);
                    table.ForeignKey(
                        name: "FK_Report_LevelType_LevelTypeID",
                        column: x => x.LevelTypeID,
                        principalTable: "LevelType",
                        principalColumn: "LevelTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationProcess",
                columns: table => new
                {
                    RegistrationProcessID = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ReportID = table.Column<Guid>(nullable: false),
                    OwnerID = table.Column<Guid>(nullable: false),
                    EnvironmentTypeID = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationProcess", x => x.RegistrationProcessID);
                    table.ForeignKey(
                        name: "FK_RegistrationProcess_EnvironmentType_EnvironmentTypeID",
                        column: x => x.EnvironmentTypeID,
                        principalTable: "EnvironmentType",
                        principalColumn: "EnvironmentTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrationProcess_Report_ReportID",
                        column: x => x.ReportID,
                        principalTable: "Report",
                        principalColumn: "ReportID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationProcess_EnvironmentTypeID",
                table: "RegistrationProcess",
                column: "EnvironmentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationProcess_ReportID",
                table: "RegistrationProcess",
                column: "ReportID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Report_LevelTypeID",
                table: "Report",
                column: "LevelTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationProcess");

            migrationBuilder.DropTable(
                name: "EnvironmentType");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "LevelType");
        }
    }
}
