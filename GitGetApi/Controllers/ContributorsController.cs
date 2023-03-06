using GitGetApi.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GitGetApi.Controllers
{
    [ApiController]
    [Route("/api/v1/{owner}/{repository}/[controller]")]
    public class ContributorsController : ControllerBase
    {
        private readonly ILogger<ContributorsController> _logger;

        private readonly IMediator _mediator;

        public ContributorsController(ILogger<ContributorsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public IEnumerable<string> Get(string owner, string repository)
        {
            _logger.LogInformation("Contributors Get called with owner:{owner} and repository:{repository}", owner, repository);

            Task<MediatrResult> authors = _mediator.Send(new GetContributorsQuery() { Owner = owner, Repository = repository });

            return (List<string>)authors.Result.Result;
        }

    }
}
