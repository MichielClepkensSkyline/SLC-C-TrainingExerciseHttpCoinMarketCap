namespace QAction_2.Processors
{
    using QAction_2.Dtos.GeneralDataResponse;
    using QAction_2.Helpers;
    using Skyline.DataMiner.Scripting;

    public class CryptoGeneralDataProcessor
	{
		public static void HandleGeneralDataResponse(SLProtocolExt protocol)
		{
			var generalDataResponse = GetAndDeserializeGlobalDataResponse(protocol);
			SetGeneralDataParams(protocol, generalDataResponse);
		}

		private static GeneralDataDto GetAndDeserializeGlobalDataResponse(SLProtocolExt protocol)
		{
			var parameters = (object[])protocol.GetParameters(
				new uint[]
				{
					Parameter.responsecontentlatestglobalmetrics_211,
					Parameter.responsecontentcategories_212,
				});

			var globalMetricsResponseString = parameters[0] as string;
			var categoriesResponseString = parameters[1] as string;

			return new GeneralDataDto
			{
				GlobalMetricsResponse = CryptoGlobalMetricsHelper.DeserializeGlobalMetricsResponse(globalMetricsResponseString),
				CategoriesResponse = CryptoCategoriesHelper.DeserializeCategoriesResponse(categoriesResponseString),
			};
		}

		private static void SetGeneralDataParams(SLProtocolExt protocol, GeneralDataDto generalDataResponse)
		{
			CryptoGlobalMetricsHelper.SetGlobalMetricsParams(protocol, generalDataResponse.GlobalMetricsResponse);
			CryptoCategoriesHelper.SetCategoriesColumns(protocol, generalDataResponse.CategoriesResponse);
		}
	}
}
