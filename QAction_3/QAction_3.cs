using System;
using System.Collections.Generic;
using System.Linq;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Scripting.LatestListings;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;
using Skyline.Protocol.QAction_1;

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
			if (!StatusCode.CheckStatusCode(protocol, Parameter.statuscodelatestlistings_3))
			{
				return;
			}

			string json = protocol.GetParameter(Parameter.responselatestlistings_4).ToString();
			LatestListings latestListings = SecureNewtonsoftDeserialization.DeserializeObject<LatestListings>(json);

			if(latestListings == null)
			{
				protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run| Latest listing is null", LogType.Error, LogLevel.NoLogging);
				return;
			}

			if (!StatusCode.CheckErrorCode(protocol, latestListings.Status.ErrorCode, latestListings.Status.ErrorMessage))
			{
				return;
			}

			FillLatestListingsTable(protocol, latestListings);
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

			int exceptionValue = -100;

			foreach (Listing latest_listing in latestListings.Listings)
			{
				if (String.IsNullOrWhiteSpace(latest_listing.Id))
				{
					protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run| Latest Listing Id is null", LogType.Error, LogLevel.NoLogging);
					return;
				}

				latestListingsTableContent[latest_listing.Id] = new LatestlistingsQActionRow
				{
					Latestlistingsid_101 = latest_listing.Id.ToString(),
					Latestlistingsname_102 = latest_listing.Name ?? exceptionValue.ToString(),
					Latestlistingssymbol_103 = latest_listing.Symbol ?? exceptionValue.ToString(),
					Latestlistingsdateadded_104 = latest_listing.DateAdded.ToLocalTime().ToOADate(),
					Latestlistingscirculatingsupply_105 = latest_listing.TotalSupply,
					Latestlistingsrank_106 = latest_listing.CmcRank ?? exceptionValue,
					Latestlistingslastupdated_107 = latest_listing.LastUpdated.ToLocalTime().ToOADate(),
					Latestlistingsquoteprice_108 = latest_listing.Quote.USD.Price ?? exceptionValue,
					Latestlistings1hpercentagechange_109 = latest_listing.Quote.USD.PercentChange1h ?? exceptionValue,
					Latestlistingsquotevolumechange24h_110 = latest_listing.Quote.USD.VolumeChange24h ?? exceptionValue,
					Latestlistingsquotemarketcap_111 = latest_listing.Quote.USD.MarketCap ?? exceptionValue,
					Latestlistingsplatformname_112 = latest_listing.Platform?.Name ?? exceptionValue.ToString(),
					Latestlistingsmaxsupply_114 = latest_listing.MaxSupply ?? exceptionValue,
					Latestlistingsquotemarketcapdominance_115 = latest_listing.Quote.USD.MarketCapDominance ?? exceptionValue,
					Latestlistingsquotevolume24h_116 = latest_listing.Quote.USD.Volume24h ?? exceptionValue,
				}.ToObjectArray();
			}

			protocol.FillArray(Parameter.Latestlistings.tablePid, latestListingsTableContent.Values.ToList(), NotifyProtocol.SaveOption.Full);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}