using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Skyline.DataMiner.Scripting.HTTP;
using Skyline.DataMiner.Scripting.Quote;
using Skyline.DataMiner.Scripting;
//using Skyline.DataMiner.Scripting.HTTP;
//using Skyline.DataMiner.Scripting.HTTP;
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
            if (StatusCode.CheckStatusCode(protocol, Parameter.statuscodelatestquotes))
            {
                Root root = SecureNewtonsoftDeserialization.DeserializeObject<Root>(protocol.GetParameter(Parameter.responsecontentlatestquotes).ToString());
                if (root.Status.ErrorCode == 0)
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

	private static void FillLatestQuotes(SLProtocolExt protocol, Data data)
    {
        Dictionary<int, object> parameters = new Dictionary<int, object>
        {
            { Parameter.activecryptocurrencies, data.ActiveCryptocurrencies },
            { Parameter.totalcryptocurrencies, data.TotalCryptocurrencies },
            { Parameter.activeexchanges, data.ActiveExchanges },
            { Parameter.totalexchanges, data.TotalExchanges },
            { Parameter.ethereumdominance, data.EthDominance },
            { Parameter.bitcoindominance, data.BtcDominance },
            { Parameter.lastupdated, data.LastUpdated },
        };
        protocol.SetParameters(parameters);
	}
}
