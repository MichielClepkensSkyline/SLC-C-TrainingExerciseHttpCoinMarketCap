using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QAction_7;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

/// <summary>
/// DataMiner QAction Class: Set Singe Row in The Categories Table.
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
            string data = Convert.ToString(protocol.GetParameter(Parameter.jsonrsponserefreshcategory_517));
            Root deserializedlCategory = SecureNewtonsoftDeserialization.DeserializeObject<Root>(data);
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Data:{Environment.NewLine}{data}", LogType.Information, LogLevel.NoLogging);

            object[] newRow = new CategoriesQActionRow
			{
				Categoriesid_401 = deserializedlCategory.Data.Id,
				Categoriestitle_402 = deserializedlCategory.Data.Title,
				Categoriesnumberoftokens_403 = deserializedlCategory.Data.NumTokens,
				Categoriesavreagepricechange_404 =deserializedlCategory.Data.AvgPriceChange,
				Categoriesmarketcap_405 = deserializedlCategory.Data.MarketCap,
				Categoriesvolume_406 =deserializedlCategory.Data.Volume,
				Categorieslastupdated_407 =deserializedlCategory.Data.LastUpdated.ToOADate(),
			};

            protocol.SetRow(Parameter.Categories.tablePid, deserializedlCategory.Data.Id, newRow);
        }
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}