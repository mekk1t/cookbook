using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Services
{
    public class IngredientService
    {
        private readonly IRepository<Ingredient, string> _ingredientRepository;

        public IngredientService(IRepository<Ingredient, string> ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public IEnumerable<Ingredient> SearchIngredients(ListQueryFilter filter) => _ingredientRepository.Read(filter);
        public Ingredient FindIngredient(string ingredientName) => _ingredientRepository.Find(ingredientName);
        public void CreateIngredient(Ingredient ingredient) => _ingredientRepository.Create(ingredient);
        public void EditIngredient(Ingredient ingredient) => _ingredientRepository.Update(ingredient);
        public void DeleteIngredient(string ingredientName)
        {
            var ingredient = _ingredientRepository.Find(ingredientName);
            _ingredientRepository.Delete(ingredient);
        }
    }
}
