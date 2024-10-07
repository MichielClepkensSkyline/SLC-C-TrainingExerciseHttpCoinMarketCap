namespace QAction_2.Dtos.CategoriesResponse
{
	using System.Collections.Generic;

	using Newtonsoft.Json;

	using QAction_2.Dtos.Shared;

	public class CategoriesResponseDto
	{
		[JsonProperty("status")]
		public StatusDto Status { get; set; }

		[JsonProperty("data")]
		public List<CategoryDto> Data { get; set; }
	}
}