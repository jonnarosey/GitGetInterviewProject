using FluentValidation;
using GitGetApi.GitDataAccess;
using GitGetApi.Queries;
using MediatR;

namespace GitGetApi.Handlers
{
    public class GetContributorsHandler : IRequestHandler<GetContributorsQuery, List<string>>
    {
        private readonly IGitDataAccess _gitDataAccess;

        private readonly IValidator<GetContributorsQuery> _validator;

        public GetContributorsHandler(IGitDataAccess gitDataAccess, IValidator<GetContributorsQuery> validator)
        {
            _gitDataAccess = gitDataAccess;
            _validator = validator;
        }

        public Task<List<string>> Handle(GetContributorsQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            return Task.FromResult(_gitDataAccess.GetContributors(request.Owner, request.Repository));
        }
    }
}
