namespace QAction_2.Helpers
{
    using System;

    using Newtonsoft.Json;

    using QAction_2.Dtos.GlobalMetricsResponse;

    using Skyline.DataMiner.Scripting;

    public static class CryptoGlobalMetricsHelper
    {
        private const int NotAvailableNumber = -1;
        private const int NotAvailablePotentialNegativeValues = int.MinValue;

        public static GlobalMetricsResponseDto GetAndDeserializeGlobalMetricsResponse(string globalMetricsResponseString)
        {
            if (string.IsNullOrWhiteSpace(globalMetricsResponseString))
            {
                throw new ArgumentException("The response is not a valid string or is empty.");
            }

            var globalMetricsResponse = JsonConvert.DeserializeObject<GlobalMetricsResponseDto>(globalMetricsResponseString);

            if (globalMetricsResponse == null)
            {
                throw new InvalidOperationException("Failed to deserialize latest listing response.");
            }

            return globalMetricsResponse;
        }

        public static void SetGlobalMetricsParams(SLProtocolExt protocol, GlobalMetricsResponseDto globalMetricsResponse)
        {
            protocol.SetParameters(
                new[]
                {
                    Parameter.globalmetricslastupdate_49,
                    Parameter.marketdominancebitcoin_51,
                    Parameter.marketdominanceethereum_52,
                    Parameter.marketdominancebitcoin24hprecentagechange_53,
                    Parameter.marketdominanceethereum24hpercentagechange_54,
                    Parameter.totalmarketcap_56,
                    Parameter.totalvolume24h_57,
                    Parameter.totalmarketcapyesterday_58,
                    Parameter.totalmarketcapyesterdaypercentagechange_59,
                    Parameter.decentralizedfinancetradingvolume24h_60,
                    Parameter.volumeandmarketcapstablecointradingvolume24h_61,
                    Parameter.decentralizedfinancemarketcap_62,
                    Parameter.stablecoinmarketcap_63,
                },
                new object[]
                {
                    globalMetricsResponse.Data.LastUpdated,
                    globalMetricsResponse.Data.BtcDominance ?? NotAvailableNumber,
                    globalMetricsResponse.Data.EthDominance ?? NotAvailableNumber,
                    globalMetricsResponse.Data.BtcDominance24HPercentageChange ?? NotAvailablePotentialNegativeValues,
                    globalMetricsResponse.Data.EthDominance24HPercentageChange ?? NotAvailablePotentialNegativeValues,
                    globalMetricsResponse.Data.Quote?.Usd?.TotalMarketCap ?? NotAvailableNumber,
                    globalMetricsResponse.Data.Quote?.Usd.TotalVolume24H ?? NotAvailableNumber,
                    globalMetricsResponse.Data.Quote?.Usd?.TotalMarketCapYesterday ?? NotAvailableNumber,
                    globalMetricsResponse.Data?.Quote?.Usd?.TotalMarketCapYesterdayPercentageChange ?? NotAvailablePotentialNegativeValues,
                    globalMetricsResponse.Data.DefiVolume24H ?? NotAvailableNumber,
                    globalMetricsResponse.Data.StablecoinVolume24H ?? NotAvailableNumber,
                    globalMetricsResponse.Data.DefiMarketCap ?? NotAvailableNumber,
                    globalMetricsResponse.Data.StablecoinMarketCap ?? NotAvailableNumber,
                });
        }
    }
}
