using System;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Scripting.Category;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;
using Skyline.Protocol.QAction_1;

/// <summary>
/// DataMiner QAction Class: Parse Individual Category.
/// </summary>
public static class QAction
{
	/// <summary>
	/// The QAction entry point.
	/// </summary>
	/// <param name="protocol">Link with SLProtocol process.</param>
	public static void Run(SLProtocol protocol)
	{
		try
		{
			string json = protocol.GetParameter(Parameter.responseindividualcategory_12).ToString();
			Category category = SecureNewtonsoftDeserialization.DeserializeObject<Category>(json);

			if (StatusCode.CheckStatusCode(protocol, Parameter.statuscodeindividualcategory_11))
			{
				if(StatusCode.CheckErrorCode(protocol, category.Status.ErrorCode, category.Status.ErrorMessage))
				{
					UpdateCategoryRow(protocol, category.Data);
				}
			}
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void UpdateCategoryRow(SLProtocol protocol, Data category)
	{
		object[] newCategoryRow = new CategoriesQActionRow
		{
			Categoriesid_201 = category.Id.ToString(),
			Categoriesname_202 = category.Name,
			Categoriesnumberoftokens_203 = category.NumTokens,
			Categoriesaveragepricechange_204 = category.AvgPriceChange,
			Categoriesmarketcap_205 = category.MarketCap,
			Categoriesmarketcapchange_206 = category.MarketCapChange,
			Categoriesvolume_207 = category.Volume,
			Categoriesvolumechange_208 = category.VolumeChange,
			Categorieslastupdated_209 = category.LastUpdated.ToOADate(),
		};

		protocol.SetRow(Parameter.Categories.tablePid, category.Id, newCategoryRow);
	}
}