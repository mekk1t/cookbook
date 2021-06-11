using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Models;
using System;
using System.Collections.Generic;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;

namespace KitProjects.MasterChef.WebApplication.ApplicationServices
{
    public class IngredientsCrud
    {
        private readonly ICommand<CreateIngredientCommand> _createIngredient;
        private readonly IQuery<IEnumerable<Ingredient>, GetIngredientsQuery> _getIngredients;
        private readonly IQuery<Ingredient, GetIngredientQuery> _getIngredient;
        private readonly ICommand<DeleteIngredientCommand> _deleteIngredient;
        private readonly ICommand<EditIngredientCommand> _editIngredient;

        public IngredientsCrud(
            ICommand<CreateIngredientCommand> createIngredient,
            IQuery<IEnumerable<Ingredient>, GetIngredientsQuery> getIngredients,
            IQuery<Ingredient, GetIngredientQuery> getIngredient,
            ICommand<DeleteIngredientCommand> deleteIngredient,
            ICommand<EditIngredientCommand> editIngredient)
        {
            _createIngredient = createIngredient;
            _getIngredients = getIngredients;
            _getIngredient = getIngredient;
            _deleteIngredient = deleteIngredient;
            _editIngredient = editIngredient;
        }

        public void Create(string name, string[] categories = default)
        {
            if (categories == default)
                _createIngredient.Execute(new CreateIngredientCommand(name, Array.Empty<string>()));
            else
                _createIngredient.Execute(new CreateIngredientCommand(name, categories));
        }

        public Ingredient Read(Guid id) => _getIngredient.Execute(new GetIngredientQuery(id));

        public Ingredient Read(string name) => _getIngredient.Execute(new GetIngredientQuery(name));

        public IEnumerable<Ingredient> Read() => _getIngredients.Execute(new GetIngredientsQuery());

        public IEnumerable<Ingredient> Read(int limit, int offset) =>
            _getIngredients.Execute(new GetIngredientsQuery(limit: limit, offset: offset));

        public void Update(Guid id, string name) => _editIngredient.Execute(new EditIngredientCommand(id, name));

        public void Delete(Guid id) => _deleteIngredient.Execute(new DeleteIngredientCommand(id));
    }
}
