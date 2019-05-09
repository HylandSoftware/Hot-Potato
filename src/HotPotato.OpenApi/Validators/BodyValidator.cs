
using HotPotato.OpenApi.Models;
using Newtonsoft.Json;
using NJsonSchema;
using NSwag;
using System.Collections.Generic;
using System.Xml;

namespace HotPotato.OpenApi.Validators
{
    internal class BodyValidator
    {
        public string BodyString { get; private set; }
        public string ContentType { get; }

        public BodyValidator(string bodyString, string contentType)
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
                var contentSchemas = SanitizeContentTypes(swagResp.Content);
                if (contentSchemas.ContainsKey(ContentType))
                {
                    specBody = contentSchemas[ContentType].Schema;
                }
            }

            if (specBody == null)
            {
                return new InvalidResult(Reason.MissingSpecBody);
            }
            else if(BodyString == "")
            {
                return new InvalidResult(Reason.MissingBody);
            }

            if (!ContentType.ToLower().Contains("json"))
            {
                ConvertBodyString();
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

        internal void ConvertBodyString()
        {
            if (ContentType.Contains("xml"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(BodyString);
                BodyString = JsonConvert.SerializeXmlNode(xmlDoc);
            }
            else
            {
                //this will allow "text/" content types to be validated
                //also cases like "application/pdf" will just need a string to be validated
                BodyString = JsonConvert.SerializeObject(BodyString);
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
