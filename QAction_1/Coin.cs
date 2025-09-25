using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAction_1
{
    public class Coin
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
        public string[] Tags { get; set; }

        [JsonProperty("max_supply")]
        public float? MaxSupply { get; set; }

        [JsonProperty("circulating_supply")]
        public float CirculatingSupply { get; set; }

        [JsonProperty("total_supply")]
        public float TotalSupply { get; set; }

        [JsonProperty("infinite_supply")]
        public bool InfiniteSupply { get; set; }

        [JsonProperty("platform")]
        public Platform Platform { get; set; }

        [JsonProperty("cmc_rank")]
        public int CmcRank { get; set; }

        [JsonProperty("self_reported_circulating_supply")]
        public float? SelfReportedCirculatingSupply { get; set; }

        [JsonProperty("self_reported_market_cap")]
        public float? SelfReportedMarketCap { get; set; }

        [JsonProperty("tvl_ratio")]
        public float? TvlRatio { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("quote")]
        public Quote Quote { get; set; }
    }
}
