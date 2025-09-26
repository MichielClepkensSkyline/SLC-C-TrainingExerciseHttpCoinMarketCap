using System;
using System.Collections.Generic;
using System.Linq;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Scripting.Categories;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;
using Skyline.Protocol.QAction_1;

/// <summary>
/// DataMiner QAction Class: Parse Categories.
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
			string json = protocol.GetParameter(Parameter.responsecategories_8).ToString();
			Categories categories = SecureNewtonsoftDeserialization.DeserializeObject<Categories>(json);
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|BEarer token : {protocol.GetParameter(Parameter.bearertoken_311) }", LogType.Error, LogLevel.NoLogging);

			if (StatusCode.CheckStatusCode(protocol, Parameter.statuscodecategories_7))
			{
				if (StatusCode.CheckErrorCode(protocol, categories.Status.ErrorCode, categories.Status.ErrorMessage))
				{
					FillCategoriesTable(protocol, categories);
				}
			}
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void FillCategoriesTable(SLProtocol protocol, Categories categories)
	{
		Dictionary<string, object[]> categoriesTableContent = new Dictionary<string, object[]>();

		foreach (Category category in categories.CategoryList)
		{
			if (!String.IsNullOrWhiteSpace(category.Id))
			{
				categoriesTableContent[category.Id] = new CategoriesQActionRow
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
				}.ToObjectArray();
			}
			else
			{
				protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run| Category Id is null", LogType.Error, LogLevel.NoLogging);
			}
		}

		protocol.FillArray(Parameter.Categories.tablePid, categoriesTableContent.Values.ToList(), NotifyProtocol.SaveOption.Full);
	}
}