namespace QAction_2.Dtos.LatestListingResponse
{
    using Newtonsoft.Json;

    internal class ListingQuoteDto
    {
        [JsonProperty("USD")]
        public ListingUsdDto Usd { get; set; }
    }

}
