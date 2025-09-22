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
			//Check if status code is 200?
			protocol.Log($"QA{protocol.QActionID}|Run|{protocol.GetParameter(Parameter.statuscode)}", LogType.Error, LogLevel.NoLogging);
			string statusCode = (string)protocol.GetParameter(Parameter.statuscode);
			int status = Int32.Parse(statusCode.Split(' ')[1]);
			protocol.Log($"QA{protocol.QActionID}|Run|{status}", LogType.Error, LogLevel.NoLogging);
			if (status == 200)
			{
				protocol.Log($"QA{protocol.QActionID}|Run|YES, Let's go", LogType.Error, LogLevel.NoLogging);

			}
			//Parameter.responsecontent
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}
