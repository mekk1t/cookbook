using Microsoft.EntityFrameworkCore.Migrations;

namespace KP.Cookbook.Database.Migrations
{
    public partial class new_recipe_field_synopsis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Synopsis",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Synopsis",
                table: "Recipes");
        }
    }
}
