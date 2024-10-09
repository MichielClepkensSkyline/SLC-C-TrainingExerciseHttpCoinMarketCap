﻿namespace QAction_2.Dtos.Shared
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class CoinDataDto
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("num_market_pairs")]
        public int? NumMarketPairs { get; set; }

        [JsonProperty("date_added")]
        public DateTime DateAdded { get; set; } = DateTime.MinValue;

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("max_supply")]
        public long? MaxSupply { get; set; }

        [JsonProperty("circulating_supply")]
        public double? CirculatingSupply { get; set; }

        [JsonProperty("total_supply")]
        public double? TotalSupply { get; set; }

        [JsonProperty("infinite_supply")]
        public bool? InfiniteSupply { get; set; }

        [JsonProperty("platform")]
        public PlatformDto Platform { get; set; }

        [JsonProperty("cmc_rank")]
        public int? CmcRank { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; } = DateTime.MinValue;

        [JsonProperty("quote")]
        public QuoteDto Quote { get; set; }
    }
}
