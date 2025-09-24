using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAction_5
{
    public class USD
    {
        [JsonProperty("total_market_cap")]
        public float TotalMarketCap { get; set; }

        [JsonProperty("total_volume_24h")]
        public float TotalVolume24h { get; set; }

        [JsonProperty("total_volume_24h_reported")]
        public float TotalVolume24hReported { get; set; }

        [JsonProperty("altcoin_volume_24h")]
        public float AltcoinVolume24h { get; set; }

        [JsonProperty("altcoin_volume_24h_reported")]
        public float AltcoinVolume24hReported { get; set; }

        [JsonProperty("altcoin_market_cap")]
        public float AltcoinMarketCap { get; set; }

        [JsonProperty("defi_volume_24h")]
        public float DefiVolume24h { get; set; }

        [JsonProperty("defi_volume_24h_reported")]
        public float DefiVolume24hReported { get; set; }

        [JsonProperty("defi_24h_percentage_change")]
        public float Defi24hPercentageChange { get; set; }

        [JsonProperty("defi_market_cap")]
        public float DefiMarketCap { get; set; }

        [JsonProperty("stablecoin_volume_24h")]
        public float StablecoinVolume24h { get; set; }

        [JsonProperty("stablecoin_volume_24h_reported")]
        public float StablecoinVolume24hReported { get; set; }

        [JsonProperty("stablecoin_24h_percentage_change")]
        public float Stablecoin24hPercentageChange { get; set; }

        [JsonProperty("stablecoin_market_cap")]
        public float StablecoinMarketCap { get; set; }

        [JsonProperty("derivatives_volume_24h")]
        public float DerivativesVolume24h { get; set; }

        [JsonProperty("derivatives_volume_24h_reported")]
        public float DerivativesVolume24hReported { get; set; }

        [JsonProperty("derivatives_24h_percentage_change")]
        public float Derivatives24hPercentageChange { get; set; }

        [JsonProperty("total_market_cap_yesterday")]
        public float TotalMarketCapYesterday { get; set; }

        [JsonProperty("total_volume_24h_yesterday")]
        public long TotalVolume24hYesterday { get; set; }

        [JsonProperty("total_market_cap_yesterday_percentage_change")]
        public float TotalMarketCapYesterdayPercentageChange { get; set; }

        [JsonProperty("total_volume_24h_yesterday_percentage_change")]
        public float TotalVolume24hYesterdayPercentageChange { get; set; }
    }
}
