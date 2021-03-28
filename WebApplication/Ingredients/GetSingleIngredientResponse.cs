using KitProjects.MasterChef.Kernel.Models;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Ingredients
{
    public class GetSingleIngredientResponse
    {
        public Guid Id { get; }
        public string Name { get; }
        public IEnumerable<Category> Categories { get; }

        public GetSingleIngredientResponse(Guid id, string name, IEnumerable<Category> categories)
        {
            Id = id;
            Name = name;
            Categories = categories;
        }
    }
}
