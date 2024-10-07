namespace QAction_2.Dtos.GlobalMetricsResponse
{
	using Newtonsoft.Json;

	using QAction_2.Dtos.Shared;

	public class GlobalMetricsResponseDto
	{
		[JsonProperty("status")]
		public StatusDto Status { get; set; }

		[JsonProperty("data")]
		public GlobalMetricsDataDto Data { get; set; }
	}
}
