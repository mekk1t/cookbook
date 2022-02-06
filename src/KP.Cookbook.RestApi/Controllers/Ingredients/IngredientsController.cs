using KP.Cookbook.Domain.Entities;
using KP.Cookbook.RestApi.Controllers.Ingredients.Requests;
using Microsoft.AspNetCore.Mvc;

namespace KP.Cookbook.RestApi.Controllers.Ingredients
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly IngredientsService _service;

        public IngredientsController(IngredientsService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Ingredient> GetIngredients() => _service.Get();

        [HttpPost]
        public Ingredient CreateIngredient([FromBody] CreateIngredientRequest request) =>
            _service.Create(new Ingredient(request.Name, request.Type, request.Description));

        [HttpPatch]
        public void UpdateIngredient([FromBody] CreateIngredientRequest request) =>
            _service.Update(new Ingredient(request.Name, request.Type, request.Description));

        [HttpDelete("{id}")]
        public void DeleteIngredientById([FromRoute] long id) => _service.Delete(id);
    }
}