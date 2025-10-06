namespace Skyline.Protocol
{
	using System;
	using Skyline.DataMiner.Scripting;

	namespace QAction_1
    {
        public static class StatusCode
        {
            public static bool CheckStatusCode(SLProtocol protocol, int parameterId)
            {
				string statusCode = protocol.GetParameter(parameterId).ToString();
				string statusOK = "200";

				if (statusCode.Contains(statusOK))
				{
					return true;
				}
				else
				{
					protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown: Status of the response is: {statusCode}", LogType.Error, LogLevel.NoLogging);
					return false;
				}
			}

            public static bool CheckErrorCode(SLProtocol protocol, int errorCode, object errorMessage)
			{
				int errorCodeOK = 0;

				if (errorCode == errorCodeOK)
				{
					return true;
				}
				else
				{
					protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown: Status of the response is: {errorCode} \n Error Message: {errorMessage.ToString()}", LogType.Error, LogLevel.NoLogging);
					return false;
				}
			}
		}
    }
}