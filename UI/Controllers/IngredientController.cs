using KP.Cookbook.Database;
using KP.Cookbook.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace KP.Cookbook.UI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Route("api/ingredients")]
    public class IngredientController : ControllerBase
    {
        private readonly Repository<Ingredient> _repository;

        public IngredientController(Repository<Ingredient> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            return new JsonResult(_repository.GetList(limit: int.MaxValue));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CreateNewIngredient([FromBody] Ingredient ingredient)
        {
            _repository.Save(ingredient);
            return new JsonResult(ingredient.Id);
        }
    }
}
