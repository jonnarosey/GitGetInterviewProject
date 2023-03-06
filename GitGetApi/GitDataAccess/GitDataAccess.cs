using Newtonsoft.Json;

namespace GitGetApi.GitDataAccess
{
    public class GitDataAccess : IGitDataAccess
    {
        private List<string> _committerNames;

        private readonly IConfiguration _configuration;

        private ILogger<GitDataAccess> _logger;

        public GitDataAccess(IConfiguration configuration, ILogger<GitDataAccess> logger)
        {
            _committerNames = new List<string>();
            _configuration = configuration;
            _logger = logger;
        }

        /*
            TODO:
            This could return a result object which could have a success true / false as well as the result
            we could use this to eliminate the throwing of exceptions and reutrn a 200 with a message or a
            400 / 404 instead of a 500 when the repo isn't found.
        */

        public List<string> GetContributors(string owner, string repository)
        {
            int numResults = _configuration.GetValue<int>("NumCommitsToPullFromGitHub");

            string gitApiUrl = $"https://api.github.com/repos/{owner}/{repository}/commits?per_page={numResults}";

            try
            {
                using (var client = new HttpClient())
                {
                    // Add required request headers
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                    var response = client.GetAsync(gitApiUrl).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var gitResult = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);

                        foreach (var committerName in gitResult)
                        {
                            _committerNames.Add(committerName.commit.author.name.ToString());
                        }
                    }
                    else
                    {
                        throw new Exception("Call to git was not successful, check owner / repository values.");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error trying to access:{gitApiUrl}, {e.Message} {e.StackTrace}");
                throw;
            }
            return _committerNames;
        }
    }
}
