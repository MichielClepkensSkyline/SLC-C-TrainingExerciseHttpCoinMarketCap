// Classes for mapping listing JSON data from CoinMarketCap.
namespace Skyline.DataMiner.Scripting.Listing
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class Listing
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("num_market_pairs")]
        public int NumMarketPairs { get; set; }

        [JsonProperty("date_added")]
        public DateTime DateAdded { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("max_supply")]
        public double? MaxSupply { get; set; }

        [JsonProperty("circulating_supply")]
        public double CirculatingSupply { get; set; }

        [JsonProperty("total_supply")]
        public double TotalSupply { get; set; }

        [JsonProperty("infinite_supply")]
        public bool InfiniteSupply { get; set; }

        [JsonProperty("platform")]
        public Platform Platform { get; set; }

        [JsonProperty("cmc_rank")]
        public int CmcRank { get; set; }

        [JsonProperty("self_reported_circulating_supply")]
        public double? SelfReportedCirculatingSupply { get; set; }

        [JsonProperty("self_reported_market_cap")]
        public double? SelfReportedMarketCap { get; set; }

        [JsonProperty("tvl_ratio")]
        public double? TvlRatio { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("quote")]
        public Quote Quote { get; set; }
    }

	public class Platform
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("token_address")]
        public string TokenAddress { get; set; }
    }

	public class Quote
    {
        [JsonProperty("USD")]
        public USD USD { get; set; }
    }

	public class Root
    {
        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("data")]
        public List<Listing> Listings { get; set; }
    }

	public class Status
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }

        [JsonProperty("error_message")]
        public object ErrorMessage { get; set; }

        [JsonProperty("elapsed")]
        public int Elapsed { get; set; }

        [JsonProperty("credit_count")]
        public int CreditCount { get; set; }

        [JsonProperty("notice")]
        public object Notice { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }

	public class USD
    {
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("volume_24h")]
        public double Volume24h { get; set; }

        [JsonProperty("volume_change_24h")]
        public double VolumeChange24h { get; set; }

        [JsonProperty("percent_change_1h")]
        public double PercentChange1h { get; set; }

        [JsonProperty("percent_change_24h")]
        public double PercentChange24h { get; set; }

        [JsonProperty("percent_change_7d")]
        public double PercentChange7d { get; set; }

        [JsonProperty("percent_change_30d")]
        public double PercentChange30d { get; set; }

        [JsonProperty("percent_change_60d")]
        public double PercentChange60d { get; set; }

        [JsonProperty("percent_change_90d")]
        public double PercentChange90d { get; set; }

        [JsonProperty("market_cap")]
        public double MarketCap { get; set; }

        [JsonProperty("market_cap_dominance")]
        public double MarketCapDominance { get; set; }

        [JsonProperty("fully_diluted_market_cap")]
        public double FullyDilutedMarketCap { get; set; }

        [JsonProperty("tvl")]
        public double? Tvl { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
    }
}
