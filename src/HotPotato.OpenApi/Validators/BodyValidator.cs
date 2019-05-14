
using HotPotato.OpenApi.Models;
using HotPotato.Core.Http;
using Newtonsoft.Json;
using NJsonSchema;
using NSwag;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    internal class BodyValidator
    {
        public string BodyString { get; private set; }
        public HttpContentType ContentType { get; }

        public BodyValidator(string bodyString, HttpContentType contentType)
        {
            if (string.IsNullOrWhiteSpace(bodyString))
            {
                BodyString = "";
            }
            else
            {
                BodyString = bodyString;
            }

            ContentType = contentType;
        }

        public IValidationResult Validate(SwaggerResponse swagResp)
        {
            JsonSchema4 specBody = swagResp.ActualResponse.Schema;

            //Conditional for matching schemas with multiple content-type returns
            if (swagResp.Content != null && swagResp.Content.Count > 0)
            {
                Dictionary<string, OpenApiMediaType> contentSchemas = SanitizeContentTypes(swagResp.Content);
                if (contentSchemas.ContainsKey(ContentType.Type))
                {
                    specBody = contentSchemas[ContentType.Type].Schema;
                }
            }

            if (specBody == null)
            {
                return new InvalidResult(Reason.MissingSpecBody);
            }
            else if (BodyString == "")
            {
                return new InvalidResult(Reason.MissingBody);
            }

            if (!ContentType.Type.ToLower().Contains("json") && !ContentType.Type.ToLower().Contains("xml"))
            {
                //this will allow "text/" content types to be validated
                //also cases like "application/pdf" will just need a string to be validated
                BodyString = JsonConvert.SerializeObject(BodyString);
            }
            else if (ContentType.Type.ToLower().Contains("xml"))
            {
                var xmlErrList = specBody.ValidateXml(BodyString);
                if (xmlErrList.Count == 0)
                {
                    return new ValidResult();
                }
                else
                {
                    ValidationError[] xmlErrorArr = xmlErrList.ToArray();
                    return new InvalidResult(Reason.InvalidBody, xmlErrorArr);
                }
            }

            ICollection<NJsonSchema.Validation.ValidationError> errors = specBody.Validate(BodyString);
            if (errors == null || errors.Count == 0)
            {
                return new ValidResult();
            }
            else
            {
                List<ValidationError> errList = errors.ToValidationErrorList();
                ValidationError[] errorArr = errList.ToArray();
                return new InvalidResult(Reason.InvalidBody, errorArr);
            }
        }

        /// <summary>
        /// Remove all trailing encodings from content-types for uniform matching
        /// </summary>
        /// <param name="Content">A SwaggerResponse.Content dict</param>
        /// <returns>Content dict with sanitized content-types</returns>
        private Dictionary<string, OpenApiMediaType> SanitizeContentTypes(IDictionary<string, OpenApiMediaType> Content)
        {
            Dictionary<string, OpenApiMediaType> returnDict = new Dictionary<string, OpenApiMediaType>();
            foreach (KeyValuePair<string, OpenApiMediaType> kvp in Content)
            {
                if (kvp.Key.Contains(";"))
                {
                    returnDict.Add(kvp.Key.Split(";")[0], kvp.Value);
                }
                else
                {
                    returnDict.Add(kvp.Key, kvp.Value);
                }
            }
            return returnDict;
        }
    }
}
