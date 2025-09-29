using Skyline.DataMiner.Scripting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

/// <summary>
/// DataMiner QAction Class: Refresh Category by Id.
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
            SetGetCategoryURL(protocol);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
        }
    }

    public static void SetGetCategoryURL(SLProtocol protocol)
    {
        string url = "api/custom/coinmarketcap?content=category&id=" + protocol.RowKey().ToString();
        protocol.SetParameter(Parameter.refreshcategoryurl_516, url);
    }
}