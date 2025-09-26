using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.Protocol.Extension;

/// <summary>
/// DataMiner QAction Class.
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
			string categoryId = protocol.RowKey();
			string url = $"api/custom/coinmarketcap?content=category&id={categoryId}";
			int succes = protocol.SetParameter(Parameter.urlonecategory, url);
			if (succes!=0)
            {
                protocol.Log($"QA{protocol.QActionID}|Run|The one category url set has not succeeded", LogType.Error, LogLevel.NoLogging);
            }
        }
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}
