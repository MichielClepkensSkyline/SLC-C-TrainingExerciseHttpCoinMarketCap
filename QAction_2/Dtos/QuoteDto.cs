namespace QAction_2.Dtos
{
	using Newtonsoft.Json;

	internal class QuoteDto
	{
		[JsonProperty("USD")]
		public UsdDto Usd { get; set; }
	}

}
