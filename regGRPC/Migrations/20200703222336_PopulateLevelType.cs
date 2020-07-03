using Microsoft.EntityFrameworkCore.Migrations;

namespace regGRPC.Migrations
{
    public partial class PopulateLevelType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
              INSERT INTO [dbo].[LevelType] ([LevelTypeID] ,[Name] ,[NormalizedName]) VALUES (1, 'Error', 'ERROR')
              INSERT INTO [dbo].[LevelType] ([LevelTypeID] ,[Name] ,[NormalizedName]) VALUES (2, 'Warning', 'WARNING')
              INSERT INTO [dbo].[LevelType] ([LevelTypeID] ,[Name] ,[NormalizedName]) VALUES (3, 'Debug', 'DEBUG')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE [dbo].[LevelType]");
        }
    }
}
