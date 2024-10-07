namespace QAction_2
{
	using System;

	using Newtonsoft.Json;

	using QAction_2.Dtos.GlobalMetricsResponse;

	using Skyline.DataMiner.Scripting;

	public static class CryptoGlobalMetricsProcessor
	{
		private const int NotAvailableNumber = -1;

		public static void HandlGlobalMetricsResponse(SLProtocolExt protocol)
		{
			var globalMetricsResponse = GetAndDeserializeGlobalMetricsResponse(protocol);
			SetGlobalMetricsParams(protocol, globalMetricsResponse);
		}

		private static GlobalMetricsResponseDto GetAndDeserializeGlobalMetricsResponse(SLProtocolExt protocol)
		{
			var responseString = (string)protocol.GetParameter(Parameter.responsecontentlatestglobalmetrics_211);

			if (string.IsNullOrWhiteSpace(responseString))
			{
				throw new ArgumentException("The response is not a valid string or is empty.");
			}

			var latestListingResponse = JsonConvert.DeserializeObject<GlobalMetricsResponseDto>(responseString);

			if (latestListingResponse == null)
			{
				throw new InvalidOperationException("Failed to deserialize latest listing response.");
			}

			return latestListingResponse;
		}

		private static void SetGlobalMetricsParams(SLProtocolExt protocol, GlobalMetricsResponseDto latestQuotesResponse)
		{
			protocol.SetParameters(
				new[]
				{
					Parameter.globalmetricslastupdate_49,
					Parameter.marketdominancebitcoin_51,
					Parameter.marketdominanceethereum_52,
					Parameter.marketdominancebitcoin24hprecentagechange_53,
					Parameter.marketdominanceethereum24hpercentagechange_54,
					Parameter.volumeandmarketcaptotalmarketcap_56,
					Parameter.volumeandmarketcaptotalvolume24h_57,
					Parameter.volumeandmarketcaptotalmarketcapyesterday_58,
					Parameter.volumeandmarketcaptotalmarketcapyesterdaypercentagechange_59,
					Parameter.volumeandmarketcapdefitradingvolume24h_60,
					Parameter.volumeandmarketcapstablecointradingvolume24h_61,
					Parameter.volumeandmarketcapdefimarketcap_62,
					Parameter.volumeandmarketcapstablecoinmarketcap_63,
				},
				new object[]
				{
					latestQuotesResponse.Data.LastUpdated,
					latestQuotesResponse.Data.BtcDominance ?? NotAvailableNumber,
					latestQuotesResponse.Data.EthDominance ?? NotAvailableNumber,
					latestQuotesResponse.Data.BtcDominance24HPercentageChange ?? NotAvailableNumber,
					latestQuotesResponse.Data.EthDominance24HPercentageChange ?? NotAvailableNumber,
					latestQuotesResponse.Data.Quote?.Usd?.TotalMarketCap ?? NotAvailableNumber,
					latestQuotesResponse.Data.Quote?.Usd.TotalVolume24H ?? NotAvailableNumber,
					latestQuotesResponse.Data.Quote?.Usd?.TotalMarketCapYesterday ?? NotAvailableNumber,
					latestQuotesResponse.Data?.Quote?.Usd?.TotalMarketCapYesterdayPercentageChange ?? NotAvailableNumber,
					latestQuotesResponse.Data.DefiVolume24H ?? NotAvailableNumber,
					latestQuotesResponse.Data.StablecoinVolume24H ?? NotAvailableNumber,
					latestQuotesResponse.Data.DefiMarketCap ?? NotAvailableNumber,
					latestQuotesResponse.Data.StablecoinMarketCap ?? NotAvailableNumber,
				});
		}

	}
}
