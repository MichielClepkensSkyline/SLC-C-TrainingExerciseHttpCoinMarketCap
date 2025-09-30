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
			if (!StatusCode.CheckStatusCode(protocol, Parameter.statuscodeindividualcategory_9))
			{
				return;
			}

			string json = protocol.GetParameter(Parameter.responseindividualcategory_10).ToString();
			Category category = SecureNewtonsoftDeserialization.DeserializeObject<Category>(json);

			if (!StatusCode.CheckErrorCode(protocol, category.Status.ErrorCode, category.Status.ErrorMessage))
			{
				return;
			}

			UpdateCategoryRow(protocol, category.Data);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void UpdateCategoryRow(SLProtocol protocol, Data category)
	{
		var exceptionValue = -100;

		object[] newCategoryRow = new CategoriesQActionRow
		{
			Categoriesid_201 = category.Id.ToString(),
			Categoriesname_202 = category.Name ?? exceptionValue.ToString(),
			Categoriesnumberoftokens_203 = category.NumTokens ?? exceptionValue,
			Categoriesaveragepricechange_204 = category.AvgPriceChange ?? exceptionValue,
			Categoriesmarketcap_205 = category.MarketCap ?? exceptionValue,
			Categoriesmarketcapchange_206 = category.MarketCapChange ?? exceptionValue,
			Categoriesvolume_207 = category.Volume ?? exceptionValue,
			Categoriesvolumechange_208 = category.VolumeChange ?? exceptionValue,
			Categorieslastupdated_209 = category.LastUpdated.ToOADate(),
		};

		protocol.SetRow(Parameter.Categories.tablePid, category.Id, newCategoryRow);
	}
}