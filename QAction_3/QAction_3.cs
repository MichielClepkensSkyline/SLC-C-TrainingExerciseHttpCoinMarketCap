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
			if (listing != null && listing.Id != null)
			{
				LastlistingQActionRow lastlistingQActionRow = new LastlistingQActionRow
				{
					Lastlistingid = listing.Id,
					Lastlistingname = listing.Name == null ? string.Empty : listing.Name,
					Lastlistingsymbol = listing.Symbol == null ? string.Empty : listing.Symbol,
					Lastlistingcoinmarketcaprank = listing.CmcRank == null ? -1 : listing.CmcRank,
					Lastlistingnummarketpairs = listing.NumMarketPairs == null ? -1 : listing.NumMarketPairs,
					Lastlistingcirculatingsupply = listing.CirculatingSupply == null ? -1 : listing.CirculatingSupply,
					Lastlistingtotalsupply = listing.TotalSupply == null ? -1 : listing.TotalSupply,
					Lastlistingmaxsupply = listing.MaxSupply == null ? -1 : listing.MaxSupply,
					Lastlistinglastupdated = listing.LastUpdated == null ? -1 : listing.LastUpdated.ToLocalTime().ToOADate(),
					Lastlistingdateadded = listing.DateAdded == null ? -1 : listing.DateAdded.ToOADate(),
					Lastlistingtotalvaluelockedratio = listing.TvlRatio == null ? -1 : listing.TvlRatio,
					Lastlistingplatformname = listing.Platform?.Name == null ? "no platform" : listing.Platform?.Name,
					Lastlistingquote = listing.Quote?.USD == null ?string.Empty : listing.Quote?.USD.ToString().Split('.').Last(),
					Lastlistingquoteprice = listing.Quote?.USD?.Price == null ? -1 : listing.Quote?.USD?.Price,
					Lastlistingquotevolume24h = listing.Quote?.USD?.Volume24h == null ? -1 : listing.Quote?.USD?.Volume24h,
					Lastlistingquotevolumechange24h = listing.Quote?.USD?.PercentChange24h == null ? -1 : listing.Quote?.USD?.PercentChange24h,
					Lastlistingmarketcap = listing.Quote?.USD?.MarketCap == null ? -1 : listing.Quote?.USD?.MarketCap,
					Lastlistingmarketcapdominance = listing.Quote?.USD?.MarketCapDominance == null ? -1 : listing.Quote?.USD?.MarketCapDominance,
					Lastlistingquotepercentchange1h = listing.Quote?.USD?.PercentChange1h == null ? -1 : listing.Quote?.USD?.PercentChange1h,
					Lastlistingquotepercentchange24h = listing.Quote?.USD?.PercentChange24h == null ? -1 : listing.Quote?.USD?.PercentChange24h,
					Lastlistingquotepercentchange7d = listing.Quote?.USD?.PercentChange7d == null ? -1 : listing.Quote?.USD?.PercentChange7d,
				};
				lastListingRows.Add(listing.Id, lastlistingQActionRow);
			}
        }

		object[] lastListingColumns = protocol.lastlisting.QActionRowsToObjectFillArray(lastListingRows.Values.ToArray());
		bool succes = (bool)protocol.FillArray(Parameter.Lastlisting.tablePid, lastListingColumns);

		if (! succes)
		{
			protocol.Log($"QA{protocol.QActionID}|FillLastListings|The listings table set has not succeeded", LogType.Error, LogLevel.NoLogging);
		}
	}
}
