namespace QAction_2.Dtos.Shared
{
    using Newtonsoft.Json;

    public class QuoteDto
    {
        [JsonProperty("USD")]
        public UsdDto Usd { get; set; }
    }

}
