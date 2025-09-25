using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAction_1
{
    public class Category
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
        public int NumTokens { get; set; }

        [JsonProperty("avg_price_change")]
        public double AvgPriceChange { get; set; }

        [JsonProperty("market_cap")]
        public double MarketCap { get; set; }

        [JsonProperty("market_cap_change")]
        public double MarketCapChange { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("volume_change")]
        public double VolumeChange { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated
        {
            get; set;
        }
    }
}
