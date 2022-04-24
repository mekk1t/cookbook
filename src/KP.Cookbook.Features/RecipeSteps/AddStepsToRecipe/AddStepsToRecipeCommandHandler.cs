using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP.Cookbook.Features.RecipeSteps.AddStepsToRecipe
{
    public class AddStepsToRecipeCommandHandler : ICommandHandler<AddStepsToRecipeCommand>
    {
        private readonly RecipesRepository _recipesRepository;

        public AddStepsToRecipeCommandHandler(RecipesRepository recipesRepository)
        {
            _recipesRepository = recipesRepository;
        }

        public void Execute(AddStepsToRecipeCommand command)
        {
            // Если шагов нет, то достаточно этого шага
            _recipesRepository.AddStepsToRecipe(command.RecipeId, command.CookingStepsCollection);

            // Если шаги уже есть, то возможные конфликты:
            // 1. Уже есть шаг с таким порядковым номером. НУЖНА ВАЛИДАЦИЯ
            // 2. Новый шаг(и) добавляются в конец (конфликта, на самом деле, нет. Достаточно также просто добавить в БД)
            // 3. Новый шаг(и) добавляются между разными шагами либо до какого-то шага. НУЖНА НОРМАЛИЗАЦИЯ
        }
    }
}
