namespace QAction_2.Processors
{
	using QAction_2.Dtos.GeneralDataResponse;
	using QAction_2.Helpers;

	using Skyline.DataMiner.Scripting;

	public class CryptoGeneralDataProcessor
	{
		public static void HandleGeneralDataResponse(SLProtocolExt protocol)
		{
			var generalDataResponse = GetAndDeserializeGeneralDataResponse(protocol);
			SetGeneralDataParams(protocol, generalDataResponse);
		}

		private static GeneralDataDto GetAndDeserializeGeneralDataResponse(SLProtocolExt protocol)
		{
			var parameters = (object[])protocol.GetParameters(
				new uint[]
				{
					Parameter.statuscodelatestglobalmetrics_201,
					Parameter.statuscodecategories_202,
					Parameter.responsecontentlatestglobalmetrics_211,
					Parameter.responsecontentcategories_212,
				});

			var globalMetricsResponseStatusCode = ResponseStatusHelper.ParseResponseStatusCode(protocol, parameters[0] as string);
			var categoriesResponseStatusCode = ResponseStatusHelper.ParseResponseStatusCode(protocol, parameters[1] as string);
			var globalMetricsResponseString = parameters[2] as string;
			var categoriesResponseString = parameters[3] as string;

			return new GeneralDataDto
			{
				GlobalMetricsResponse = CryptoGlobalMetricsHelper.DeserializeGlobalMetricsResponse(globalMetricsResponseString, globalMetricsResponseStatusCode),
				CategoriesResponse = CryptoCategoriesHelper.DeserializeCategoriesResponse(categoriesResponseString, categoriesResponseStatusCode),
			};
		}

		private static void SetGeneralDataParams(SLProtocolExt protocol, GeneralDataDto generalDataResponse)
		{
			CryptoGlobalMetricsHelper.SetGlobalMetricsParams(protocol, generalDataResponse.GlobalMetricsResponse);
			CryptoCategoriesHelper.SetCategoriesColumns(protocol, generalDataResponse.CategoriesResponse);
		}
	}
}
