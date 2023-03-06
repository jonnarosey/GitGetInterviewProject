using FluentValidation;
using GitGetApi.GitDataAccess;
using GitGetApi.Queries;
using MediatR;

namespace GitGetApi.Handlers
{
    public class GetContributorsHandler : IRequestHandler<GetContributorsQuery, MediatrResult>
    {
        private readonly IGitDataAccess _gitDataAccess;

        private readonly IValidator<GetContributorsQuery> _validator;

        public GetContributorsHandler(IGitDataAccess gitDataAccess, IValidator<GetContributorsQuery> validator)
        {
            _gitDataAccess = gitDataAccess;
            _validator = validator;
        }

        public Task<MediatrResult> Handle(GetContributorsQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            return _gitDataAccess.GetContributors(request.Owner, request.Repository);    
        }
    }
}
