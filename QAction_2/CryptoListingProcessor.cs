namespace QAction_2
{
	using System;
	using System.Linq;
	using Newtonsoft.Json;
	using QAction_2.Dtos;
	using Skyline.DataMiner.Scripting;

	public static class CryptoListingProcessor
	{
		private const string NotAvailableString = "-1";
		private const int NotAvailableNumber = -1;
		private static DateTime notAvailableDate = DateTime.MinValue;

		public static void HandleLatestListingResponse(SLProtocolExt protocol)
		{
			var latestListingResponse = GetAndDeserializeLatestListingResponse(protocol);
			SetLatestListingTableColumns(protocol, latestListingResponse);
		}

		private static void SetLatestListingTableColumns(SLProtocolExt protocol, LatestListingResponseDto latestListingResponse)
		{
			var latestListingTableRows = latestListingResponse.Data
				.Where(coinData => coinData != null)
				.Select(coinData => PrepareLatestListingTableSingleRow(coinData))
				.Where(latestListingSingleRow => latestListingSingleRow != null)
				.ToList<QActionTableRow>();

			protocol.latestlistingsoverview.FillArray(latestListingTableRows);
		}

		private static LatestlistingsoverviewQActionRow PrepareLatestListingTableSingleRow(CoinDataDto coinData)
		{
			var latestListingSingleRow = new LatestlistingsoverviewQActionRow
			{
				Latestlistingsoverviewinstance_11 = coinData?.Id,
				Latestlistingsoverviewname_12 = coinData?.Name ?? NotAvailableString,
				Latestlistingsoverviewsymbol_13 = coinData?.Symbol ?? NotAvailableString,
				Latestlistingsoverviewnumberofmarketpairs_14 = coinData?.NumMarketPairs ?? NotAvailableNumber,
				Latestlistingsoverviewmineable_15 = (coinData?.Tags != null && coinData.Tags.Contains("mineable")) ? 1 : 0,
				Latestlistingsoverviewmaximumsupply_16 = coinData?.MaxSupply ?? NotAvailableNumber,
				Latestlistingsoverviewcirculatingsupply_17 = coinData?.CirculatingSupply ?? NotAvailableNumber,
				Latestlistingsoverviewtotalsupply_18 = coinData?.TotalSupply ?? NotAvailableNumber,
				Latestlistingsoverviewplatformname_19 = coinData?.Platform?.Name ?? "Native",
				Latestlistingsoverviewcoinmarketcaprank_20 = coinData?.CmcRank ?? NotAvailableNumber,
				Latestlistingsoverviewprice_21 = coinData?.Quote?.Usd?.Price ?? NotAvailableNumber,
				Latestlistingsoverviewtradingvolume24hours_22 = coinData?.Quote?.Usd?.Volume24H ?? NotAvailableNumber,
				Latestlistingsoverviewmarketcap_23 = coinData?.Quote?.Usd?.MarketCap ?? NotAvailableNumber,
				Latestlistingsoverviewmarketcapdominance_24 = coinData?.Quote?.Usd?.MarketCapDominance ?? NotAvailableNumber,
				Latestlistingsoverviewpricepercentagechange24hours_25 = coinData?.Quote?.Usd?.PercentChange24H ?? NotAvailableNumber,
				Latestlistingsoverviewpricepercentangechange7days_26 = coinData?.Quote?.Usd?.PercentChange7D ?? NotAvailableNumber,
				Latestlistingsoverviewlastupdate_27 = coinData?.Quote?.Usd?.LastUpdated ?? notAvailableDate,
			};

			return latestListingSingleRow;
		}

		private static LatestListingResponseDto GetAndDeserializeLatestListingResponse(SLProtocolExt protocol)
		{
			var responseString = (string)protocol.GetParameter(Parameter.latestlistingresponsecontent_210);

			if (string.IsNullOrWhiteSpace(responseString))
			{
				throw new ArgumentException("The response is not a valid string or is empty.");
			}

			var latestListingResponse = JsonConvert.DeserializeObject<LatestListingResponseDto>(responseString);

			if (latestListingResponse == null)
			{
				throw new InvalidOperationException("Failed to deserialize latest listing response.");
			}

			return latestListingResponse;
		}
	}
}
