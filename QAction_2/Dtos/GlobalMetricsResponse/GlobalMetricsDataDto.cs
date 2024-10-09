namespace QAction_2.Dtos.GlobalMetricsResponse
{
	using System;

	using Newtonsoft.Json;

	public class GlobalMetricsDataDto
	{
		[JsonProperty("active_cryptocurrencies")]
		public int ActiveCryptocurrencies { get; set; }

		[JsonProperty("total_cryptocurrencies")]
		public int TotalCryptocurrencies { get; set; }

		[JsonProperty("active_market_pairs")]
		public int ActiveMarketPairs { get; set; }

		[JsonProperty("active_exchanges")]
		public int ActiveExchanges { get; set; }

		[JsonProperty("total_exchanges")]
		public int TotalExchanges { get; set; }

		[JsonProperty("eth_dominance")]
		public double? EthDominance { get; set; }

		[JsonProperty("btc_dominance")]
		public double? BtcDominance { get; set; }

		[JsonProperty("eth_dominance_yesterday")]
		public double? EthDominanceYesterday { get; set; }

		[JsonProperty("btc_dominance_yesterday")]
		public double? BtcDominanceYesterday { get; set; }

		[JsonProperty("eth_dominance_24h_percentage_change")]
		public double? EthDominance24HPercentageChange { get; set; }

		[JsonProperty("btc_dominance_24h_percentage_change")]
		public double? BtcDominance24HPercentageChange { get; set; }

		[JsonProperty("defi_volume_24h")]
		public double? DefiVolume24H { get; set; }

		[JsonProperty("defi_volume_24h_reported")]
		public double? DefiVolume24HReported { get; set; }

		[JsonProperty("defi_market_cap")]
		public double? DefiMarketCap { get; set; }

		[JsonProperty("defi_24h_percentage_change")]
		public double? Defi24HPercentageChange { get; set; }

		[JsonProperty("stablecoin_volume_24h")]
		public double? StablecoinVolume24H { get; set; }

		[JsonProperty("stablecoin_volume_24h_reported")]
		public double? StablecoinVolume24HReported { get; set; }

		[JsonProperty("stablecoin_market_cap")]
		public double? StablecoinMarketCap { get; set; }

		[JsonProperty("stablecoin_24h_percentage_change")]
		public double? Stablecoin24HPercentageChange { get; set; }

		[JsonProperty("derivatives_volume_24h")]
		public double? DerivativesVolume24H { get; set; }

		[JsonProperty("derivatives_volume_24h_reported")]
		public double? DerivativesVolume24HReported { get; set; }

		[JsonProperty("derivatives_24h_percentage_change")]
		public double? Derivatives24HPercentageChange { get; set; }

		[JsonProperty("quote")]
		public GlobalMetricsQuoteDto Quote { get; set; }

		[JsonProperty("last_updated")]
		public DateTime LastUpdated { get; set; } = DateTime.MinValue;
	}
}
