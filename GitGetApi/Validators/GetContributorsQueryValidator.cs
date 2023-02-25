using FluentValidation;
using GitGetApi.Queries;

namespace GitGetApi.Validators
{
    public class GetContributorsQueryValidator : AbstractValidator<GetContributorsQuery>
    {
        public GetContributorsQueryValidator()
        {
            RuleFor(x => x.Owner)
                .NotEmpty()
                .WithMessage("Git Repository Owner must be specified");
            RuleFor(x => x.Repository)
                .NotEmpty()
                .WithMessage("Git Repository must be specified");
        }
    }
}
