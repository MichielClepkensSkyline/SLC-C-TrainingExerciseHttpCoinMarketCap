namespace Skyline.Protocol
{
    using QAction_1;
    using Skyline.DataMiner.Net.Messages;
    using Skyline.DataMiner.Net.Messages.SLDataGateway;
    using Skyline.DataMiner.Scripting;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    namespace MyExtension
    {
        public class HelperMethods
        {
            public virtual bool CheckStatusCode(object httpResponseParameter, SLProtocol protocol)
            {
                string httpToString = httpResponseParameter.ToString();
                string statusCodeString = httpToString.Split(' ')[1];
                int statusCode = int.Parse(statusCodeString);

                if (statusCode == 200)
                {
                    return true;
                }
                else
                {
                    protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|HTTP Response Code:{Environment.NewLine}{httpToString}", LogType.Information, LogLevel.NoLogging);
                    return false;
                }
            }

            public virtual bool CheckJSONResponseStatus(Status status, SLProtocol protocol)
            {
                if (status.ErrorCode!=0)
                {
                    protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Status error message:{Environment.NewLine}{status.ErrorMessage}", LogType.Information, LogLevel.NoLogging);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
