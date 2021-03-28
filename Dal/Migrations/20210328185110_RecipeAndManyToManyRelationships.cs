using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KitProjects.MasterChef.Dal.Migrations
{
    public partial class RecipeAndManyToManyRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbRecipe",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbRecipe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbRecipeCategory",
                columns: table => new
                {
                    DbRecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DbCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbRecipeCategory", x => new { x.DbRecipeId, x.DbCategoryId });
                    table.ForeignKey(
                        name: "FK_DbRecipeCategory_Categories_DbCategoryId",
                        column: x => x.DbCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbRecipeCategory_DbRecipe_DbRecipeId",
                        column: x => x.DbRecipeId,
                        principalTable: "DbRecipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbRecipeIngredient",
                columns: table => new
                {
                    DbRecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DbIngredientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IngredientMeasure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IngredientxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbRecipeIngredient", x => new { x.DbRecipeId, x.DbIngredientId });
                    table.ForeignKey(
                        name: "FK_DbRecipeIngredient_DbRecipe_DbRecipeId",
                        column: x => x.DbRecipeId,
                        principalTable: "DbRecipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbRecipeIngredient_Ingredients_DbIngredientId",
                        column: x => x.DbIngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbRecipeStep",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DbRecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbRecipeStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbRecipeStep_DbRecipe_DbRecipeId",
                        column: x => x.DbRecipeId,
                        principalTable: "DbRecipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DbRecipeStepIngredient",
                columns: table => new
                {
                    DbRecipeStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DbIngredientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Measure = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbRecipeStepIngredient", x => new { x.DbIngredientId, x.DbRecipeStepId });
                    table.ForeignKey(
                        name: "FK_DbRecipeStepIngredient_DbRecipeStep_DbRecipeStepId",
                        column: x => x.DbRecipeStepId,
                        principalTable: "DbRecipeStep",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbRecipeStepIngredient_Ingredients_DbIngredientId",
                        column: x => x.DbIngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbRecipeCategory_DbCategoryId",
                table: "DbRecipeCategory",
                column: "DbCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DbRecipeIngredient_DbIngredientId",
                table: "DbRecipeIngredient",
                column: "DbIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_DbRecipeStep_DbRecipeId",
                table: "DbRecipeStep",
                column: "DbRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_DbRecipeStepIngredient_DbRecipeStepId",
                table: "DbRecipeStepIngredient",
                column: "DbRecipeStepId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbRecipeCategory");

            migrationBuilder.DropTable(
                name: "DbRecipeIngredient");

            migrationBuilder.DropTable(
                name: "DbRecipeStepIngredient");

            migrationBuilder.DropTable(
                name: "DbRecipeStep");

            migrationBuilder.DropTable(
                name: "DbRecipe");
        }
    }
}
