using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using QAction_3;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

/// <summary>
/// DataMiner QAction Class: Parse Latest Listings.
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
			string json = protocol.GetParameter(Parameter.responselatestlistings_4).ToString();
			LatestListings latestListings = SecureNewtonsoftDeserialization.DeserializeObject<LatestListings>(json);
			var statusCode = protocol.GetParameter(Parameter.statuscodelatestlistings_3).ToString();
			int status = Int32.Parse(statusCode.Split(' ')[1]);
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run| Status {status}", LogType.Error, LogLevel.NoLogging);


			if (status == 200)
			{
				FillLatestListingsTable(protocol, latestListings);
			}
			else
			{
				protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown: Status of the response is: {status}", LogType.Error, LogLevel.NoLogging);

			}
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void FillLatestListingsTable(SLProtocol protocol, LatestListings latestListings)
	{
		try
		{
			Dictionary<string, object[]> latestListingsTableContent = new Dictionary<string, object[]>();

			foreach (Listing latest_listing in latestListings.Listings)
			{
				if (!String.IsNullOrWhiteSpace(latest_listing.Id))
				{
					latestListingsTableContent[latest_listing.Id] = new LatestlistingsQActionRow
					{
						Latestlistingsid_101 = latest_listing.Id.ToString(),
						Latestlistingsname_102 = latest_listing.Name,
						Latestlistingssymbol_103 = latest_listing.Symbol,
						Latestlistingsdateadded_104 = latest_listing.DateAdded.ToOADate(),
						Latestlistingstotalsupply_105 = latest_listing.TotalSupply,
						Latestlistingsrank_106 = latest_listing.CmcRank,
						Latestlistingslastupdated_107 = latest_listing.LastUpdated.ToOADate(),
						Latestlistingsprice_108 = latest_listing.Quote.USD.Price,
						Latestlistings1hpercentagechange_109 = latest_listing.Quote.USD.PercentChange1h,
					}.ToObjectArray();
				}
				else
				{
					protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run| Latest Listing Id is null", LogType.Error, LogLevel.NoLogging);
				}

			}

			protocol.FillArray(Parameter.Latestlistings.tablePid, latestListingsTableContent.Values.ToList(), NotifyProtocol.SaveOption.Full);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}