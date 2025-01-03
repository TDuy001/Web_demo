using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo3.Data.Migrations
{
    /// <inheritdoc />
    public partial class CheckCategoriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" 
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U')) 
            BEGIN 
                CREATE TABLE [dbo].[Categories] (
                [CategoryId] int NOT NULL IDENTITY,
                [Name] nvarchar(100) NOT NULL, 
                CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryId]) );
            END 
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
