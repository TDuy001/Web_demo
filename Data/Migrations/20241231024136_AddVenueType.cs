using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo3.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVenueType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Venues",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Venues");
        }
    }
}
