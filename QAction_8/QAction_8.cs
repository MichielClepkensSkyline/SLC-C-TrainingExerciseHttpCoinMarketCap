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
            var token = protocol.GetParameter(Parameter.bearertokenvalue_7).ToString();
            var key = protocol.GetParameter(Parameter.apikey_5).ToString();

            string splitKey= key.Split(' ')[0];
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{splitKey}", LogType.Information, LogLevel.NoLogging);
            string newToken = splitKey +' '+ token;
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{newToken}", LogType.Information, LogLevel.NoLogging);
            protocol.SetParameter(Parameter.apikey_5, newToken);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
        }
    }
}