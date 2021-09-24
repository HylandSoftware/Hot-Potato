
using HotPotato.OpenApi.Validators;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace HotPotato.Http.Default
{
	public class CustomSpecTestData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			yield return new object[] { "specs/rawpotato/", HttpMethod.Delete,
			HttpStatusCode.OK, "https://api.hyland.com/v1/48/plans/48", "application/xml",
				@"<BaseResponse><Code>48</Code><Message>Deleted with a valid xml</Message></BaseResponse>" };

			yield return new object[] { "specs/rawpotato/", HttpMethod.Get,
					HttpStatusCode.OK, "https://api.hyland.com/risks/48/accountSnapshot", "application/octet-stream",
					"ByteStringsAreBase64"};

			yield return new object[] { "specs/rawpotato/", HttpMethod.Get,
					HttpStatusCode.OK, "https://api.hyland.com/risks/48/accountSnapshot", "text/csv",
						@"booking_date;purpose;amount;currency;counter_iban;counter_bic;counter_holder;tags;category_id" +
						@"26.07.2007; 'CONCLUSION NO INFORMATION, SEE GGF. STATEMENT OF ACCOUNT'; -7.32; EUR; ; ; ; ;" };

			yield return new object[] { "specs/rawpotato/", HttpMethod.Get,
					HttpStatusCode.OK, "https://api.hyland.com/order/4/finishedFile/8", "application/pdf",
						"101" };

			yield return new object[] { "specs/rawpotato/", HttpMethod.Get,
					HttpStatusCode.OK, "https://api.hyland.com/order/4/finishedFile/8", "image/jpg",
						"10101" };

			yield return new object[] { "specs/rdds/configurationservice/", HttpMethod.Get,
				HttpStatusCode.OK, "https://api.hyland.com/ibpaf/rdds/configurations/48/content", "text/plain", "Configuration Content"};
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	public class CustomSpecNegTestData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			yield return new object[] { "specs/rawpotato/", HttpMethod.Delete,
			HttpStatusCode.OK, "https://api.hyland.com/v1/48/plans/48", "application/xml",
				@"<BaseResponse><Code>48</Code><Message>Deleted with a malformed xml</Message>", ValidationErrorKind.InvalidXml };

			yield return new object[] { "specs/rawpotato/", HttpMethod.Delete,
			HttpStatusCode.OK, "https://api.hyland.com/v1/48/plans/48", "application/xml",
				@"<BaseResponse><Message>Deleted with a missing required property</Message></BaseResponse>", ValidationErrorKind.PropertyRequired };

			yield return new object[] { "specs/rawpotato/", HttpMethod.Get,
					HttpStatusCode.OK, "https://api.hyland.com/risks/48/accountSnapshot", "application/octet-stream",
					"Byte Strings Are Base64", ValidationErrorKind.Base64Expected};

			yield return new object[] { "specs/rawpotato/", HttpMethod.Get,
					HttpStatusCode.OK, "https://api.hyland.com/risks/48/accountSnapshot", "application/pdf",
					"'ByteStringsAreBase64'", ValidationErrorKind.Base64Expected};

		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

