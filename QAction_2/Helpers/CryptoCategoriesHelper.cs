namespace QAction_2.Helpers
{
	using System;
	using System.Linq;
	using System.Net.Http;

	using Newtonsoft.Json;

	using QAction_2.Dtos.CategoriesResponse;

	using Skyline.DataMiner.Scripting;

	using Parameter = Skyline.DataMiner.Scripting.Parameter;

	public static class CryptoCategoriesHelper
	{
		public static CategoriesResponseDto DeserializeCategoriesResponse(string categoriesResponseString, int responseStatusCode)
		{
			if (string.IsNullOrWhiteSpace(categoriesResponseString))
			{
				throw new ArgumentException("The response is not a valid string or is empty.");
			}

			var categoriesResponse = JsonConvert.DeserializeObject<CategoriesResponseDto>(categoriesResponseString);

			if (categoriesResponse == null)
			{
				throw new InvalidOperationException("Failed to deserialize latest listing response.");
			}

			if (responseStatusCode != 200)
			{
				throw new HttpRequestException($"Request failed with status code: {responseStatusCode}\n Message: {categoriesResponse.Status.ErrorMessage}");
			}

			return categoriesResponse;
		}

		public static void SetCategoriesColumns(SLProtocolExt protocol, CategoriesResponseDto categoriesResponse)
		{
			var categoriesTableRows = categoriesResponse.Data
				.Where(data => data != null)
				.Select(data => PrepareCategoriesSingleRow(data))
				.Where(categoriesSingleRow => categoriesSingleRow != null)
				.ToList<QActionTableRow>();

			protocol.coincategoriesoverview.FillArray(categoriesTableRows);
		}

		public static void HandleCategoriesSingleRowResponse(SLProtocolExt protocol)
		{
			var categoryResponse = GetAndDeserializeSingleCategoryResponse(protocol);
			SetCategoryRow(protocol, categoryResponse);
		}

		public static SingleCategoryResponseDto GetAndDeserializeSingleCategoryResponse(SLProtocolExt protocol)
		{
			var parameters = (object[])protocol.GetParameters(
				new uint[]
				{
					Parameter.statuscodecategoriesonrowrefresh_203,
					Parameter.responsecontentcategoriesonrowrefresh_213,
				});

			var responseStatusCode = ResponseStatusHelper.ParseResponseStatusCode(protocol, parameters[0] as string);
			var responseString = parameters[1] as string;

			if (string.IsNullOrWhiteSpace(responseString))
			{
				throw new ArgumentException("The response is not a valid string or is empty.");
			}

			var singleCategoryResponse = JsonConvert.DeserializeObject<SingleCategoryResponseDto>(responseString);

			if (singleCategoryResponse == null)
			{
				throw new InvalidOperationException("Failed to deserialize latest listing response.");
			}

			if (responseStatusCode != 200)
			{
				throw new HttpRequestException($"Request failed with status code: {responseStatusCode}\n Message: {singleCategoryResponse.Status.ErrorMessage}");
			}

			return singleCategoryResponse;
		}

		public static CoincategoriesoverviewQActionRow PrepareCategoriesSingleRow(CategoryDto data)
		{
			var categoriesSingleRow = new CoincategoriesoverviewQActionRow
			{
				Coincategoriesoverviewinstance_71 = data.Id,
				Coincategoriesoverviewname_72 = data.Name ?? NotAvailableValues.NotAvailableString,
				Coincategoriesoverviewnumberoftokens_73 = data.NumTokens ?? NotAvailableValues.NotAvailableNumber,
				Coincategoriesoverviewaveragepricechange_74 = data.AvgPriceChange ?? NotAvailableValues.NotAvailablePotentialNegativeValues,
				Coincategoriesoverviewmarketcap_75 = data.MarketCap ?? NotAvailableValues.NotAvailableNumber,
				Coincategoriesoverviewmarketcapchange_76 = data.MarketCapChange ?? NotAvailableValues.NotAvailablePotentialNegativeValues,
				Coincategoriesoverviewvolume_77 = data.Volume ?? NotAvailableValues.NotAvailableNumber,
				Coincategoriesoverviewvolumechange_78 = data.VolumeChange ?? NotAvailableValues.NotAvailablePotentialNegativeValues,
				Coincategoriesoverviewlastupdated_79 = data.LastUpdated.ToOADate(),
				Coincategoriesoverviewlastrefresh_80 = DateTime.UtcNow.ToOADate(),
			};

			return categoriesSingleRow;
		}

		public static void SetCategoryRow(SLProtocolExt protocol, SingleCategoryResponseDto categoryResponse)
		{
			var tableRow = new CoincategoriesoverviewQActionRow
			{
				Coincategoriesoverviewinstance_71 = categoryResponse.Data.Id,
				Coincategoriesoverviewname_72 = categoryResponse.Data.Name ?? NotAvailableValues.NotAvailableString,
				Coincategoriesoverviewnumberoftokens_73 = categoryResponse.Data?.NumTokens ?? NotAvailableValues.NotAvailableNumber,
				Coincategoriesoverviewaveragepricechange_74 = categoryResponse.Data?.AvgPriceChange ?? NotAvailableValues.NotAvailablePotentialNegativeValues,
				Coincategoriesoverviewmarketcap_75 = categoryResponse.Data?.MarketCap ?? NotAvailableValues.NotAvailableNumber,
				Coincategoriesoverviewmarketcapchange_76 = categoryResponse.Data?.MarketCapChange ?? NotAvailableValues.NotAvailablePotentialNegativeValues,
				Coincategoriesoverviewvolume_77 = categoryResponse.Data?.Volume ?? NotAvailableValues.NotAvailableNumber,
				Coincategoriesoverviewvolumechange_78 = categoryResponse.Data?.VolumeChange ?? NotAvailableValues.NotAvailablePotentialNegativeValues,
				Coincategoriesoverviewlastupdated_79 = categoryResponse.Data?.LastUpdated.ToOADate(),
				Coincategoriesoverviewlastrefresh_80 = DateTime.UtcNow.ToOADate(),
			};

			protocol.coincategoriesoverview.SetRow(tableRow);
		}
	}
}
