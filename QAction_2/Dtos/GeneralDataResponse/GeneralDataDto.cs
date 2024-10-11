namespace QAction_2.Dtos.GeneralDataResponse
{
	using QAction_2.Dtos.CategoriesResponse;
	using QAction_2.Dtos.GlobalMetricsResponse;

	public class GeneralDataDto
	{
        public GlobalMetricsResponseDto GlobalMetricsResponse { get; set; }

        public CategoriesResponseDto CategoriesResponse { get; set; }
    }
}
