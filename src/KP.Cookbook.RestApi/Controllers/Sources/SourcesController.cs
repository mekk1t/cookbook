using KP.Cookbook.Domain.Entities;
using KP.Cookbook.RestApi.Controllers.Sources.Requests;
using KP.Cookbook.Services;
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
        public Source CreateIngredient([FromBody] UpsertSourceRequest request) =>
            _service.Create(new Source(request.Name)
            {
                Description = request.Description,
                Image = request.Image,
                IsApproved = request.IsApproved,
                Link = request.Link
            });

        [HttpPatch("{id}")]
        public void UpdateIngredient([FromBody] UpsertSourceRequest request, [FromRoute] long id) =>
            _service.Update(new Source(id, request.Name)
            {
                Description = request.Description,
                Image = request.Image,
                IsApproved = request.IsApproved,
                Link = request.Link
            });

        [HttpDelete("{id}")]
        public void DeleteIngredientById([FromRoute] long id) => _service.Delete(id);
    }
}
