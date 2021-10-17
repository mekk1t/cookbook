using KP.Cookbook.Database;
using Microsoft.AspNetCore.Mvc;

namespace KP.Cookbook.UI.Controllers
{
    [Route("api/recipes")]
    public class RecipeController : Controller
    {
        private readonly RecipeRepository _repository;

        public RecipeController(RecipeRepository repository)
        {
            _repository = repository;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(long id)
        {
            var recipe = _repository.GetDetails(id);
            _repository.Delete(recipe);
            return NoContent();
        }
    }
}
