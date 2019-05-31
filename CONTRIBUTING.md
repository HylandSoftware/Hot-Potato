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

## Pull requests

When coding and testing is complete, a pull request can be created to merge. We will try to respond to the request within three business days.

* Make sure the entire Test Automation team is added to the PR (this should be done by default when creating the PR).
* Request will be declined immediately if there are no tests for added code
* Make sure there are no merge conflicts
* Make sure all tests are passing
* Make sure build is passing
* Update readme/documentation if necessary

## Style guide

* Try to follow SOLID principles 
* Try to limit external dependencies when possible
* External dependencies should be isolated in a way that test doubles can be used 

