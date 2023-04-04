# External Examples

Working examples of endpoints and OpenApi specifications for exploratory conformance testing

| REMOTE_ENDPOINT | SPEC_LOCATION | Source | Example Endpoint |
|--------------|-----------|------------|------------|
| https://indikatorer-api.naturvardsverket.se/ | https://raw.githubusercontent.com/greentechdev/greentechdev.github.io/master/environmental_indicators_api.yaml | https://greentechdev.github.io/data/environmental-indicators/ | /api/v1/indicators |
| https://api.vimeo.com | https://raw.githubusercontent.com/vimeo/openapi/master/api.yaml | https://github.com/vimeo/openapi | /channels/{channel_id}/privacy/users<br />*this will return a 401 error code with a content-type of application/vnd.vimeo.error+json. The spec expects application/vnd.vimeo.user+json, so Hot Potato yields a MissingContentType validation error. |
| https://api.hel.fi/respa/v1 | https://raw.githubusercontent.com/City-of-Helsinki/respa/develop/openapi.yaml | https://github.com/City-of-Helsinki/respa | /reservations |
