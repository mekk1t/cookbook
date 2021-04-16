using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class EditRecipeIngredientDescriptionDecorator : ICommand<EditRecipeIngredientDescriptionCommand>
    {
        private readonly ICommand<EditRecipeIngredientDescriptionCommand> _decoratee;

        public EditRecipeIngredientDescriptionDecorator(
            ICommand<EditRecipeIngredientDescriptionCommand> decoratee)
        {
            _decoratee = decoratee;
        }

        public void Execute(EditRecipeIngredientDescriptionCommand command)
        {
            if (!command.HasValues)
                return;

            _decoratee.Execute(command);
        }
    }
}
