using KitProjects.MasterChef.Dal.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitProjects.MasterChef.Dal.Database
{
    public class EntityConfiguration
    {
        public class RecipeCategoryConfiguration : IEntityTypeConfiguration<DbRecipeCategory>
        {
            public void Configure(EntityTypeBuilder<DbRecipeCategory> builder)
            {
                builder.HasKey(rc => new { rc.DbRecipeId, rc.DbCategoryId });

                builder
                    .HasOne(rc => rc.DbRecipe)
                    .WithMany(r => r.RecipeCategoriesLink)
                    .HasForeignKey(r => r.DbRecipeId);

                builder
                    .HasOne(rc => rc.DbCategory)
                    .WithMany(c => c.RecipesCategoriesLink)
                    .HasForeignKey(c => c.DbCategoryId);
            }
        }

        public class RecipeStepIngredientConfiguration : IEntityTypeConfiguration<DbRecipeStepIngredient>
        {
            public void Configure(EntityTypeBuilder<DbRecipeStepIngredient> builder)
            {
                builder.HasKey(rc => new { rc.DbIngredientId, rc.DbRecipeStepId });

                builder
                    .HasOne(rc => rc.DbIngredient)
                    .WithMany(r => r.StepIngredientsLink)
                    .HasForeignKey(r => r.DbIngredientId);

                builder
                    .HasOne(rc => rc.DbRecipeStep)
                    .WithMany(c => c.StepIngredientsLink)
                    .HasForeignKey(c => c.DbRecipeStepId);

                builder
                    .Property(b => b.Measure)
                    .HasConversion<string>();
            }
        }

        public class RecipeStepConfiguration : IEntityTypeConfiguration<DbRecipeStep>
        {
            public void Configure(EntityTypeBuilder<DbRecipeStep> builder)
            {
                builder.Property(p => p.Id).ValueGeneratedNever();
            }
        }

        public class RecipeIngredientConfiguration : IEntityTypeConfiguration<DbRecipeIngredient>
        {
            public void Configure(EntityTypeBuilder<DbRecipeIngredient> builder)
            {
                builder.HasKey(rc => new { rc.DbRecipeId, rc.DbIngredientId });

                builder
                    .HasOne(rc => rc.DbRecipe)
                    .WithMany(r => r.RecipeIngredientLink)
                    .HasForeignKey(r => r.DbRecipeId);

                builder
                    .HasOne(rc => rc.DbIngredient)
                    .WithMany(c => c.RecipeIngredientsLink)
                    .HasForeignKey(c => c.DbIngredientId);

                builder
                    .Property(b => b.IngredientMeasure)
                    .HasConversion<string>();
            }
        }
    }
}
