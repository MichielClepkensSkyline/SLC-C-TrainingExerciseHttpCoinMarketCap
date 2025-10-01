using System;
using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class: Make The Url For Individual Category.
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
			string rowPK = protocol.RowKey();

			string api = FormUrl(rowPK);

			protocol.SetParameter(Parameter.individualcategoryapi_11, api);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	public static string FormUrl(string rowPK)
	{
		string firstPartOfUrl = "api/custom/coinmarketcap?content=category&id=";

		string completeUrl = firstPartOfUrl + rowPK;

		return completeUrl;
	}
}