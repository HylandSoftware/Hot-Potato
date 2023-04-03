# Hot Potato Smoke Tests

## Happy Path - Tests execute against API with all endpoints defined in spec.

| Component         | Considerations    |
|--------------|-----------|
| Path | Paths with parameters such as "/custom-queries/{customQueryId}/keyword-types"      |
| Method      | A variety of methods to be tested: Delete, Get, Head, Options, Patch, Post, Put, Trace |
| Status Code      | Passing status codes from the 200/300 level and failing status codes in 400 level that are still defined in the spec.<br />The content-type for the defined 400-level responses will be something like "application/problem+json"<br />Special consideration should be given to '204 - No Content' status codes that have a special condition for body validation |
| Body      | Different content types such as application/json, application/xml, and text/plain, with some edge cases like application/pdf, application/octet-stream, image/png, etc.<br />Constraints on required properties such as string format, string length, and number ranges<br />Various data types to be validated like string, integer, bool, object, and array |
| Headers    | Constraints on required properties such as string format, string length, and number ranges<br />Various data types to be validated like string, integer, bool |

## Not in Spec - Spec defines endpoints not in API

| Scenario         | Description     | Example Location |
|--------------|-----------|------------|
| MissingPath | Send a request with a path not defined in the spec | GET /missingpath |
| MissingMethod | Send a request with a path defined in the spec, but with a method not listed underneath the defined path | GET /missingmethod |
| MissingStatusCode | Manipulate what status code will be returned to not be found in the spec, or create a path in the spec that doesn't expect a common status code like 200 | GET /missingcode |

## Non-conformant - API defines endpoints, but not conformant to spec

| Scenario         | Description     | Example Location |
|--------------|-----------|------------|
| InvalidBody | Missing properties, properties with unexpected types, strings not formatted correctly, malformed bodies, numbers not in range, strings too short/long | GET /order/555 |
| InvalidHeaders | Unexpected types, strings not formatted correctly, numbers not in range, strings too short/long | DELETE /order/666/items/666 |
| MissingBody | Have the response return with an empty body | GET /order/777 |
| MissingHeaders | Omit header(s) defined as required in the spec | DELETE /order/777/items/777 |
| MissingContent | Have the response return a content-type not listed in the spec | GET /order/888 |
| UnexpectedBody | For '204 - No Content' status codes - have a response return with an unexpected body | DELETE /order/555/items/555 |
