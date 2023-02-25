using Newtonsoft.Json;

namespace GitGetApi.GitDataAccess
{
    public class GitDataAccess : IGitDataAccess
    {
        private List<string> _committerNames;

        private readonly IConfiguration _configuration;

        public GitDataAccess(IConfiguration configuration)
        {
            _committerNames = new List<string>();
            _configuration = configuration;
        }

        /*
            TODO:
            This could return a result object which could have a success true / false as well as the result
            we could use this to eliminate the throwing of exceptions and reutrn a 404 instead of a 500 when the repo isn't found.
        */

        public List<string> GetContributors(string owner, string repository)
        {
            int numResults = _configuration.GetValue<int>("NumCommitsToPullFromGitHub");

            string apiUrl = $"https://api.github.com/repos/{owner}/{repository}/commits?per_page={numResults}";

            using (var client = new HttpClient())
            {
                // Add required request headers
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                var response = client.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var gitResult = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);

                    if (gitResult == null)
                    {
                        throw new Exception("Result from git was not successfully deserialized");
                    }

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
            return _committerNames;
        }
    }
}
