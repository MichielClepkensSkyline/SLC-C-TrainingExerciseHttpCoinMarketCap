using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Newtonsoft.Json;

using QAction_2.Dtos;

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
	public static void Run(SLProtocol protocol)
	{
		try
		{
			HandleLatestListingResponse(protocol);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void HandleLatestListingResponse(SLProtocol protocol)
	{
		var latestListingResponse = GetAndDeserializeLatestListingResponse(protocol);

		protocol.Log($"QA{protocol.QActionID}|TEST|Number of Latest Listing Table rows: {latestListingResponse.Data.Count}", LogType.Error, LogLevel.NoLogging);
	}

	private static LatestListingResponseDto GetAndDeserializeLatestListingResponse(SLProtocol protocol)
	{
		var responseString = (string)protocol.GetParameter(Parameter.latestlistingresponsecontent_210);

		if (string.IsNullOrWhiteSpace(responseString))
		{
			throw new ArgumentException("The response is not a valid string or is empty.");
		}

		var latestListingResponse = JsonConvert.DeserializeObject<LatestListingResponseDto>(responseString);

		if (latestListingResponse == null)
		{
			throw new InvalidOperationException("Failed to deserialize latest listing response.");
		}

		return latestListingResponse;
	}
}
