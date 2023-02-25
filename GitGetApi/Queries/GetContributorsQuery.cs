using MediatR;

namespace GitGetApi.Queries
{
    public class GetContributorsQuery : IRequest<List<string>>
    {
        public string Owner { get; set; } = string.Empty;
        public string Repository { get; set; } = string.Empty;
    }
}
