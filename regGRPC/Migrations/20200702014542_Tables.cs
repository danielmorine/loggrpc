using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace regGRPC.Migrations
{
    public partial class Tables : Migration
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
                    ID = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    ReportDescription = table.Column<string>(nullable: true),
                    ReportSource = table.Column<string>(nullable: true),
                    LevelTypeID = table.Column<byte>(nullable: false),
                    Events = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Report_LevelType_LevelTypeID",
                        column: x => x.LevelTypeID,
                        principalTable: "LevelType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationProcess",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ReportID = table.Column<Guid>(nullable: false),
                    OwnerID = table.Column<Guid>(nullable: false),
                    EnvironmentTypeID = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationProcess", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegistrationProcess_EnvironmentType_EnvironmentTypeID",
                        column: x => x.EnvironmentTypeID,
                        principalTable: "EnvironmentType",
                        principalColumn: "EnvironmentTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationProcess_Report_ReportID",
                        column: x => x.ReportID,
                        principalTable: "Report",
                        principalColumn: "ID",
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
