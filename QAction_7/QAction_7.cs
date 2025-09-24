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
			int status = Int32.Parse(statusCode.Split(' ')[1]);
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|statusCode: '{statusCode}'  and response {category.Data.Id}", LogType.Information, LogLevel.NoLogging);

			if (status == 200)
			{
				UpdateCategoryRow(protocol, category.Data);
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