namespace Skyline.Protocol
{
    using Skyline.DataMiner.Net.Messages;
    using Skyline.DataMiner.Net.Messages.SLDataGateway;
    using Skyline.DataMiner.Scripting;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    namespace MyExtension
    {
        public class MyMethods
        {
            public bool CheckStatusCode(object httpResponseParameter,SLProtocol protocol)
            {
                protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Recived parameter:{Environment.NewLine}{httpResponseParameter}", LogType.Information, LogLevel.NoLogging);
                string httpToString =httpResponseParameter.ToString();
                protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|parameter to string:{Environment.NewLine}{httpToString}", LogType.Information, LogLevel.NoLogging);
                string statusCodeString = httpToString.Split(' ')[1];
                protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Split string:{Environment.NewLine}{statusCodeString}", LogType.Information, LogLevel.NoLogging);
                int statusCode = int.Parse(statusCodeString);
                protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Parsed string:{Environment.NewLine}{statusCode}", LogType.Information, LogLevel.NoLogging);

                if (statusCode == 200)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
