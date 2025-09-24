using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAction_5
{
    public class Data
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
        public float EthDominance { get; set; }

        [JsonProperty("btc_dominance")]
        public float BtcDominance { get; set; }

        [JsonProperty("eth_dominance_yesterday")]
        public float EthDominanceYesterday { get; set; }

        [JsonProperty("btc_dominance_yesterday")]
        public float BtcDominanceYesterday { get; set; }

        [JsonProperty("eth_dominance_24h_percentage_change")]
        public float EthDominance24hChange { get; set; }

        [JsonProperty("btc_dominance_24h_percentage_change")]
        public float BtcDominance24hChange { get; set; }

        [JsonProperty("defi_volume_24h")]
        public float DefiVolume24h { get; set; }

        [JsonProperty("defi_volume_24h_reported")]
        public float DefiVolume24hReported { get; set; }

        [JsonProperty("defi_market_cap")]
        public float DefiMarketCap { get; set; }

        [JsonProperty("defi_24h_percentage_change")]
        public float Defi24hPercentageChange { get; set; }

        [JsonProperty("stablecoin_volume_24h")]
        public float StablecoinVolume24h { get; set; }

        [JsonProperty("stablecoin_volume_24h_reported")]
        public float StablecoinVolume24hReported { get; set; }

        [JsonProperty("stablecoin_market_cap")]
        public float StablecoinMarketCap { get; set; }

        [JsonProperty("stablecoin_24h_percentage_change")]
        public float Stablecoin24hPercentageChange { get; set; }

        [JsonProperty("derivatives_volume_24h")]
        public float DerivativesVolume24h { get; set; }

        [JsonProperty("derivatives_volume_24h_reported")]
        public float DerivativesVolume24hReported { get; set; }

        [JsonProperty("derivatives_24h_percentage_change")]
        public float Derivatives24hPercentageChange { get; set; }

        [JsonProperty("total_crypto_dex_currencies")]
        public int TotalCryptoDexCurrencies { get; set; }

        [JsonProperty("today_incremental_crypto_number")]
        public int TodayIncrementalCryptoNumber { get; set; }

        [JsonProperty("past_24h_incremental_crypto_number")]
        public int Past24hIncrementalCryptoNumber { get; set; }

        [JsonProperty("past_7d_incremental_crypto_number")]
        public int Past7dIncrementalCryptoNumber { get; set; }

        [JsonProperty("past_30d_incremental_crypto_number")]
        public int Past30dIncrementalCryptoNumber { get; set; }

        [JsonProperty("today_change_percent")]
        public float TodayChangePercent { get; set; }

        [JsonProperty("tracked_yearly_number")]
        public TrackedYearlyNumber TrackedYearlyNumber { get; set; }

        [JsonProperty("quote")]
        public Quote Quote { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
    }
}
