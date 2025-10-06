// Utility class for checking HTTP status codes
namespace Skyline.DataMiner.Scripting.HTTP
{
    using System;
    using System.Text.RegularExpressions;

    public static class StatusCode
    {
        public static bool CheckStatusCode(SLProtocol protocol, uint statusCodeId, uint responseContentId, uint urlId)
        {
            object[] parameters = (object[]) protocol.GetParameters(new uint[] { statusCodeId, responseContentId, urlId });

            string statusCode = (string)parameters[0];
            string responseContent = (string)parameters[1];
            string url = (string)parameters[2];

            var match = Regex.Match(statusCode, @"^HTTP\/\d\.\d\s+200(\s|$)");

            if (match.Success)
            {
                return true;
            }
            else
            {
                protocol.Log($"QA{protocol.QActionID}|CheckStatusCode|Bad statuscode:\nURL API call: {url} \nStatuscode: {statusCode}\nResponse content: {responseContent}", LogType.Error, LogLevel.NoLogging);
                return false;
            }
        }
    }
}
