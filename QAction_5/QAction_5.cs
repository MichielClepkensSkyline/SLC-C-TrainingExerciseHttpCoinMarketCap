using QAction_5;
using Skyline.DataMiner.Core.DataMinerSystem.Protocol;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.Protocol.Extension;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;
using Skyline.Protocol.MyExtension;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// DataMiner QAction Class: Get Latest Quotes Data Into a Table.
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
            PoolLatestQuotes(protocol);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
        }
    }

    public static void PoolLatestQuotes(SLProtocol protocol)
    {
        var helper = new HelperMethods();
        var httpParameter = protocol.GetParameter(Parameter.httpresponsecodelatestquotes_500);
        bool statusResponse = helper.CheckStatusCode(httpParameter, protocol);

        if (!statusResponse)
        {
            return;
        }

        string data = Convert.ToString(protocol.GetParameter(Parameter.jsonresponselatestquotes_501));
        LatestQuotes deserializedlatestQuotes = SecureNewtonsoftDeserialization.DeserializeObject<LatestQuotes>(data);

        bool jsonStatusResponse = helper.CheckJSONResponseStatus(deserializedlatestQuotes.Status, protocol);

        if (!jsonStatusResponse)
        {
            return;
        }

        Dictionary<int, object> parameters = new Dictionary<int, object>();
        parameters[Parameter.activecryptocurrencies_503]= deserializedlatestQuotes.Data.ActiveCryptocurrencies;
        parameters[Parameter.activemarketpairs_504] =deserializedlatestQuotes.Data.ActiveMarketPairs;
        parameters[Parameter.activeexchanges_505] = deserializedlatestQuotes.Data.ActiveExchanges;
        parameters[Parameter.lastupdated_506] = deserializedlatestQuotes.Data.LastUpdated.ToOADate();
        parameters[Parameter.btcdominance_508] = deserializedlatestQuotes.Data.BtcDominance;
        parameters[Parameter.ethdominance_509] =deserializedlatestQuotes.Data.EthDominance;
        parameters[Parameter.totalmarketcap_511] = deserializedlatestQuotes.Data.Quote.USD.TotalMarketCap;
        parameters[Parameter.total24hvolume_512] = deserializedlatestQuotes.Data.Quote.USD.TotalVolume24h;
        parameters[Parameter.defimarketcap_514] = deserializedlatestQuotes.Data.Quote.USD.DefiMarketCap;
        parameters[Parameter.stablecoinmarketcap_515] =deserializedlatestQuotes.Data.Quote.USD.StablecoinMarketCap;
        protocol.SetParameters(parameters);
    }
}