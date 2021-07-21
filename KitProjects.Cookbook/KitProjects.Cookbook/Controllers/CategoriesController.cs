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
            ProcessRequest(() =>
            {
                if (filter == null)
                    filter = new PaginationFilter();

                var categories = _categoryRepository.GetList(new PaginationFilter
                {
                    LastId = filter.LastId,
                    Limit = filter.Limit + 1
                });
                if (categories == null)
                    return new ApiCollectionResponse<Category>(null, false);

                bool thereAreMoreCategories = categories.Count == filter.Limit + 1;
                if (thereAreMoreCategories)
                    categories.RemoveAt(categories.Count - 1);

                return new ApiCollectionResponse<Category>(categories, thereAreMoreCategories);
            });

        [HttpGet("{id}")]
        public IActionResult GetCategoryById([FromRoute] long id) =>
            ProcessRequest(() => new ApiObjectResponse<Category>(_crud.Read(id)));

        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category) =>
            ProcessRequest(() => new ApiObjectResponse<Category>(_crud.Create(category)));

        [HttpPut]
        public IActionResult UpdateCategory([FromBody] Category category) =>
            ProcessRequest(() => new ApiObjectResponse<Category>(_crud.Update(category)));

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory([FromRoute] long id) =>
            ProcessRequest(() =>
            {
                var category = _crud.Read(id);
                _crud.Delete(category);
            });
    }
}
