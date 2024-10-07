namespace QAction_2.Dtos.GlobalMetricsResponse
{
	using Newtonsoft.Json;

	public class GlobalMetricsQuoteDto
	{
		[JsonProperty("USD")]
		public GlobalMetricsUsdDto Usd { get; set; }
	}
}
