using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class: Make a Bearer token.
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
			string token = protocol.GetParameter(Parameter.acceskeytoken_312).ToString();
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Access key: {token}", LogType.Error, LogLevel.NoLogging);

			string bearerToken = "Bearer " + token;
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|BEarer key: {bearerToken}", LogType.Error, LogLevel.NoLogging);
			protocol.SetParameter(Parameter.bearetoken_311, bearerToken);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}