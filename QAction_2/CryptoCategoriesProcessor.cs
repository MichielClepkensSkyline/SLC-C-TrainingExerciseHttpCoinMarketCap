namespace QAction_2
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Newtonsoft.Json;

	using QAction_2.Dtos.CategoriesResponse;
	using QAction_2.Dtos.LatestListingResponse;

	using Skyline.DataMiner.Scripting;

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
				Coincategoriesoverviewlastupdated_79 = data.LastUpdated ?? NotAvailableDate,
				Coincategoriesoverviewlastrefresh_80 = DateTime.UtcNow,
			};

			return categoriesSingleRow;
		}
	}
}
