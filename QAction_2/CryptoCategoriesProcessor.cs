namespace QAction_2
{
	using System;
	using System.Linq;
	using Newtonsoft.Json;

	using QAction_2.Dtos.CategoriesResponse;

	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Scripting;

	using Parameter = Skyline.DataMiner.Scripting.Parameter;

	public class CryptoCategoriesProcessor
	{
		private const int NotAvailableNumber = -1;
		private const string NotAvailableString = "-1";
		private static readonly DateTime? NotAvailableDate = DateTime.MinValue;

		public static void HandleCategoriesResponse(SLProtocolExt protocol)
		{
			var categoriesResponse = GetAndDeserializeCategoriesResponse(protocol);
			SetCategoriesColumns(protocol, categoriesResponse);
		}

		public static void HandleCategoriesSingleRowResponse(SLProtocolExt protocol)
		{
			var categoryResponse = GetAndDeserializeSingleCategoryResponse(protocol);
			SetCategoryRow(protocol, categoryResponse);
		}

		private static CategoriesResponseDto GetAndDeserializeCategoriesResponse(SLProtocolExt protocol)
		{
			var responseString = (string)protocol.GetParameter(Parameter.responsecontentcategories_212);

			if (string.IsNullOrWhiteSpace(responseString))
			{
				throw new ArgumentException("The response is not a valid string or is empty.");
			}

			var categoriesResponse = JsonConvert.DeserializeObject<CategoriesResponseDto>(responseString);

			if (categoriesResponse == null)
			{
				throw new InvalidOperationException("Failed to deserialize latest listing response.");
			}

			return categoriesResponse;
		}

		private static SingleCategoryResponseDto GetAndDeserializeSingleCategoryResponse(SLProtocolExt protocol)
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

		private static void SetCategoriesColumns(SLProtocolExt protocol, CategoriesResponseDto categoriesResponse)
		{
			var categoriesTableRows = categoriesResponse.Data
				.Where(data => data != null)
				.Select(data => PrepareCategoriesSingleRow(data))
				.Where(categoriesSingleRow => categoriesSingleRow != null)
				.ToList<QActionTableRow>();

			protocol.coincategoriesoverview.FillArray(categoriesTableRows);
		}

		private static CoincategoriesoverviewQActionRow PrepareCategoriesSingleRow(CategoryDto data)
		{
			var categoriesSingleRow = new CoincategoriesoverviewQActionRow
			{
				Coincategoriesoverviewinstance_71 = data.Id,
				Coincategoriesoverviewname_72 = data.Name ?? NotAvailableString,
				Coincategoriesoverviewnumberoftokens_73 = data.NumTokens ?? NotAvailableNumber,
				Coincategoriesoverviewaveragepricechange_74 = data.AvgPriceChange ?? NotAvailableNumber,
				Coincategoriesoverviewmarketcap_75 = data.MarketCap ?? NotAvailableNumber,
				Coincategoriesoverviewmarketcapchange_76 = data.MarketCapChange ?? NotAvailableNumber,
				Coincategoriesoverviewvolume_77 = data.Volume ?? NotAvailableNumber,
				Coincategoriesoverviewvolumechange_78 = data.VolumeChange ?? NotAvailableNumber,
				Coincategoriesoverviewlastupdated_79 = Convert.ToString(data.LastUpdated ?? NotAvailableDate),
				Coincategoriesoverviewlastrefresh_80 = Convert.ToString(DateTime.UtcNow),
			};

			return categoriesSingleRow;
		}

		private static void SetCategoryRow(SLProtocolExt protocol, SingleCategoryResponseDto categoryResponse)
		{
			var tableRow = new CoincategoriesoverviewQActionRow
			{
				Coincategoriesoverviewinstance_71 = categoryResponse.Data.Id,
				Coincategoriesoverviewname_72 = categoryResponse.Data.Name ?? NotAvailableString,
				Coincategoriesoverviewnumberoftokens_73 = categoryResponse.Data?.NumTokens ?? NotAvailableNumber,
				Coincategoriesoverviewaveragepricechange_74 = categoryResponse.Data?.AvgPriceChange ?? NotAvailableNumber,
				Coincategoriesoverviewmarketcap_75 = categoryResponse.Data?.MarketCap ?? NotAvailableNumber,
				Coincategoriesoverviewmarketcapchange_76 = categoryResponse.Data?.MarketCapChange ?? NotAvailableNumber,
				Coincategoriesoverviewvolume_77 = categoryResponse.Data?.Volume ?? NotAvailableNumber,
				Coincategoriesoverviewvolumechange_78 = categoryResponse.Data?.VolumeChange ?? NotAvailableNumber,
				Coincategoriesoverviewlastupdated_79 = Convert.ToString(categoryResponse.Data?.LastUpdated ?? NotAvailableDate),
				Coincategoriesoverviewlastrefresh_80 = Convert.ToString(DateTime.UtcNow),
			};

			protocol.coincategoriesoverview.SetRow(tableRow);
		}
	}
}
