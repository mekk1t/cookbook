using KP.Cookbook.Cqrs;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Features.Sources.CreateSource;
using KP.Cookbook.Features.Sources.DeleteSource;
using KP.Cookbook.Features.Sources.GetSourceDetails;
using KP.Cookbook.Features.Sources.GetSources;
using KP.Cookbook.Features.Sources.UpdateSource;
using KP.Cookbook.RestApi.Controllers.Sources.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace KP.Cookbook.RestApi.Controllers.Sources
{
    [ApiController]
    [Route("[controller]")]
    public class SourcesController : CookbookApiJsonController
    {
        private readonly ICommandHandler<CreateSourceCommand, Source> _createSource;
        private readonly ICommandHandler<DeleteSourceCommand> _deleteSource;
        private readonly ICommandHandler<UpdateSourceCommand> _updateSource;
        private readonly IQueryHandler<GetSourceDetailsQuery, Source> _getSourceDetails;
        private readonly IQueryHandler<GetSourcesQuery, List<Source>> _getSources;

        public SourcesController(
            ICommandHandler<CreateSourceCommand, Source> createSource,
            ICommandHandler<DeleteSourceCommand> deleteSource,
            ICommandHandler<UpdateSourceCommand> updateSource,
            IQueryHandler<GetSourceDetailsQuery, Source> getSourceDetails,
            IQueryHandler<GetSourcesQuery, List<Source>> getSources,
            ILogger<SourcesController> logger) : base(logger)
        {
            _createSource = createSource;
            _deleteSource = deleteSource;
            _updateSource = updateSource;
            _getSourceDetails = getSourceDetails;
            _getSources = getSources;
        }

        [HttpGet]
        public IActionResult GetSources() => ExecuteCollectionRequest(() => _getSources.Execute(GetSourcesQuery.Empty));

        [HttpGet("{id}")]
        public IActionResult GetSourceDetails([FromRoute] long id) =>
            ExecuteObjectRequest(() => _getSourceDetails.Execute(new GetSourceDetailsQuery(id)));

        [HttpPost]
        public IActionResult CreateSource([FromBody] UpsertSourceRequest request) =>
            ExecuteObjectRequest(() => _createSource.Execute(
                new CreateSourceCommand(
                    new Source(request.Name)
                    {
                        Description = request.Description,
                        Image = request.Image,
                        IsApproved = request.IsApproved,
                        Link = request.Link
                    })));

        [HttpPatch("{id}")]
        public IActionResult UpdateSource([FromBody] UpsertSourceRequest request, [FromRoute] long id) =>
            ExecuteAction(() => _updateSource.Execute(
                new UpdateSourceCommand(
                    new Source(id, request.Name)
                    {
                        Description = request.Description,
                        Image = request.Image,
                        IsApproved = request.IsApproved,
                        Link = request.Link
                    })));

        [HttpDelete("{id}")]
        public IActionResult DeleteSourceById([FromRoute] long id) => ExecuteAction(() => _deleteSource.Execute(new DeleteSourceCommand(id)));
    }
}
