namespace QAction_2.Helpers
{
	using System;
	using System.Net.Http;

	using Newtonsoft.Json;

	using QAction_2.Dtos.GlobalMetricsResponse;

	using Skyline.DataMiner.Scripting;

	public static class CryptoGlobalMetricsHelper
	{
		public static GlobalMetricsResponseDto DeserializeGlobalMetricsResponse(string globalMetricsResponseString, int responseStatusCode)
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

			if (responseStatusCode != 200)
			{
				throw new HttpRequestException($"Request failed with status code: {responseStatusCode}\n Message: {globalMetricsResponse.Status.ErrorMessage}");
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
					globalMetricsResponse.Data.BtcDominance ?? NotAvailableValues.NotAvailableNumber,
					globalMetricsResponse.Data.EthDominance ?? NotAvailableValues.NotAvailableNumber,
					globalMetricsResponse.Data.BtcDominance24HPercentageChange ?? NotAvailableValues.NotAvailablePotentialNegativeValues,
					globalMetricsResponse.Data.EthDominance24HPercentageChange ?? NotAvailableValues.NotAvailablePotentialNegativeValues,
					globalMetricsResponse.Data.Quote?.Usd?.TotalMarketCap ?? NotAvailableValues.NotAvailableNumber,
					globalMetricsResponse.Data.Quote?.Usd.TotalVolume24H ?? NotAvailableValues.NotAvailableNumber,
					globalMetricsResponse.Data.Quote?.Usd?.TotalMarketCapYesterday ?? NotAvailableValues.NotAvailableNumber,
					globalMetricsResponse.Data?.Quote?.Usd?.TotalMarketCapYesterdayPercentageChange ?? NotAvailableValues.NotAvailablePotentialNegativeValues,
					globalMetricsResponse.Data.DefiVolume24H ?? NotAvailableValues.NotAvailableNumber,
					globalMetricsResponse.Data.StablecoinVolume24H ?? NotAvailableValues.NotAvailableNumber,
					globalMetricsResponse.Data.DefiMarketCap ?? NotAvailableValues.NotAvailableNumber,
					globalMetricsResponse.Data.StablecoinMarketCap ?? NotAvailableValues.NotAvailableNumber,
				});
		}
	}
}
