

using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Scripting.LatestQuote;
using Skyline.DataMiner.Utils.Protocol.Extension;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;
using Skyline.Protocol.QAction_1;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

/// <summary>
/// DataMiner QAction Class: Parse Latest Quote.
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
			string json = protocol.GetParameter(Parameter.responselatestquote_10).ToString();
			LatestQuote latestQuote = SecureNewtonsoftDeserialization.DeserializeObject<LatestQuote>(json);

			if (StatusCode.CheckStatusCode(protocol, Parameter.statuscodelatestquote_9))
			{
				if (StatusCode.CheckErrorCode(protocol, latestQuote.Status.ErrorCode, latestQuote.Status.ErrorMessage))
				{
					FillLatestQuoteParameters(protocol, latestQuote.Data);
				}
			}
			/*string statusCode = protocol.GetParameter(Parameter.statuscodelatestquote_9).ToString();
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|statusCode: '{statusCode}'", LogType.Information, LogLevel.NoLogging);

			int status = Int32.Parse(statusCode.Split(' ')[1]);

			if (status == 200)
			{
				FillLatestQuoteParameters(protocol, latestQuote.Data);
			}
			else
			{
				protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown: Status of the response is: {status}", LogType.Error, LogLevel.NoLogging);
			}*/
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void FillLatestQuoteParameters(SLProtocol protocol, Data latestQuote)
	{
		Dictionary<int, object> parameters = new Dictionary<int, object>
		{
			{ Parameter.latestquoteactivecryptocurrencies_30, latestQuote.ActiveCryptocurrencies},
			{ Parameter.latestquotetotalcryptocurrencies_31, latestQuote.TotalCryptocurrencies },
			{ Parameter.latestquoteactiveexchanges_32, latestQuote.ActiveExchanges },
			{ Parameter.latestquotetotalexchanges_33, latestQuote.TotalExchanges },
			{ Parameter.latestquoteethdominance_34, latestQuote.EthDominance },
			{ Parameter.latestquotebtcdominance_35, latestQuote.BtcDominance },
			{ Parameter.latestquoteethdominance24percentagechange_36, latestQuote.EthDominance24hPercentageChange },
			{ Parameter.latestquotebtcdominance24percentagechange_37, latestQuote.BtcDominance24hPercentageChange },
			{ Parameter.latestquotedefi24hpercentagechange_38, latestQuote.Defi24hPercentageChange },
			{ Parameter.latestquotetodaychangepercent_39, latestQuote.TodayChangePercent },
			{ Parameter.latestquotelastupdate_40, latestQuote.LastUpdated.ToOADate() },
			{ Parameter.latestquotetotalmarketcap24h_41, latestQuote.Quote.USD.TotalMarketCap },
			{ Parameter.latestquotealtcoinvolume24h_42,latestQuote.Quote.USD.AltcoinVolume24h },
			{ Parameter.latestquotedefivolume24hpercentagechange_43, latestQuote.Quote.USD.Defi24hPercentageChange },
			{ Parameter.latestquotestablecoinvolume24hpercentagechange_44, latestQuote.Quote.USD.Stablecoin24hPercentageChange },
			{ Parameter.latestquotederivativesvolume24hpercentagechange_45, latestQuote.Quote.USD.Derivatives24hPercentageChange },
			{ Parameter.latestquotederivativesvolume24h_46, latestQuote.Quote.USD.DerivativesVolume24h },
			{ Parameter.latestquotedefivolume24h_47, latestQuote.Quote.USD.DefiVolume24h },
		};
		protocol.SetParameters(parameters);
	}
}