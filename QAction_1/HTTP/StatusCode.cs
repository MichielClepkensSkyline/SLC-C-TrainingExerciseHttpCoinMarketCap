// Utility class for checking HTTP status codes
namespace Skyline.DataMiner.Scripting.HTTP
{
    using System;

    public static class StatusCode
    {
        public static bool CheckStatusCode(SLProtocol protocol, int id)
        {
            string statusLine = (string) protocol.GetParameter(id);
            int statusCode = Int32.Parse(statusLine.Split(' ')[1]);
            if (statusCode == 200)
            {
                return true;
            }
            else
            {
                protocol.Log($"QA{protocol.QActionID}|CheckStatusCode|Bad statuscode: {statusLine}", LogType.Error, LogLevel.NoLogging);
                return false;
            }
        }
    }
}
