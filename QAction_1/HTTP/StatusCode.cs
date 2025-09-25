// Utility class for checking HTTP status codes
namespace Skyline.DataMiner.Scripting.HTTP
{
    using System;

    public static class StatusCode
    {
        public static bool CheckStatusCode(SLProtocol protocol, int statusId, int contentId, int urlId)
        {
            string statusLine = (string) protocol.GetParameter(statusId);
            int statusCode = Int32.Parse(statusLine.Split(' ')[1]);
            if (statusCode == 200)
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
