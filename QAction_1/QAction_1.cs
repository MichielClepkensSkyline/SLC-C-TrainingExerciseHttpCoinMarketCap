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
				int status = Int32.Parse(statusCode.Split(' ')[1]);

				if (status == 200)
				{
					return true;
				}
				else
				{
					protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown: Status of the response is: {status}", LogType.Error, LogLevel.NoLogging);
					return false;
				}
			}

            public static bool CheckErrorCode(SLProtocol protocol, int errorCode, object errorMessage)
			{
				if (errorCode == 0)
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