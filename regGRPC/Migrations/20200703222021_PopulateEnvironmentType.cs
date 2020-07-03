using Microsoft.EntityFrameworkCore.Migrations;

namespace regGRPC.Migrations
{
    public partial class PopulateEnvironmentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[EnvironmentType] ([EnvironmentTypeID] ,[Name] ,[NormalizedName]) VALUES (1, 'Produção', 'PRODUCAO')
                INSERT INTO [dbo].[EnvironmentType] ([EnvironmentTypeID] ,[Name] ,[NormalizedName]) VALUES (2, 'Homologação', 'HOMOLOGACAO')
                INSERT INTO [dbo].[EnvironmentType] ([EnvironmentTypeID] ,[Name] ,[NormalizedName]) VALUES (3, 'Dev', 'DEV')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE [dbo].[EnvironmentType]");
        }
    }
}
