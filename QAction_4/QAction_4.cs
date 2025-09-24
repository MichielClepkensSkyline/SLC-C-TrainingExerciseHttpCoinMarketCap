using QAction_4;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

/// <summary>
/// DataMiner QAction Class: Get Categories Data Into a Table.
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
            string data = Convert.ToString(protocol.GetParameter(Parameter.jsonresponsecategories_301));
            Categories deserializedCategories = SecureNewtonsoftDeserialization.DeserializeObject<Categories>(data);
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Data:{Environment.NewLine}{data}", LogType.Error, LogLevel.NoLogging);
            Dictionary<string, object[]> categoriesDictionary = new Dictionary<string, object[]>();
            foreach (Category category in deserializedCategories.Data)
            {
                if (String.IsNullOrWhiteSpace(category.Id))
                {
                    protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|No primary key found for traansport stream{Environment.NewLine}", LogType.Error, LogLevel.NoLogging);
                }

                categoriesDictionary[category.Id] = new CategoriesQActionRow
                {
                    Categoriesid_401 = category.Id,
                    Categoriestitle_402 = category.Title,
                    Categoriesnumberoftokens_403 = category.NumTokens,
                    Categoriesavreagepricechange_404=category.AvgPriceChange,
                    Categoriesmarketcap_405 = category.MarketCap,
                    Categoriesvolume_406 =category.Volume,
                    Categorieslastupdated_407 =category.LastUpdated.ToOADate(),
                }.ToObjectArray();
                protocol.FillArray(Parameter.Categories.tablePid, categoriesDictionary.Values.ToList(), NotifyProtocol.SaveOption.Full);
            }
        }
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}