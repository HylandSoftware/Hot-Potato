using Newtonsoft.Json;
using NJsonSchema;
using NSwag;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace HotPotato.OpenApi.Validators
{
    public static class BodyValidatorExtensions
    {
        // Since preserving xml element data types for validation would require work beyond the scope of this project,
        // this validator will check if the body is a parsable xml and if all the required properties are present
        public static List<ValidationError> ValidateXml(this JsonSchema @this, string xmlBody)
        {
            List<ValidationError> errorList = new List<ValidationError>();

            try
            {
                XElement xElem = XElement.Parse(xmlBody);
            }
            catch (XmlException ex)
            {
                ValidationError valErr = new ValidationError(ex.Message, ValidationErrorKind.InvalidXml, "", ex.LineNumber, ex.LinePosition);
                errorList.Add(valErr);
                return errorList;
            }

            IReadOnlyDictionary<string, JsonSchemaProperty> expectedProperties = @this.ActualSchema?.ActualProperties;
            if (expectedProperties == null || expectedProperties.Count == 0)
            {
                return errorList;
            }

            foreach (KeyValuePair<string, JsonSchemaProperty> property in expectedProperties)
            {
                if (property.Value.IsRequired && !xmlBody.Contains(property.Key))
                {
                    ValidationError valErr = new ValidationError($"Required property {property.Key} was not found in the response body", ValidationErrorKind.PropertyRequired, "", 0, 0);
                    errorList.Add(valErr);
                }
            }
            return errorList;
        }

        //this will allow "text/" content types to be validated
        //also cases like "application/pdf" will just need a string to be validated
        public static string ToJsonText(this string @this)
        {
            return JsonConvert.SerializeObject(@this);
        }

        /// <summary>
        /// Remove all trailing encodings from content-types for uniform matching
        /// </summary>
        /// <param name="this">A OpenApiResponse.Content dic</param>
        /// <returns>Content dict with sanitized content-types</returns>
        public static Dictionary<string, OpenApiMediaType> SanitizeContentTypes(this IDictionary<string, OpenApiMediaType> @this)
        {
            Dictionary<string, OpenApiMediaType> returnDict = new Dictionary<string, OpenApiMediaType>();
            foreach (KeyValuePair<string, OpenApiMediaType> kvp in @this)
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
