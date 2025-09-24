using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using QAction_4;

using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

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
			string statusCode = protocol.GetParameter(Parameter.statuscodecategories_7).ToString();
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|statusCode: '{statusCode}'", LogType.Information, LogLevel.NoLogging);

			int status = Int32.Parse(statusCode.Split(' ')[1]);

			if(status == 200)
			{
				FillCategoriesTable(protocol, categories);
			}
			else
			{
				protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown: Status of the response is: {status}", LogType.Error, LogLevel.NoLogging);
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

			}
		}

		protocol.FillArray(Parameter.Categories.tablePid, categoriesTableContent.Values.ToList(), NotifyProtocol.SaveOption.Full);
	}
}