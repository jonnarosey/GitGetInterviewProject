namespace GitGetApi.GitDataAccess
{
    public interface IGitDataAccess
    {
        Task<MediatrResult> GetContributors(string owner, string repository);
    }
}