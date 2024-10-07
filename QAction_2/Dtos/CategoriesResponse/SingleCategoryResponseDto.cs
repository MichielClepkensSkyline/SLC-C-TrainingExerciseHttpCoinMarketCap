namespace QAction_2.Dtos.CategoriesResponse
{
	using Newtonsoft.Json;
	using QAction_2.Dtos.Shared;

	public class SingleCategoryResponseDto
	{
		[JsonProperty("status")]
		public StatusDto Status { get; set; }

		[JsonProperty("data")]
		public SingleCategoryDataDto Data { get; set; }
	}
}
