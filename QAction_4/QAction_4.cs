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
			if (!StatusCode.CheckStatusCode(protocol, Parameter.statuscodecategories_5))
			{
				return;
			}

			string json = protocol.GetParameter(Parameter.responsecategories_6).ToString();
			Categories categories = SecureNewtonsoftDeserialization.DeserializeObject<Categories>(json);

			if (categories == null)
			{
				protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run| Categories are null", LogType.Error, LogLevel.NoLogging);
				return;
			}

			if (!StatusCode.CheckErrorCode(protocol, categories.Status.ErrorCode, categories.Status.ErrorMessage))
			{
				return;
			}

			FillCategoriesTable(protocol, categories);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void FillCategoriesTable(SLProtocol protocol, Categories categories)
	{
		try
		{
			Dictionary<string, object[]> categoriesTableContent = new Dictionary<string, object[]>();

			var exceptionValue = -100;

			foreach (Category category in categories.CategoryList)
			{
				if (String.IsNullOrWhiteSpace(category.Id))
				{
					protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run| Category Id is null", LogType.Error, LogLevel.NoLogging);
					return;
				}

				categoriesTableContent[category.Id] = new CategoriesQActionRow
				{
					Categoriesid_201 = category.Id.ToString(),
					Categoriesname_202 = category.Name ?? exceptionValue.ToString(),
					Categoriesnumberoftokens_203 = category.NumTokens ?? exceptionValue,
					Categoriesaveragepricechange_204 = category?.AvgPriceChange ?? exceptionValue,
					Categoriesmarketcap_205 = category.MarketCap ?? exceptionValue,
					Categoriesmarketcapchange_206 = category.MarketCapChange ?? exceptionValue,
					Categoriesvolume_207 = category.Volume ?? exceptionValue,
					Categoriesvolumechange_208 = category.VolumeChange ?? exceptionValue,
					Categorieslastupdated_209 = category.LastUpdated.ToLocalTime().ToOADate(),
				}.ToObjectArray();
			}

			protocol.FillArray(Parameter.Categories.tablePid, categoriesTableContent.Values.ToList(), NotifyProtocol.SaveOption.Full);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}