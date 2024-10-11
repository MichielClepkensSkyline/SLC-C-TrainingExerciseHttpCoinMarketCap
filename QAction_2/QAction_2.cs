using System;
using QAction_2;
using QAction_2.Helpers;
using QAction_2.Processors;
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

			switch (triggerPID)
			{
				case (int)DummyParameter.ExecuteQActionAfterLatestListingDataPoll_310:
					CryptoListingProcessor.HandleLatestListingResponse(protocol);
					break;

				case (int)DummyParameter.ExecuteQActionAfterGeneralDataPoll_311:
					CryptoGeneralDataProcessor.HandleGeneralDataResponse(protocol);
					break;

				case (int)DummyParameter.ExecuteQActionAfterRowRefreshDataPoll_312:
					CryptoCategoriesHelper.HandleCategoriesSingleRowResponse(protocol);
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
