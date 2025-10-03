// Utility class for checking HTTP status codes
namespace Skyline.DataMiner.Scripting.HTTP
{
    using System;
    using System.Text.RegularExpressions;

    public static class StatusCode
    {
        public static bool CheckStatusCode(SLProtocol protocol, int statusId, int contentId, int urlId)
        {
            string statusLine = (string)protocol.GetParameter(statusId);
            var match = Regex.Match(statusLine, @"^HTTP\/\d\.\d\s+200(\s|$)");

            if (match.Success)
            {
                return true;
            }
            else
            {
                protocol.Log($"QA{protocol.QActionID}|CheckStatusCode|Bad statuscode:\nURL API call: {protocol.GetParameter(urlId)} \nStatuscode: {statusLine}\nResponse content: {protocol.GetParameter(contentId)}", LogType.Error, LogLevel.NoLogging);
                return false;
            }
        }
    }
}
