using MediatR;

namespace GitGetApi.Queries
{
    public class GetContributorsQuery : IRequest<MediatrResult>
    {
        public string Owner { get; set; } = string.Empty;
        public string Repository { get; set; } = string.Empty;
    }
}
