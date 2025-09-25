using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Text;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Scripting.HTTP;
using Skyline.DataMiner.Scripting.Listing;
using Skyline.DataMiner.Utils.Protocol.Extension;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

/// <summary>
/// DataMiner QAction Class.
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
			if (StatusCode.CheckStatusCode(protocol, Parameter.statuscodelastlistings, Parameter.responsecontentlastlistings, Parameter.urllastlistings))
			{
				Root root = SecureNewtonsoftDeserialization.DeserializeObject<Root>(protocol.GetParameter(Parameter.responsecontentlastlistings).ToString());
				if (root != null)
				{
					if (root.Status == null)
					{
						protocol.Log($"QA{protocol.QActionID}|Run|root.Status is null", LogType.Error, LogLevel.NoLogging);
					}
					else if (root.Status.ErrorCode == 0)
					{
						FillLastListings(protocol, root.Listings);
					}
					else
					{
						protocol.Log($"QA{protocol.QActionID}|Run|{root.Status.ErrorMessage}", LogType.Error, LogLevel.NoLogging);
					}
				}
				else
				{
					protocol.Log($"QA{protocol.QActionID}|Run|Failed to deserialize response into object.", LogType.Error, LogLevel.NoLogging);
				}
            }
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void FillLastListings(SLProtocolExt protocol, List<Listing> lastListings)
	{
		Dictionary<string, LastlistingQActionRow> lastListingRows = new Dictionary<string, LastlistingQActionRow>();
		foreach (Listing listing in lastListings)
		{
			if (listing != null)
			{
				LastlistingQActionRow lastlistingQActionRow = new LastlistingQActionRow
				{
					Lastlistingid = listing.Id,
					Lastlistingname = listing.Name,
					Lastlistingsymbol = listing.Symbol,
					Lastlistingcoinmarketcaprank = listing.CmcRank,
					Lastlistingnummarketpairs = listing.NumMarketPairs,
					Lastlistingcirculatingsupply = listing.CirculatingSupply,
					Lastlistingtotalsupply = listing.TotalSupply,
					Lastlistingmaxsupply = listing.MaxSupply == null ? -1 : listing.MaxSupply,
					Lastlistinglastupdated = listing.LastUpdated.ToLocalTime().ToOADate(),
					Lastlistingdateadded = listing.DateAdded.ToOADate(),
					Lastlistingtotalvaluelockedratio = listing.TvlRatio == null ? 0 : listing.TvlRatio,
					Lastlistingplatformname = listing.Platform?.Name == null ? "No Platform": listing.Platform?.Name,
					Lastlistingquote = listing.Quote?.USD?.ToString(),
					Lastlistingquoteprice = listing.Quote?.USD?.Price,
					Lastlistingquotevolume24h = listing.Quote?.USD?.Volume24h,
					Lastlistingquotevolumechange24h = listing.Quote?.USD?.PercentChange24h,
					Lastlistingmarketcap = listing.Quote?.USD?.MarketCap,
					Lastlistingmarketcapdominance = listing.Quote?.USD?.MarketCapDominance,
					Lastlistingquotepercentchange1h = listing.Quote?.USD?.PercentChange1h,
					Lastlistingquotepercentchange24h = listing.Quote?.USD?.PercentChange24h,
					Lastlistingquotepercentchange7d = listing.Quote?.USD?.PercentChange7d,
				};
				lastListingRows.Add(listing.Id, lastlistingQActionRow);
			}
        }

		object[] lastListingColumns = protocol.lastlisting.QActionRowsToObjectFillArray(lastListingRows.Values.ToArray());
		protocol.FillArray(Parameter.Lastlisting.tablePid, lastListingColumns);
	}
}
