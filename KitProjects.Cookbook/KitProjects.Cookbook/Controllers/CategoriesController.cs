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
        private readonly IRepository<Category, PaginationFilter> _categoryRepository;

        public CategoriesController(
            ILogger<CategoriesController> logger,
            ICrud<Category, long> crud,
            IRepository<Category, PaginationFilter> categoryRepository) : base(logger)
        {
            _crud = crud;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetCategories([FromQuery] PaginationFilter filter) =>
            ProcessRequest(() => _categoryRepository.GetList(filter));

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
