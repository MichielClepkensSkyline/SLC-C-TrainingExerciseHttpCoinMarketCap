// Classes for mapping quote JSON data from CoinMarketCap.
namespace Skyline.DataMiner.Scripting.Quote
{
    using System;
    using Newtonsoft.Json;

    public class Data
    {
        [JsonProperty("active_cryptocurrencies")]
        public int? ActiveCryptocurrencies { get; set; }

        [JsonProperty("total_cryptocurrencies")]
        public int? TotalCryptocurrencies { get; set; }

        [JsonProperty("active_market_pairs")]
        public int? ActiveMarketPairs { get; set; }

        [JsonProperty("active_exchanges")]
        public int? ActiveExchanges { get; set; }

        [JsonProperty("total_exchanges")]
        public int? TotalExchanges { get; set; }

        [JsonProperty("eth_dominance")]
        public double? EthDominance { get; set; }

        [JsonProperty("btc_dominance")]
        public double? BtcDominance { get; set; }

        [JsonProperty("eth_dominance_yesterday")]
        public double EthDominanceYesterday { get; set; }

        [JsonProperty("btc_dominance_yesterday")]
        public double BtcDominanceYesterday { get; set; }

        [JsonProperty("eth_dominance_24h_percentage_change")]
        public double? EthDominance24hPercentageChange { get; set; }

        [JsonProperty("btc_dominance_24h_percentage_change")]
        public double? BtcDominance24hPercentageChange { get; set; }

        [JsonProperty("defi_volume_24h")]
        public double? DefiVolume24h { get; set; }

        [JsonProperty("defi_volume_24h_reported")]
        public double DefiVolume24hReported { get; set; }

        [JsonProperty("defi_market_cap")]
        public double? DefiMarketCap { get; set; }

        [JsonProperty("defi_24h_percentage_change")]
        public double? Defi24hPercentageChange { get; set; }

        [JsonProperty("stablecoin_volume_24h")]
        public double? StablecoinVolume24h { get; set; }

        [JsonProperty("stablecoin_volume_24h_reported")]
        public double StablecoinVolume24hReported { get; set; }

        [JsonProperty("stablecoin_market_cap")]
        public double? StablecoinMarketCap { get; set; }

        [JsonProperty("stablecoin_24h_percentage_change")]
        public double? Stablecoin24hPercentageChange { get; set; }

        [JsonProperty("derivatives_volume_24h")]
        public double DerivativesVolume24h { get; set; }

        [JsonProperty("derivatives_volume_24h_reported")]
        public double DerivativesVolume24hReported { get; set; }

        [JsonProperty("derivatives_24h_percentage_change")]
        public double Derivatives24hPercentageChange { get; set; }

        [JsonProperty("total_crypto_dex_currencies")]
        public int? TotalCryptoDexCurrencies { get; set; }

        [JsonProperty("today_incremental_crypto_number")]
        public int TodayIncrementalCryptoNumber { get; set; }

        [JsonProperty("past_24h_incremental_crypto_number")]
        public int? Past24hIncrementalCryptoNumber { get; set; }

        [JsonProperty("past_7d_incremental_crypto_number")]
        public int Past7dIncrementalCryptoNumber { get; set; }

        [JsonProperty("past_30d_incremental_crypto_number")]
        public int Past30dIncrementalCryptoNumber { get; set; }

        [JsonProperty("today_change_percent")]
        public double TodayChangePercent { get; set; }

        [JsonProperty("tracked_yearly_number")]
        public TrackedYearlyNumber TrackedYearlyNumber { get; set; }

        [JsonProperty("quote")]
        public Quote Quote { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
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
        public Data Data { get; set; }
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
    }

    public class TrackedYearlyNumber
    {
        [JsonProperty("maxIncrementalNumber")]
        public int MaxIncrementalNumber { get; set; }

        [JsonProperty("minIncrementalNumber")]
        public int MinIncrementalNumber { get; set; }

        [JsonProperty("maxIncrementalDate")]
        public DateTime MaxIncrementalDate { get; set; }

        [JsonProperty("minIncrementalDate")]
        public DateTime MinIncrementalDate { get; set; }
    }

    public class USD
    {
        [JsonProperty("total_market_cap")]
        public double TotalMarketCap { get; set; }

        [JsonProperty("total_volume_24h")]
        public double TotalVolume24h { get; set; }

        [JsonProperty("total_volume_24h_reported")]
        public double TotalVolume24hReported { get; set; }

        [JsonProperty("altcoin_volume_24h")]
        public double AltcoinVolume24h { get; set; }

        [JsonProperty("altcoin_volume_24h_reported")]
        public double AltcoinVolume24hReported { get; set; }

        [JsonProperty("altcoin_market_cap")]
        public double AltcoinMarketCap { get; set; }

        [JsonProperty("defi_volume_24h")]
        public double DefiVolume24h { get; set; }

        [JsonProperty("defi_volume_24h_reported")]
        public double DefiVolume24hReported { get; set; }

        [JsonProperty("defi_24h_percentage_change")]
        public double Defi24hPercentageChange { get; set; }

        [JsonProperty("defi_market_cap")]
        public double DefiMarketCap { get; set; }

        [JsonProperty("stablecoin_volume_24h")]
        public double StablecoinVolume24h { get; set; }

        [JsonProperty("stablecoin_volume_24h_reported")]
        public double StablecoinVolume24hReported { get; set; }

        [JsonProperty("stablecoin_24h_percentage_change")]
        public double Stablecoin24hPercentageChange { get; set; }

        [JsonProperty("stablecoin_market_cap")]
        public double StablecoinMarketCap { get; set; }

        [JsonProperty("derivatives_volume_24h")]
        public double DerivativesVolume24h { get; set; }

        [JsonProperty("derivatives_volume_24h_reported")]
        public double DerivativesVolume24hReported { get; set; }

        [JsonProperty("derivatives_24h_percentage_change")]
        public double Derivatives24hPercentageChange { get; set; }

        [JsonProperty("total_market_cap_yesterday")]
        public double TotalMarketCapYesterday { get; set; }

        [JsonProperty("total_volume_24h_yesterday")]
        public long TotalVolume24hYesterday { get; set; }

        [JsonProperty("total_market_cap_yesterday_percentage_change")]
        public double TotalMarketCapYesterdayPercentageChange { get; set; }

        [JsonProperty("total_volume_24h_yesterday_percentage_change")]
        public double TotalVolume24hYesterdayPercentageChange { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
    }
}