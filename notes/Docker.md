# Local Docker Testing Notes

To test the Docker functionality locally, you can do a Docker build locally in the root folder of your Hot Potato solution, passing in an arbitrary number for IMAGE_VERSION. Also make sure to create a docker network beforehand, like so:

`docker network create hp`

`docker build --tag hot-potato:4.8 --build-arg IMAGE_VERSION=4.8 .`

Now to test, you may pass in values for REMOTE_ENDPOINT and SPEC_LOCATION through the command line. Connecting Docker with your localhost requires some nontrivial network setup, so I recommend using an external API like so:

`docker run --rm -d --network hp --name Conformance -p 3232:3232 -e HttpClientSettings__IgnoreClientHttpsCertificateValidationErrors=true -e REMOTE_ENDPOINT=https://indikatorer-api.naturvardsverket.se -e SPEC_LOCATION=https://raw.githubusercontent.com/greentechdev/greentechdev.github.io/master/environmental_indicators_api.yaml hot-potato:4.8`

`Those values represent the API and spec of the Swedish EPA's list of environmental indicators. The endpoints can be found here: https://greentechdev.github.io/data/environmental-indicators/, and as of 10/28/21, the endpoints yield conformant results. My go-to endpoint is /api/v1/indicators`
