﻿using KP.Cookbook.Cqrs;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Features.Sources.CreateSource;
using KP.Cookbook.Features.Sources.DeleteSource;
using KP.Cookbook.Features.Sources.GetSourceDetails;
using KP.Cookbook.Features.Sources.GetSources;
using KP.Cookbook.Features.Sources.UpdateSource;
using KP.Cookbook.RestApi.Controllers.Sources.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KP.Cookbook.RestApi.Controllers.Sources
{
    [ApiController]
    [Route("[controller]")]
    public class SourcesController : ControllerBase
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
            IQueryHandler<GetSourcesQuery, List<Source>> getSources)
        {
            _createSource = createSource;
            _deleteSource = deleteSource;
            _updateSource = updateSource;
            _getSourceDetails = getSourceDetails;
            _getSources = getSources;
        }

        [HttpGet]
        public List<Source> GetSources() => _getSources.Execute(GetSourcesQuery.Empty);

        [HttpGet("{id}")]
        public Source GetSourceDetails([FromRoute] long id) => _getSourceDetails.Execute(new GetSourceDetailsQuery(id));

        [HttpPost]
        public Source CreateSource([FromBody] UpsertSourceRequest request) =>
            _createSource.Execute(
                new CreateSourceCommand(
                    new Source(request.Name)
                    {
                        Description = request.Description,
                        Image = request.Image,
                        IsApproved = request.IsApproved,
                        Link = request.Link
                    }));

        [HttpPatch("{id}")]
        public void UpdateIngredient([FromBody] UpsertSourceRequest request, [FromRoute] long id) =>
            _updateSource.Execute(
                new UpdateSourceCommand(
                    new Source(id, request.Name)
                    {
                        Description = request.Description,
                        Image = request.Image,
                        IsApproved = request.IsApproved,
                        Link = request.Link
                    }));

        [HttpDelete("{id}")]
        public void DeleteIngredientById([FromRoute] long id) => _deleteSource.Execute(new DeleteSourceCommand(id));
    }
}
