using System;
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
			string bearerPrefix = "Bearer ";
			string bearerToken = bearerPrefix + token;

			protocol.SetParameter(Parameter.bearertoken_311, bearerToken);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}