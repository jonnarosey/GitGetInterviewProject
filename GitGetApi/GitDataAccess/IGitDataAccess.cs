namespace GitGetApi.GitDataAccess
{
    public interface IGitDataAccess
    {
        List<string> GetContributors(string owner, string repository);
    }
}