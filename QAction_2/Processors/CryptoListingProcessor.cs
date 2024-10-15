namespace QAction_2.Processors
{
	using System;
	using System.Linq;
	using System.Net.Http;

	using Newtonsoft.Json;

	using QAction_2.Dtos.LatestListingResponse;
	using QAction_2.Dtos.Shared;
	using QAction_2.Enums;
	using QAction_2.Helpers;

	using Skyline.DataMiner.Scripting;

	public static class CryptoListingProcessor
	{
		public static void HandleLatestListingResponse(SLProtocolExt protocol)
		{
			var latestListingResponse = GetAndDeserializeLatestListingResponse(protocol);
			SetLatestListingTableColumns(protocol, latestListingResponse);
		}

		private static void SetLatestListingTableColumns(SLProtocolExt protocol, ListingResponseDto latestListingResponse)
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
				Latestlistingsoverviewinstance_11 = coinData.Id,
				Latestlistingsoverviewname_12 = coinData.Name ?? NotAvailableValues.NotAvailableString,
				Latestlistingsoverviewsymbol_13 = coinData.Symbol ?? NotAvailableValues.NotAvailableString,
				Latestlistingsoverviewnumberofmarketpairs_14 = coinData.NumMarketPairs ?? NotAvailableValues.NotAvailableNumber,
				Latestlistingsoverviewmineability_15 = coinData.Tags != null && coinData.Tags.Contains("mineable") ? (int)Mineability.Mineable : (int)Mineability.Unmineable,
				Latestlistingsoverviewmaximumsupply_16 = coinData.MaxSupply ?? NotAvailableValues.NotAvailableNumber,
				Latestlistingsoverviewcirculatingsupply_17 = coinData.CirculatingSupply ?? NotAvailableValues.NotAvailableNumber,
				Latestlistingsoverviewtotalsupply_18 = coinData.TotalSupply ?? NotAvailableValues.NotAvailableNumber,
				Latestlistingsoverviewplatformname_19 = coinData.Platform?.Name ?? "Native",
				Latestlistingsoverviewcoinmarketcaprank_20 = coinData.CmcRank ?? NotAvailableValues.NotAvailableNumber,
				Latestlistingsoverviewprice_21 = coinData.Quote?.Usd?.Price ?? NotAvailableValues.NotAvailableNumber,
				Latestlistingsoverviewtradingvolume24hours_22 = coinData.Quote?.Usd?.Volume24H ?? NotAvailableValues.NotAvailableNumber,
				Latestlistingsoverviewmarketcap_23 = coinData.Quote?.Usd?.MarketCap ?? NotAvailableValues.NotAvailableNumber,
				Latestlistingsoverviewmarketcapdominance_24 = coinData.Quote?.Usd?.MarketCapDominance ?? NotAvailableValues.NotAvailablePotentialNegativeValues,
				Latestlistingsoverviewpricepercentagechange24hours_25 = coinData.Quote?.Usd?.PercentChange24H ?? NotAvailableValues.NotAvailablePotentialNegativeValues,
				Latestlistingsoverviewpricepercentangechange7days_26 = coinData.Quote?.Usd?.PercentChange7D ?? NotAvailableValues.NotAvailablePotentialNegativeValues,
				Latestlistingsoverviewlastupdate_27 = coinData.Quote?.Usd?.LastUpdated.ToOADate(),
			};

			return latestListingSingleRow;
		}

		private static ListingResponseDto GetAndDeserializeLatestListingResponse(SLProtocolExt protocol)
		{
			var parameters = (object[])protocol.GetParameters(
				new uint[]
				{
					Parameter.statuscodelatestlisting_200,
					Parameter.responsecontentlatestlisting_210,
				});

			var responseStatusCode = ResponseStatusHelper.ParseResponseStatusCode(protocol, parameters[0] as string);
			var responseString = parameters[1] as string;

			if (string.IsNullOrWhiteSpace(responseString))
			{
				throw new ArgumentException("The response is not a valid string or is empty.");
			}

			var latestListingResponse = JsonConvert.DeserializeObject<ListingResponseDto>(responseString);

			if (latestListingResponse == null)
			{
				throw new InvalidOperationException("Failed to deserialize latest listing response.");
			}

			if (responseStatusCode != 200)
			{
				throw new HttpRequestException($"Request failed with status code: {responseStatusCode}\n Message: {latestListingResponse.Status.ErrorMessage}");
			}

			return latestListingResponse;
		}
	}
}
