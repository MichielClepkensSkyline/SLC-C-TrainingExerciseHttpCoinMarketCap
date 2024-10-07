namespace QAction_2.Dtos.LatestListingResponse
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using QAction_2.Dtos.Shared;

    internal class ListingResponseDto
    {
        [JsonProperty("status")]
        public StatusDto Status { get; set; }

        [JsonProperty("data")]
        public List<CoinDataDto> Data { get; set; }
    }
}
