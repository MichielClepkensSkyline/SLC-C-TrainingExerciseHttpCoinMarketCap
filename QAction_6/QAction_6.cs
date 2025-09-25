using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class: Make The API To Call in Session 4.
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
			/*var rowPK = protocol.RowKey();
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Row: {rowPK}", LogType.Error, LogLevel.NoLogging);*/

			string api = Form(protocol);

			protocol.SetParameter(Parameter.individualcategoryapi_13, api);

			//protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|API :{protocol.GetParameter(Parameter.individualcategoryapi_13)}", LogType.Error, LogLevel.NoLogging);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static string Form(SLProtocol protocol)
	{
		var rowPK = protocol.RowKey();
		string firstPartOfApi = "api/custom/coinmarketcap?content=category&id=";

		return firstPartOfApi + rowPK.ToString();
	}
}