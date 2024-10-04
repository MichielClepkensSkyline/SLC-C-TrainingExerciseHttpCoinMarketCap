namespace QAction_2.Dtos
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

	internal class LatestListingResponseDto
	{
		[JsonProperty("status")]
		public StatusDto Status { get; set; }

		[JsonProperty("data")]
		public List<CoinDataDto> Data { get; set; }
	}
}
