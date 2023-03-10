using GitGetApi;
using GitGetApi.GitDataAccess;
using GitGetApi.Handlers;
using GitGetApi.Queries;
using GitGetApi.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        private Mock<ILogger<GitDataAccess>> _getContributorsLogger = new();

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

            var dataAccess = new GitDataAccess(_configuration, _getContributorsLogger.Object);

            var handler = new GetContributorsHandler(dataAccess, _getContributorsQueryValidator);

            // Act & Assert
            Assert.Throws<Exception>(() => handler.Handle(_getContributorsQuery.Object, default));
        }


        [Test]
        public void Call_GetContributorsHandler_With_Invalid_Repository_Details_Throws_Exception()
        {
            // Arrange
            _getContributorsQuery.Object.Owner = "jonnarosey";
            _getContributorsQuery.Object.Repository = "fake-repository";

            var dataAccess = new GitDataAccess(_configuration, _getContributorsLogger.Object);

            var handler = new GetContributorsHandler(dataAccess, _getContributorsQueryValidator);

            // Act & Assert
            Assert.Throws<Exception>(() => handler.Handle(_getContributorsQuery.Object, default));
        }

        [Test]
        public void Call_GetContributorsHandler_With_Invalid_Owner_And_Repository_Details_Returns_Not_Successful()
        {
            // Arrange
            _getContributorsQuery.Object.Owner = "fake-owner";
            _getContributorsQuery.Object.Repository = "fake-repository";

            var dataAccess = new GitDataAccess(_configuration, _getContributorsLogger.Object);

            var handler = new GetContributorsHandler(dataAccess, _getContributorsQueryValidator);

            // Act
            var result = handler.Handle(_getContributorsQuery.Object, default);

            // Assert
            Assert.IsFalse(result.Result.Success);
        }

        [Test]
        public void Call_GetContributorsHandler_With_No_Owner_Details_Throws_Exception()
        {
            // Arrange
            _getContributorsQuery.Object.Owner = "";
            _getContributorsQuery.Object.Repository = "e-bx_test_repo";

            var dataAccess = new GitDataAccess(_configuration, _getContributorsLogger.Object);

            var handler = new GetContributorsHandler(dataAccess, _getContributorsQueryValidator);

            // Act & Assert
            Assert.Throws<Exception>(() => handler.Handle(_getContributorsQuery.Object, default));
        }

        [Test]
        public void Call_GetContributorsHandler_With_No_Repository_Details_Throws_Exception()
        {
            // Arrange
            _getContributorsQuery.Object.Owner = "jonnarosey";
            _getContributorsQuery.Object.Repository = "";

            var dataAccess = new GitDataAccess(_configuration, _getContributorsLogger.Object);

            var handler = new GetContributorsHandler(dataAccess, _getContributorsQueryValidator);

            // Act & Assert
            Assert.Throws<Exception>(() => handler.Handle(_getContributorsQuery.Object, default));
        }

        [Test]
        public void Call_GetContributorsHandler_With_No_Owner_And_Repository_Details_Throws_Exception()
        {
            // Arrange
            _getContributorsQuery.Object.Owner = "";
            _getContributorsQuery.Object.Repository = "";

            var dataAccess = new GitDataAccess(_configuration, _getContributorsLogger.Object);

            var handler = new GetContributorsHandler(dataAccess, _getContributorsQueryValidator);

            // Act & Assert
            Assert.Throws<Exception>(() => handler.Handle(_getContributorsQuery.Object, default));
        }

        [Test]
        public void Call_GetContributorsHandler_With_Valid_Details_Is_Successful()
        {             
            // Arrange
            _getContributorsQuery.Object.Owner = "jonnarosey";
            _getContributorsQuery.Object.Repository = "e-bx_test_repo";

            var dataAccess = new GitDataAccess(_configuration, _getContributorsLogger.Object);

            var handler = new GetContributorsHandler(dataAccess, _getContributorsQueryValidator);

            // Act
            var result = handler.Handle(_getContributorsQuery.Object, default);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That((List<MediatrResult>)result.Result.Result, Has.Count.EqualTo(NumCommitsToPullFromGitHub));
        }

    }
}