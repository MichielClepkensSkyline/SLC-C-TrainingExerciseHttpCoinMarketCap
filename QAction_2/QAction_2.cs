using System;
using QAction_2;
using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class: After Startup.
/// </summary>
public static class QAction
{
	/// <summary>
	/// The QAction entry point.
	/// </summary>
	/// <param name="protocol">Link with SLProtocol process.</param>
	public static void Run(SLProtocolExt protocol)
	{
		try
		{
			int triggerPID = protocol.GetTriggerParameter();

			protocol.Log($"QA{protocol.QActionID}|TRIGGER|{triggerPID}", LogType.Error, LogLevel.NoLogging);

			switch (triggerPID)
			{
				case Parameter.responsecontentlatestlisting_210:
					CryptoListingProcessor.HandleLatestListingResponse(protocol);
					break;

				case Parameter.responsecontentlatestglobalmetrics_211:
					CryptoGlobalMetricsProcessor.HandlGlobalMetricsResponse(protocol);
					break;

				case Parameter.responsecontentcategories_212:
					CryptoCategoriesProcessor.HandleCategoriesResponse(protocol);
					break;

				default:
					protocol.Log($"Unhandled trigger parameter: {triggerPID}", LogType.Error, LogLevel.NoLogging);
					break;
			}
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}
