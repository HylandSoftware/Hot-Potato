using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using Newtonsoft.Json;
using NJsonSchema;
using NSwag;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
	internal class HeaderValidator
	{
		public HttpHeaders Headers { get; }

		public HeaderValidator(HttpHeaders headers)
		{
			Headers = headers;
		}

		public IValidationResult Validate(OpenApiResponse swagResp)
		{
			if (swagResp.Headers != null && swagResp.Headers.Count > 0)
			{
				foreach (var swagHeader in swagResp.Headers)
				{
					string headerKey = swagHeader.Key;
					if (Headers == null || !Headers.ContainsKey(headerKey))
					{
						return new InvalidResult(Reason.MissingHeaders);
					}
					else
					{
						List<string> headerValues = Headers[headerKey];
						foreach (string value in headerValues)
						{
							string jValue = JsonConvert.SerializeObject(value);

							JsonSchema swagHeaderSchema;
							if (swagHeader.Value?.ActualSchema != null)
							{
								swagHeaderSchema = GetHeaderSchema(swagHeader.Value.ActualSchema);
							}
							else
							{
								return new InvalidResult(Reason.NullHeaderSchema);
							}

							ICollection<NJsonSchema.Validation.ValidationError> errors = swagHeaderSchema.Validate(jValue);

							if (errors != null && errors.Count != 0)
							{
								List<ValidationError> errList = errors.ToValidationErrorList();
								ValidationError[] errorArr = errList.ToArray();
								return new InvalidResult(Reason.InvalidHeaders, errorArr);
							}

						}
					}

				}
			}
			return new ValidResult();
		}

		/// <summary>
		/// Gets the header schema needed for validation, either in the base schema or the ExtentionData
		/// If a spec header's schema is in a ref, it will be set in the ExtensionData property
		/// </summary>
		/// <param name="schema"></param>
		/// <returns></returns>
		private static JsonSchema GetHeaderSchema(JsonSchema schema)
		{
			if (schema.ExtensionData != null &&
				schema.ExtensionData.ContainsKey("schema") 
				&& schema.ExtensionData["schema"] != null)
			{
				return (JsonSchema)schema.ExtensionData["schema"];
			}
			else
			{
				return schema;
			}
		}
	}
}
