namespace QAction_2.Dtos.LatestListingResponse
{
    using System;
    using Newtonsoft.Json;

    internal class ListingUsdDto
    {
        [JsonProperty("price")]
        public double? Price { get; set; }

        [JsonProperty("volume_24h")]
        public double? Volume24H { get; set; }

        [JsonProperty("volume_change_24h")]
        public double? VolumeChange24H { get; set; }

        [JsonProperty("percent_change_1h")]
        public double? PercentChange1H { get; set; }

        [JsonProperty("percent_change_24h")]
        public double? PercentChange24H { get; set; }

        [JsonProperty("percent_change_7d")]
        public double? PercentChange7D { get; set; }

        [JsonProperty("market_cap")]
        public double? MarketCap { get; set; }

        [JsonProperty("market_cap_dominance")]
        public double? MarketCapDominance { get; set; }

        [JsonProperty("fully_diluted_market_cap")]
        public double? FullyDilutedMarketCap { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
    }
}
