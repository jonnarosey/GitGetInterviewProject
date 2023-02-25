# GitGetInterviewProject
Test Project for e-bx

The solution can be found in the GitGetApi folder, running the solution presents a swagger page for easy testing of the endpoint.

There are two comments, marked TODO, showing where I'd like to make improvements:

I've written a comment in GetContributorsHandlerTests.cs explaining that the next step I'd like to take is to untangle the logic in GitDataAccess.cs so that it's properly testable and how I would do this, I've not done this because of time contstraints.

Additionally the code returns a 500 with a message when it fails to find a repository, I don't like throwing exceptions like this so have suggested an alternative in GitDataAccess.cs.