using System;
using System.Collections.Generic;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Scripting.LatestQuote;
using Skyline.DataMiner.Utils.Protocol.Extension;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;
using Skyline.Protocol.QAction_1;

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
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void FillLatestQuoteParameters(SLProtocol protocol, Data latestQuote)
	{
		var exceptionValue = -100;

		Dictionary<int, object> parameters = new Dictionary<int, object>
		{
			{ Parameter.latestquoteactivecryptocurrencies_30, latestQuote.ActiveCryptocurrencies ?? exceptionValue},
			{ Parameter.latestquotetotalcryptocurrencies_31, latestQuote.TotalCryptocurrencies ?? exceptionValue},
			{ Parameter.latestquoteactiveexchanges_32, latestQuote.ActiveExchanges ?? exceptionValue},
			{ Parameter.latestquotetotalexchanges_33, latestQuote.TotalExchanges ?? exceptionValue},
			{ Parameter.latestquoteethdominance_34, latestQuote.EthDominance ?? exceptionValue},
			{ Parameter.latestquotebtcdominance_35, latestQuote.BtcDominance ?? exceptionValue},
			{ Parameter.latestquoteethdominance24percentagechange_36, latestQuote.EthDominance24hPercentageChange ?? exceptionValue},
			{ Parameter.latestquotebtcdominance24percentagechange_37, latestQuote.BtcDominance24hPercentageChange ?? exceptionValue},
			{ Parameter.latestquotedefi24hpercentagechange_38, latestQuote.Defi24hPercentageChange ?? exceptionValue},
			{ Parameter.latestquotetodaychangepercent_39, latestQuote.TodayChangePercent ?? exceptionValue},
			{ Parameter.latestquotelastupdate_40, latestQuote.LastUpdated.ToOADate() },
			{ Parameter.latestquotetotalmarketcap24h_41, latestQuote.Quote.USD.TotalMarketCap ?? exceptionValue},
			{ Parameter.latestquotealtcoinvolume24h_42,latestQuote.Quote.USD.AltcoinVolume24h ?? exceptionValue},
			{ Parameter.latestquotedefivolume24hpercentagechange_43, latestQuote.Quote.USD.Defi24hPercentageChange ?? exceptionValue},
			{ Parameter.latestquotestablecoinvolume24hpercentagechange_44, latestQuote.Quote.USD.Stablecoin24hPercentageChange ?? exceptionValue},
			{ Parameter.latestquotederivativesvolume24hpercentagechange_45, latestQuote.Quote.USD.Derivatives24hPercentageChange ?? exceptionValue},
			{ Parameter.latestquotederivativesvolume24h_46, latestQuote.Quote.USD.DerivativesVolume24h ?? exceptionValue},
			{ Parameter.latestquotedefivolume24h_47, latestQuote.Quote.USD.DefiVolume24h ?? exceptionValue},
		};
		protocol.SetParameters(parameters);
	}
}