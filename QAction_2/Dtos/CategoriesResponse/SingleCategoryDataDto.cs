namespace QAction_2.Dtos.CategoriesResponse
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using QAction_2.Dtos.Shared;

	public class SingleCategoryDataDto
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("num_tokens")]
		public int? NumTokens { get; set; }

		[JsonProperty("last_updated")]
		public DateTime LastUpdated { get; set; }

		[JsonProperty("avg_price_change")]
		public double? AvgPriceChange { get; set; }

		[JsonProperty("market_cap")]
		public double? MarketCap { get; set; }

		[JsonProperty("market_cap_change")]
		public double? MarketCapChange { get; set; }

		[JsonProperty("volume")]
		public decimal? Volume { get; set; }

		[JsonProperty("volume_change")]
		public double? VolumeChange { get; set; }

		[JsonProperty("coins")]
		public List<CoinDataDto> Coins { get; set; }
	}
}
