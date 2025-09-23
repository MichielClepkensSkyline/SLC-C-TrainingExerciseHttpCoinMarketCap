using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAction_3
{
    public class USD
    {
        [JsonProperty("price")]
        public float Price { get; set; }

        [JsonProperty("volume_24h")]
        public float Volume24h { get; set; }

        [JsonProperty("volume_change_24h")]
        public float VolumeChange24h { get; set; }

        [JsonProperty("percent_change_1h")]
        public float PercentChange1h { get; set; }

        [JsonProperty("percent_change_24h")]
        public float PercentChange24h { get; set; }

        [JsonProperty("percent_change_7d")]
        public float PercentChange7d { get; set; }

        [JsonProperty("percent_change_30d")]
        public float PercentChange30d { get; set; }

        [JsonProperty("percent_change_60d")]
        public float PercentChange60d { get; set; }

        [JsonProperty("percent_change_90d")]
        public float PercentChange90d { get; set; }

        [JsonProperty("market_cap")]
        public float MarketCap { get; set; }

        [JsonProperty("market_cap_dominance")]
        public float MarketCapDominance { get; set; }

        [JsonProperty("fully_diluted_market_cap")]
        public float FullyDilutedMarketCap { get; set; }

        [JsonProperty("tvl")]
        public float? Tvl { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
    }
}
