namespace QAction_2.Dtos.GlobalMetricsResponse
{
	using System;
	using Newtonsoft.Json;

	public class GlobalMetricsUsdDto
	{
		[JsonProperty("total_market_cap")]
		public double? TotalMarketCap { get; set; }

		[JsonProperty("total_volume_24h")]
		public double? TotalVolume24H { get; set; }

		[JsonProperty("total_volume_24h_reported")]
		public double? TotalVolume24HReported { get; set; }

		[JsonProperty("altcoin_volume_24h")]
		public double? AltcoinVolume24H { get; set; }

		[JsonProperty("altcoin_volume_24h_reported")]
		public double? AltcoinVolume24HReported { get; set; }

		[JsonProperty("altcoin_market_cap")]
		public double? AltcoinMarketCap { get; set; }

		[JsonProperty("defi_volume_24h")]
		public double? DefiVolume24H { get; set; }

		[JsonProperty("defi_volume_24h_reported")]
		public double? DefiVolume24HReported { get; set; }

		[JsonProperty("defi_24h_percentage_change")]
		public double? Defi24HPercentageChange { get; set; }

		[JsonProperty("defi_market_cap")]
		public double? DefiMarketCap { get; set; }

		[JsonProperty("stablecoin_volume_24h")]
		public double? StablecoinVolume24H { get; set; }

		[JsonProperty("stablecoin_volume_24h_reported")]
		public double? StablecoinVolume24HReported { get; set; }

		[JsonProperty("stablecoin_24h_percentage_change")]
		public double? Stablecoin24HPercentageChange { get; set; }

		[JsonProperty("stablecoin_market_cap")]
		public double? StablecoinMarketCap { get; set; }

		[JsonProperty("derivatives_volume_24h")]
		public double? DerivativesVolume24H { get; set; }

		[JsonProperty("derivatives_volume_24h_reported")]
		public double? DerivativesVolume24HReported { get; set; }

		[JsonProperty("derivatives_24h_percentage_change")]
		public double? Derivatives24HPercentageChange { get; set; }

		[JsonProperty("total_market_cap_yesterday")]
		public double? TotalMarketCapYesterday { get; set; }

		[JsonProperty("total_volume_24h_yesterday")]
		public double? TotalVolume24HYesterday { get; set; }

		[JsonProperty("total_market_cap_yesterday_percentage_change")]
		public double? TotalMarketCapYesterdayPercentageChange { get; set; }

		[JsonProperty("total_volume_24h_yesterday_percentage_change")]
		public double? TotalVolume24HYesterdayPercentageChange { get; set; }

		[JsonProperty("last_updated")]
		public DateTime LastUpdated { get; set; } = DateTime.MinValue;
	}
}
