using KitProjects.Api.AspNetCore;
using KitProjects.Cookbook.Core.Abstractions;
using KitProjects.Cookbook.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KitProjects.Cookbook.Controllers
{
    public class CategoriesController : ApiJsonController
    {
        private readonly ICrud<Category, long> _crud;

        public CategoriesController(
            ILogger<CategoriesController> logger,
            ICrud<Category, long> crud) : base(logger)
        {
            _crud = crud;
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById([FromRoute] long id) =>
            ProcessRequest(() => _crud.Read(id));

        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category) =>
            ProcessRequest(() => _crud.Create(category));

        [HttpPut]
        public IActionResult UpdateCategory([FromBody] Category category) =>
            ProcessRequest(() => _crud.Update(category));

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory([FromRoute] long id) =>
            ProcessRequest(() =>
            {
                var category = _crud.Read(id);
                _crud.Delete(category);
            });
    }
}
