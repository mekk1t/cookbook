using Microsoft.EntityFrameworkCore.Migrations;

namespace KitProjects.Cookbook.Database.Migrations
{
    public partial class source_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SourceId",
                table: "Recipes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_SourceId",
                table: "Recipes",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Sources_SourceId",
                table: "Recipes",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Sources_SourceId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_SourceId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Recipes");
        }
    }
}
