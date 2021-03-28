using Microsoft.EntityFrameworkCore.Migrations;

namespace KitProjects.MasterChef.Dal.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbRecipeCategory_DbRecipe_DbRecipeId",
                table: "DbRecipeCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_DbRecipeIngredient_DbRecipe_DbRecipeId",
                table: "DbRecipeIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_DbRecipeStep_DbRecipe_DbRecipeId",
                table: "DbRecipeStep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbRecipe",
                table: "DbRecipe");

            migrationBuilder.RenameTable(
                name: "DbRecipe",
                newName: "Recipes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbRecipeCategory_Recipes_DbRecipeId",
                table: "DbRecipeCategory",
                column: "DbRecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DbRecipeIngredient_Recipes_DbRecipeId",
                table: "DbRecipeIngredient",
                column: "DbRecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DbRecipeStep_Recipes_DbRecipeId",
                table: "DbRecipeStep",
                column: "DbRecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbRecipeCategory_Recipes_DbRecipeId",
                table: "DbRecipeCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_DbRecipeIngredient_Recipes_DbRecipeId",
                table: "DbRecipeIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_DbRecipeStep_Recipes_DbRecipeId",
                table: "DbRecipeStep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes");

            migrationBuilder.RenameTable(
                name: "Recipes",
                newName: "DbRecipe");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbRecipe",
                table: "DbRecipe",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbRecipeCategory_DbRecipe_DbRecipeId",
                table: "DbRecipeCategory",
                column: "DbRecipeId",
                principalTable: "DbRecipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DbRecipeIngredient_DbRecipe_DbRecipeId",
                table: "DbRecipeIngredient",
                column: "DbRecipeId",
                principalTable: "DbRecipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DbRecipeStep_DbRecipe_DbRecipeId",
                table: "DbRecipeStep",
                column: "DbRecipeId",
                principalTable: "DbRecipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
