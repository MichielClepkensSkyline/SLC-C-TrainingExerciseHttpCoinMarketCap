using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Text;

using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.Protocol.Extension;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;
using System.Linq;
using Skyline.DataMiner.Scripting.Listing;

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
			// Check if status code is 200? (Moet ik ook andere codes chechken? Checken of het begint met een 2?
			// Beter om in de content status de check te doen?
			protocol.Log($"QA{protocol.QActionID}|Run|{protocol.GetParameter(Parameter.statuscode)}", LogType.Error, LogLevel.NoLogging);
			string statusCode = (string)protocol.GetParameter(Parameter.statuscode);
			int status = Int32.Parse(statusCode.Split(' ')[1]);
			protocol.Log($"QA{protocol.QActionID}|Run|{status}", LogType.Error, LogLevel.NoLogging);
			if (status == 200)
			{
				protocol.Log($"QA{protocol.QActionID}|Run|YES, Let's go", LogType.Error, LogLevel.NoLogging);
				Root root = SecureNewtonsoftDeserialization.DeserializeObject<Root>(protocol.GetParameter(Parameter.responsecontent).ToString());
				int counter = 0;
				foreach (Listing listing in root.Listings)
				{
					protocol.Log($"QA{protocol.QActionID}|Run|{listing.Name}", LogType.Error, LogLevel.NoLogging);
					counter++;
				}

				protocol.Log($"QA{protocol.QActionID}|Run|{counter}", LogType.Error, LogLevel.NoLogging);
				FillLastListings(protocol, root.Listings);
			}
			//Parameter.responsecontent
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void FillLastListings(SLProtocolExt protocol, List<Listing> last_listings)
	{
		Dictionary<string, LastlistingQActionRow> lastListingRows = new Dictionary<string, LastlistingQActionRow>();
		foreach (Listing listing in last_listings)
		{
			protocol.Log($"QA{protocol.QActionID}|Run|{listing.Platform == null}", LogType.Error, LogLevel.NoLogging);
			LastlistingQActionRow lastlistingQActionRow = new LastlistingQActionRow
			{
				Lastlistingid = listing.Id,
				Lastlistingname = listing.Name,
				Lastlistingsymbol = listing.Symbol,
				Lastlistingdateadded = listing.DateAdded,
				Lastlistingtotalsupply = listing.TotalSupply,
				Lastlistingplatformname = listing.Platform != null ? listing.Platform.Name : "", // Dit kan null zijn, wat dan invullen?
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
