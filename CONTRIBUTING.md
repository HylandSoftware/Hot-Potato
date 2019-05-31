# Contributing

We invite all Hylanders to contribute to our repositories. We ask that any change be discussed via a [Jira Issue](https://jira.hylandqa.net/secure/RapidBoard.jspa?rapidView=178), [Mattermost](https://mattermost.hyland.com/hyland/channels/test-automation), via [Email](mailto:grp-automatedtesting@hyland.com), or in person. When preparing a contribution to our repository, please consider the following contribution guidelines.

## Code of Conduct

* Using welcoming and inclusive language
* Being respectful of differing viewpoints and experiences
* Gracefully accepting constructive criticism
* Focusing on what is best for the community
* Showing empathy towards other community members

## Testing

We expect all pull requests to have some amount of automated testing. Moreso, this repository uses a trunk based branching model, so it is imperitive that all changes be appropriately and completely tested before merging into master. Any pull request without automated tests may be declined immediately.

All of our test projects use the [XUnit](https://xunit.net/) test framework, [Moq](https://github.com/moq/moq) for mocking, [WireMock.Net](http://wiremock.org/) for HTTP server mocking in our system tests.

### Unit Tests

Our unit tests reside in projects that match the system under test with a `*.Test` suffix. So for `HotPotato.Core.csproj` the unit test project is `HotPotato.Core.Test.csproj`. When writing unit tests, try to isolate the system under test as best as possible, preferrably using mocks. Our team can provide assistance where needed.

### Integration Tests

Our integration test project tends to test more complicated integrated scenarios such as our validation logic using our proxy processor logic. If tests become too complicated or need multiple dependencies, place them in the `HotPotato.Integration.Test` project.

### System Tests

Our system tests are meant to act as a form of documentation for the multiple ways the proxy can be used to validate conformance to an API spec. They consist of a made-up API with a specification, a set of tests, and the HotPotato proxy set up as a reverse proxy. There are 3 suites that test valid tests, missing specification components, and invalid specification components which exist in `HotPotato.System.HappyPath.Test.csproj`, `HotPotato.System.NotInSpec.Test.csproj`, and `HotPotato.System.NonConformant.Test.csproj` respectively.

## Pull requests

When coding and testing is complete, a pull request (PR) can be created to merge. We will try to respond to the request within three business days.

* Make sure the entire Test Automation team is added to the PR (this should be done by default when creating the PR).
* Request will be declined immediately if there are no tests for added code
* Make sure there are no merge conflicts
* Make sure all tests are passing
* Make sure build is passing
* Update readme/documentation if necessary

## Style guide

* Try to follow [SOLID](https://en.wikipedia.org/wiki/SOLID) principles 
* Try to limit external dependencies when possible
* External dependencies should be isolated in a way that test doubles can be used 

