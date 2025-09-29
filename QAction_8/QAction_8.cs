using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class: Set Bearer Token.
/// </summary>
public static class QAction
{
    /// <summary>
    /// The QAction entry point.
    /// </summary>
    /// <param name="protocol">Link with SLProtocol process.</param>
    public static void Run(SLProtocol protocol)
    {
        try
        {
            SetNewBearerTokenValue(protocol);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
        }
    }

    public static void SetNewBearerTokenValue(SLProtocol protocol)
    {
        var token = protocol.GetParameter(Parameter.bearertokenvalue_7).ToString();
        var key = protocol.GetParameter(Parameter.apikey_5).ToString();

        string splitKey = key.Split(' ')[0];
        string newToken = splitKey +' '+ token;

        protocol.SetParameter(Parameter.apikey_5, newToken);
    }
}