namespace QAction_2.Helpers
{
	using System;
	using System.Text.RegularExpressions;

	using QAction_2.Enums;

	using Skyline.DataMiner.Scripting;

	public static class ResponseStatusHelper
	{
		public static int ParseResponseStatusCode(SLProtocolExt protocol, string responseStatus)
		{
			if (string.IsNullOrWhiteSpace(responseStatus))
			{
				throw new ArgumentException("The response status does not represent a valid message or is empty.");
			}

			var regex = new Regex(@"\d{3}");
			var match = regex.Match(responseStatus);

			if (match.Success)
			{
				var statusCode = int.Parse(match.Value);
				SetAuthenticationStatusParameter(protocol, statusCode);
				return statusCode;
			}
			else
			{
				throw new ArgumentException("The response status is not in valid format.");
			}
		}

		private static void SetAuthenticationStatusParameter(SLProtocolExt protocol, int statusCode)
		{
			if (statusCode == 401)
			{
				protocol.SetParameter(Parameter.authenticationstatus_3, AuthenticationStatus.Unsuccesssful);
			}
			else if (statusCode >= 500)
			{
				protocol.SetParameter(Parameter.authenticationstatus_3, AuthenticationStatus.NotAvailable);
			}
			else
			{
				protocol.SetParameter(Parameter.authenticationstatus_3, AuthenticationStatus.Successful);
			}
		}
	}
}
