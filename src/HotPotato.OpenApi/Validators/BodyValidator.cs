
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
        public string bodyString { get; private set; }
        public string contentType { get; }
        public Reason FailReason { get; private set; }
        public ValidationError[] ErrorArr { get; private set; }

        public BodyValidator(string BodyString, string ContentType)
        {
            if (string.IsNullOrWhiteSpace(BodyString))
            {
                bodyString = "";
            }
            else
            {
                bodyString = BodyString;
            }
            contentType = ContentType;
        }

        public bool Validate(SwaggerResponse swagResp)
        {
            if (swagResp.ActualResponse == null || swagResp.ActualResponse.Schema == null)
            {
                FailReason = Reason.MissingSpecBody;
                return false;
            }
            else if(bodyString == "")
            {
                FailReason = Reason.MissingBody;
                return false;
            }

            if (!contentType.ToLower().Contains("json"))
            {
                ConvertBodyString();
            }

            JsonSchema4 specBody = swagResp.ActualResponse.Schema;
            ICollection<NJsonSchema.Validation.ValidationError> errors = specBody.Validate(bodyString);
            if (errors == null || errors.Count == 0)
            {
                return true;
            }
            else
            {
                List<ValidationError> errList = errors.ToValidationErrorList();
                ErrorArr = errList.ToArray();
                FailReason = Reason.InvalidBody;
                return false;
            }
        }

        internal void ConvertBodyString()
        {
            if (contentType.Contains("xml"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(bodyString);
                bodyString = JsonConvert.SerializeXmlNode(xmlDoc);
            }
            else
            {
                //this will allow "text/" content types to be validated
                //also cases like "application/pdf" will just need a string to be validated
                bodyString = JsonConvert.SerializeObject(bodyString);
            }
        }
    }
}
