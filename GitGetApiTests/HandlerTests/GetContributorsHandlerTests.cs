using GitGetApi.GitDataAccess;
using GitGetApi.Handlers;
using GitGetApi.Queries;
using GitGetApi.Validators;
using Microsoft.Extensions.Configuration;
using Moq;

namespace GitGetApiTests.HandlerTests
{
    public class GetContributorsHandlerTests
    {
        /*
            These are just two integration tests, ideally there would be a suite of unit tests, there should be tests 
            for all the paths through the handler, the validators should be tested (no owner / repo specified results in 
            expected failure messages etc) and controller tests should be present too.

            TODO:

            The Unit tests would not be dependent on any outside factors such as internet connectivity or a specific GitHub 
            repository being available. These tests expose an issue with the code I've written, I should consider injecting 
            the HttpHandler into GitDataAccess.cs so I can use a Mocked HttpMessageHandler and then be able to write tests 
            which do not depend on internet access / github. GetContributors in GitDataAccess does too much and hides much
            of the functionality, it's not properly inverted so is hard to test, it's a compromise I've made for this exerscise.
        */

        private IConfiguration _configuration;

        private Mock<GetContributorsQuery> _getContributorsQuery = new();

        private readonly GetContributorsQueryValidator _getContributorsQueryValidator = new();

        private const int NumCommitsToPullFromGitHub = 20;

        [SetUp]
        public void Setup()
        {
            var myConfigValues = new Dictionary<string, string?>
            {
                {"NumCommitsToPullFromGitHub", $"{NumCommitsToPullFromGitHub}"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfigValues)
                .Build();
        }

        [Test]
        public void Call_GetContributorsHandler_With_Invalid_Owner_Details_Throws_Exception()
        {
            // Arrange
            _getContributorsQuery.Object.Owner = "fake-owner";
            _getContributorsQuery.Object.Repository = "e-bx_test_repo";

            var dataAccess = new GitDataAccess(_configuration);

            var handler = new GetContributorsHandler(dataAccess, _getContributorsQueryValidator);

            // Act & Assert
            Assert.Throws<Exception>(() => handler.Handle(_getContributorsQuery.Object, default));
        }

        [Test]
        public async Task Call_GetContributorsHandler_With_Valid_Details_Returns_Expected_Result()
        {
            // Arrange
            _getContributorsQuery.Object.Owner = "jonnarosey";
            _getContributorsQuery.Object.Repository = "e-bx_test_repo";

            var dataAccess = new GitDataAccess(_configuration);

            var handler = new GetContributorsHandler(dataAccess, _getContributorsQueryValidator);

            // Act
            var result = await handler.Handle(_getContributorsQuery.Object, default);

            // Assert
            Assert.That(result, Has.Count.EqualTo(NumCommitsToPullFromGitHub));
        }
    }
}