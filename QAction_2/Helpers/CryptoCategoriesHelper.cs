namespace QAction_2.Helpers
{
	using System;
	using System.Linq;

	using Newtonsoft.Json;

	using QAction_2.Dtos.CategoriesResponse;

	using Skyline.DataMiner.Scripting;

	using Parameter = Skyline.DataMiner.Scripting.Parameter;

	public class CryptoCategoriesHelper
	{
		private const int NotAvailableNumber = -1;
		private const string NotAvailableString = "-1";
		private const int NotAvailablePotentialNegativeValues = int.MinValue;

		// Methods for Entire Table Operations
		public static CategoriesResponseDto DeserializeCategoriesResponse(string categoriesResponseString)
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

		// Methods for Single Row Operations
		public static SingleCategoryResponseDto GetAndDeserializeSingleCategoryResponse(SLProtocolExt protocol)
		{
			var responseString = (string)protocol.GetParameter(Parameter.responsecontentcategoriesonrowrefresh_213);

			if (string.IsNullOrWhiteSpace(responseString))
			{
				throw new ArgumentException("The response is not a valid string or is empty.");
			}

			var singleCategoryResponse = JsonConvert.DeserializeObject<SingleCategoryResponseDto>(responseString);

			if (singleCategoryResponse == null)
			{
				throw new InvalidOperationException("Failed to deserialize latest listing response.");
			}

			return singleCategoryResponse;
		}

		public static CoincategoriesoverviewQActionRow PrepareCategoriesSingleRow(CategoryDto data)
		{
			var categoriesSingleRow = new CoincategoriesoverviewQActionRow
			{
				Coincategoriesoverviewinstance_71 = data.Id,
				Coincategoriesoverviewname_72 = data.Name ?? NotAvailableString,
				Coincategoriesoverviewnumberoftokens_73 = data.NumTokens ?? NotAvailableNumber,
				Coincategoriesoverviewaveragepricechange_74 = data.AvgPriceChange ?? NotAvailablePotentialNegativeValues,
				Coincategoriesoverviewmarketcap_75 = data.MarketCap ?? NotAvailableNumber,
				Coincategoriesoverviewmarketcapchange_76 = data.MarketCapChange ?? NotAvailablePotentialNegativeValues,
				Coincategoriesoverviewvolume_77 = data.Volume ?? NotAvailableNumber,
				Coincategoriesoverviewvolumechange_78 = data.VolumeChange ?? NotAvailablePotentialNegativeValues,
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
				Coincategoriesoverviewname_72 = categoryResponse.Data.Name ?? NotAvailableString,
				Coincategoriesoverviewnumberoftokens_73 = categoryResponse.Data?.NumTokens ?? NotAvailableNumber,
				Coincategoriesoverviewaveragepricechange_74 = categoryResponse.Data?.AvgPriceChange ?? NotAvailablePotentialNegativeValues,
				Coincategoriesoverviewmarketcap_75 = categoryResponse.Data?.MarketCap ?? NotAvailableNumber,
				Coincategoriesoverviewmarketcapchange_76 = categoryResponse.Data?.MarketCapChange ?? NotAvailablePotentialNegativeValues,
				Coincategoriesoverviewvolume_77 = categoryResponse.Data?.Volume ?? NotAvailableNumber,
				Coincategoriesoverviewvolumechange_78 = categoryResponse.Data?.VolumeChange ?? NotAvailablePotentialNegativeValues,
				Coincategoriesoverviewlastupdated_79 = categoryResponse.Data?.LastUpdated.ToOADate(),
				Coincategoriesoverviewlastrefresh_80 = DateTime.UtcNow.ToOADate(),
			};

			protocol.coincategoriesoverview.SetRow(tableRow);
		}
	}
}
