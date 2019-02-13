
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace HotPotato.Http.Default
{
    public class SpecValidationTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "specs/ccm/".ToPlatformPath(), HttpMethod.Get,
                HttpStatusCode.OK, "https://api.hyland.com/sms/messages/41", "application/json", new {
                    id = "SM4262411b90e5464b98a4f66a49c57a97",
                    created = "2019-01-04T15:08:09Z",
                    modified = "2019-01-04T15:08:09Z",
                    sent = "2019-01-04T15:08:09Z",
                    accountId = 96680,
                    from = "+5622089048",
                    to = "+15622089096",
                    body = "Test",
                    status = "accepted",
                    direction = "inbound",
                }
            };
            yield return new object[] { "specs/ccm/".ToPlatformPath(), HttpMethod.Post,
                HttpStatusCode.Accepted, "https://api.hyland.com/sms/messages", "application/json", new {
                    id = "SM4262411b90e5464b98a4f66a49c57a97",
                    created = "2019-01-04T15:08:09Z",
                    modified = "2019-01-04T15:08:09Z",
                    sent = "2019-01-04T15:08:09Z",
                    accountId = 96681,
                    from = "+5622089048",
                    to = "+15622089096",
                    body = "Test",
                    status = "accepted",
                    direction = "inbound",
                }
            };
            yield return new object[] {"specs/cv/".ToPlatformPath(), HttpMethod.Options,
                HttpStatusCode.BadRequest, "https://api.hyland.com/combined-viewer/combined-view-types/42/search-keyword-types", "application/problem+json", new {
                    items = new[]{
                        new {
                            id = "onetype",
                            name = "anothertype"
                        },
                        new {
                            id = "anothertype",
                            name = "anothertype"
                        }
                    }
                }
            };
            yield return new object[] { "specs/deficiencies/".ToPlatformPath(), HttpMethod.Get,
                HttpStatusCode.OK, "http://api.docs.hyland.io/deficiencies/deficiencies", "application/json", new {
                    items = new[] {
                        new {
                            id = "string",
                            physicianId = "string",
                            mpi = "string",
                            mpiAssigningAuthority = "string",
                            mrn = "string",
                            mrnAssigningAuthority = "string",
                            chartNumber = "string",
                            chartNumberAssigningAuthority = "string",
                            type = "MissingSignature",
                            patientFirstName = "string",
                            patientLastName = "string",
                            documentId = "string",
                            created = "2019-01-04T15:27:46Z",
                            deficiencyMessage = "string"
                        }
                    }
                }
            };
            yield return new object[] { "specs/document/".ToPlatformPath(), HttpMethod.Put,
                HttpStatusCode.BadRequest, "http://api.docs.hyland.io/document/documents/27/keywords", "application/problem+json", new {
                    type = "https://example.net/validation_error",
                    title = "Your request parameters didn't validate.",
                    status = 400,
                    detail = "The parameter `count` was not valid for the request.",
                    instance = "https://example.net/example-resource"
                }
            };
            yield return new object[] { "specs/document/".ToPlatformPath(), HttpMethod.Post,
                HttpStatusCode.Created, "http://api.docs.hyland.io/document/documents/", "application/json", new {
                    id = "string"
                }
            };
            
            yield return new object[] { "specs/rdds/configurationservice/".ToPlatformPath(), HttpMethod.Get,
                HttpStatusCode.OK, "https://api.hyland.com/ibpaf/rdds/configurations", "application/json", new {
                configurationCollection = new[]{
                    new {
                        configurationId = "string",
                        name = "string",
                        category = "string"
                        }
                    }
                }
            };
            
            yield return new object[] { "specs/rdds/messagestorageservice/".ToPlatformPath(), HttpMethod.Delete,
                HttpStatusCode.NotFound, "https://api.hyland.com/ibpaf/rdds/messages/78", "application/problem+json", new {
                    type = "https://example.net/validation_error",
                    title = "Your message id did not return a message to be deleted.",
                    status = 404,
                    detail = "message was not found for the given messageId, hence nothing will be deleted",
                    instance = "https://example.net/example-resource"
                }
            };
            
            yield return new object[] { "specs/workflow/".ToPlatformPath(), HttpMethod.Get,
                HttpStatusCode.OK, "https://api.hyland.com/workflow/life-cycles/48/", "application/json", new {
                    id = "string",
                    name = "string",
                    smallIconId = "string"
                }
            };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class SpecValidationNegTestData : IEnumerable<object[]>
    {
        //type and instace = invalid URI
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "specs/rdds/messagestorageservice/".ToPlatformPath(), HttpMethod.Delete,
            HttpStatusCode.NotFound, "https://api.hyland.com/ibpaf/rdds/messages/47", "application/problem+json", new {
                type = "https =//example.net/validation_error",
                title = "Your message id did not return a message to be deleted.",
                status = 404,
                detail = "message was not found for the given messageId, hence nothing will be deleted",
                instance = "/example-resource"
                }
            };

            //created = invalid date, accountId = invalid integer
            yield return new object[] { "specs/ccm/".ToPlatformPath(), HttpMethod.Get,
            HttpStatusCode.OK, "https://api.hyland.com/sms/messages/41", "application/json", new {
                id = "SM4262411b90e5464b98a4f66a49c57a97",
                created = "2019-01-04T15:08=09Z",
                modified = "2019-01-04T15:08:09Z",
                sent = "2019-01-04T15:08:09Z",
                accountId = "AC0db966d80e9f1662da09c61287f8bba1",
                from = "+5622089048",
                to = "+15622089096",
                body = "Test",
                status = "accepted",
                direction = "inbound",
            }
};
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}

