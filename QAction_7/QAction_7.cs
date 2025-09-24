using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Skyline.DataMiner.Scripting;

using QAction_7;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

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
			string statusCode = protocol.GetParameter(Parameter.statuscodeindividualcategory_11).ToString();
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|statusCode: '{statusCode}'  and response {categories.Data.Id}", LogType.Information, LogLevel.NoLogging);

			object[] newCategoryRow = new CategoriesQActionRow
			{
				Categoriesid_201 = category.Data.Id.ToString(),
				Categoriesname_202 = category.Data.Name + " new",
				Categoriesnumberoftokens_203 = category.Data.NumTokens,
				Categoriesaveragepricechange_204 = category.Data.AvgPriceChange,
				Categoriesmarketcap_205 = category.Data.MarketCap,
				Categoriesmarketcapchange_206 = category.Data.MarketCapChange,
				Categoriesvolume_207 = category.Data.Volume,
				Categoriesvolumechange_208 = category.Data.VolumeChange,
				Categorieslastupdated_209 = category.Data.LastUpdated.ToOADate(),
			};

			protocol.SetRow(Parameter.Categories.tablePid, category.Data.Id, newCategoryRow);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}