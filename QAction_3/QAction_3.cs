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
			if (StatusCode.CheckStatusCode(protocol, Parameter.statuscode))
			{
				Root root = SecureNewtonsoftDeserialization.DeserializeObject<Root>(protocol.GetParameter(Parameter.responsecontent).ToString());
				if (root.Status.ErrorCode == 0)
				{
					FillLastListings(protocol, root.Listings);
				}
				else
				{
					protocol.Log($"QA{protocol.QActionID}|QA3Run|{root.Status.ErrorMessage}", LogType.Error, LogLevel.NoLogging);
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
			LastlistingQActionRow lastlistingQActionRow = new LastlistingQActionRow
			{
				Lastlistingid = listing.Id,
				Lastlistingname = listing.Name,
				Lastlistingsymbol = listing.Symbol,
				Lastlistingdateadded = listing.DateAdded,
				Lastlistingtotalsupply = listing.TotalSupply,
				Lastlistingplatformname = listing.Platform != null ? listing.Platform.Name : string.Empty, // Dit kan null zijn, wat dan invullen?
				Lastlistinglastupdated = listing.LastUpdated,
				Lastlistingquote = listing.Quote.USD.ToString(),
				Lastlistingquoteprice = listing.Quote.USD.Price,
				Lastlistingquotevolume24h = listing.Quote.USD.Volume24h,
				Lastlistingpercentchange24h = listing.Quote.USD.PercentChange24h,
			};
			lastListingRows.Add(listing.Id, lastlistingQActionRow);
        }

		object[] lastListingColumns = protocol.lastlisting.QActionRowsToObjectFillArray(lastListingRows.Values.ToArray());
		protocol.FillArray(Parameter.Lastlisting.tablePid, lastListingColumns);
	}
}
