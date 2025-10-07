using System;
using System.Collections.Generic;
using System.Linq;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Scripting.HTTP;
using Skyline.DataMiner.Scripting.Quote;
using Skyline.DataMiner.Utils.Protocol.Extension;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

/// <summary>
/// DataMiner QAction Class.
/// </summary>
public static class QAction
{
    private const int ExceptionValue = -1;
    /// <summary>
    /// The QAction entry point.
    /// </summary>
    /// <param name="protocol">Link with SLProtocol process.</param>
    public static void Run(SLProtocolExt protocol)
	{
		try
		{
            if (StatusCode.CheckStatusCode(protocol, Parameter.statuscodelatestquotes, Parameter.responsecontentlatestquotes, Parameter.urllatestquotes))
            {
				string rootstring = Convert.ToString(protocol.GetParameter(Parameter.responsecontentlatestquotes));
				Root root = SecureNewtonsoftDeserialization.DeserializeObject<Root>(rootstring);
                if (root == null)
                {
                    protocol.Log($"QA{protocol.QActionID}|Run|root is null", LogType.Error, LogLevel.NoLogging);
                }
                else if(root.Status == null)
                {
                    protocol.Log($"QA{protocol.QActionID}|Run|root.Status is null", LogType.Error, LogLevel.NoLogging);
                }
                else if(root.Status.ErrorCode == 0)
                {
                    FillLatestQuotes(protocol, root.Data);
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

	private static void FillLatestQuotes(SLProtocol protocol, Data data)
    {
        if (!string.IsNullOrWhiteSpace(data.ToString()))
        {
            Dictionary<int, object> parameters = new Dictionary<int, object>
            {
                { Parameter.activecryptocurrencies, data.ActiveCryptocurrencies == null ? ExceptionValue : data.ActiveCryptocurrencies },
                { Parameter.totalcryptocurrencies, data.TotalCryptocurrencies == null ? ExceptionValue : data.TotalCryptocurrencies },
                { Parameter.activemarketpairs, data.ActiveMarketPairs == null ? ExceptionValue : data.ActiveMarketPairs },
                { Parameter.activeexchanges,data.ActiveExchanges == null ? ExceptionValue : data.ActiveExchanges },
                { Parameter.totalexchanges, data.TotalExchanges == null ? ExceptionValue : data.TotalExchanges },
                { Parameter.ethereumdominance, data.EthDominance == null ? ExceptionValue : data.EthDominance },
                { Parameter.bitcoindominance, data.BtcDominance == null ? ExceptionValue : data.BtcDominance },
                { Parameter.ethereumdominance24hpercentagechange, data.EthDominance24hPercentageChange == null ? ExceptionValue : data.EthDominance24hPercentageChange },
                { Parameter.bitcoindominance24hpercentagechange, data.BtcDominance24hPercentageChange == null ? ExceptionValue : data.BtcDominance24hPercentageChange },
                { Parameter.defivolume24h, data.DefiVolume24h == null ? ExceptionValue : data.DefiVolume24h },
                { Parameter.defimarketcap, data.DefiMarketCap == null ? ExceptionValue : data.DefiMarketCap },
                { Parameter.defi24hpercentagechange, data.Defi24hPercentageChange == null ? ExceptionValue : data.Defi24hPercentageChange },
                { Parameter.stablecoinvolume24h, data.StablecoinVolume24h == null ? ExceptionValue : data.StablecoinVolume24h },
                { Parameter.stablecoinmarketcap, data.StablecoinMarketCap == null ? ExceptionValue : data.StablecoinMarketCap },
                { Parameter.stablecoin24hpercentagechange, data.Stablecoin24hPercentageChange == null ? ExceptionValue : data.Stablecoin24hPercentageChange },
                { Parameter.totalcryptodexcurrencies, data.TotalCryptoDexCurrencies == null ? ExceptionValue : data.TotalCryptoDexCurrencies },
                { Parameter.past24hincrementalcryptonumber, data.Past24hIncrementalCryptoNumber == null ? ExceptionValue : data.Past24hIncrementalCryptoNumber },
                { Parameter.totalmarketcap, data.Quote?.USD?.TotalMarketCap == null ? ExceptionValue : data.Quote?.USD?.TotalMarketCap },
                { Parameter.totalvolume24h, data.Quote?.USD?.TotalVolume24h == null ? ExceptionValue : data.Quote?.USD?.TotalVolume24h },
                { Parameter.lastupdated, data.LastUpdated == null ? ExceptionValue : data.LastUpdated.ToLocalTime().ToOADate() },
            };
            uint[] array_succes = (uint[])protocol.SetParameters(parameters.Keys.ToArray(),parameters.Values.ToArray());
            if (!array_succes.All(r => r == 0))
            {
                protocol.Log($"QA{protocol.QActionID}|FillLatestQuotes|Setting the quote and general params has not succeeded", LogType.Error, LogLevel.NoLogging);
            }
        }
	}
}
