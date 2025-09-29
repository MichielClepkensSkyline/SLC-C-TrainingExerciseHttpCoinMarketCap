using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using QAction_1;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;
using Skyline.Protocol.MyExtension;

/// <summary>
/// DataMiner QAction Class: Poll Listing data into a table.
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
            PollLatestListings(protocol);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
        }
    }

    public static void PollLatestListings(SLProtocol protocol)
    {
        var helper = new HelperMethods();
        var httpParameter = protocol.GetParameter(Parameter.httpresponsecodelatestlistings_3);
        bool statusResponse = helper.CheckStatusCode(httpParameter, protocol);

        if (!statusResponse)
        {
            return;
        }

        string data = Convert.ToString(protocol.GetParameter(Parameter.jsonresponselatestlistings_4));
        LatestListings deserializedlatestListings = SecureNewtonsoftDeserialization.DeserializeObject<LatestListings>(data);

        bool jsonStatusResponse = helper.CheckJSONResponseStatus(deserializedlatestListings.Status, protocol);

        if (!jsonStatusResponse)
        {
            return;
        }

        Dictionary<string, object[]> latestListingsDictionary = new Dictionary<string, object[]>();
        foreach (Coin latestListings in deserializedlatestListings.Data)
        {
            if (String.IsNullOrWhiteSpace(latestListings.Id))
            {
                protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|No primary key found for Latest listings{Environment.NewLine}", LogType.Information, LogLevel.NoLogging);
            }

            latestListingsDictionary[latestListings.Id] = new LatestlistingsQActionRow
            {
                Latestlistingsid_101 = latestListings.Id,
                Latestlistingsname_102 = latestListings.Name,
                Latestlistingssymbol_103 = latestListings.Symbol,
                Latestlistingscoinmarketcaprank_104=latestListings.CmcRank,
                Latestlistingscirculatingsupply_105 = latestListings.CirculatingSupply,
                Latestlistingsusdprice_106 =latestListings.Quote.USD.Price,
                Latestlistingsmarketcap_107 =latestListings.Quote.USD.MarketCap,
                Latestlistings1hourchange_108= latestListings.Quote.USD.PercentChange1h,
                Latestlistings24hourvolumechange_109 = latestListings.Quote.USD.VolumeChange24h,
                Latestlistingslastupdated_110 = latestListings.LastUpdated.ToOADate(),
            }.ToObjectArray();
            protocol.FillArray(Parameter.Latestlistings.tablePid, latestListingsDictionary.Values.ToList(), NotifyProtocol.SaveOption.Full);
        }
    }
}