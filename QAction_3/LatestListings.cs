namespace QAction_3
{
    using System;
    using Newtonsoft.Json;

    public class LatestListings
    {
        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("data")]
        public Coin[] Data { get; set; }
    }
}
