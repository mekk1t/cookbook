using KP.Cookbook.Domain.Entities;
using KP.Cookbook.RestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace KP.Cookbook.RestApi.Controllers.Sources
{
    [ApiController]
    [Route("[controller]")]
    public class SourcesController : ControllerBase
    {
        private readonly SourcesService _service;

        public SourcesController(SourcesService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Source> GetSources() => _service.Get();

        [HttpPost]
        public Source CreateIngredient([FromBody] CreateIngredientRequest request) =>
            _service.Create(new Source(request.Name, request.Type, request.Description));

        [HttpPatch("{id}")]
        public void UpdateIngredient([FromBody] CreateIngredientRequest request, [FromRoute] long id) =>
            _service.Update(new Source(id, request.Name, request.Type, request.Description));

        [HttpDelete("{id}")]
        public void DeleteIngredientById([FromRoute] long id) => _service.Delete(id);
    }
}
