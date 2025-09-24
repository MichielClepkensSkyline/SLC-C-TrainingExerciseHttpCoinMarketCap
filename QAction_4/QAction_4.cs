using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Scripting.Category;
using Skyline.DataMiner.Scripting.HTTP;
using Skyline.DataMiner.Utils.Protocol.Extension;
using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;
using static Skyline.DataMiner.Scripting.Parameter;

/// <summary>
/// DataMiner QAction Class.
/// </summary>
public static class QAction
{
	/// <summary>
	/// The QAction entry point.
	/// </summary>
	/// <param name="protocol">Link with SLProtocol process.</param>
	public static void Run(SLProtocolExt protocol)
	{
		try
		{
            if (StatusCode.CheckStatusCode(protocol, Parameter.statuscodecategories))
            {
                Root root = SecureNewtonsoftDeserialization.DeserializeObject<Root>(protocol.GetParameter(Parameter.responsecontentcategories).ToString());
                if (root.Status.ErrorCode == 0)
                {
                    FillCategories(protocol, root.Categories);
                }
                else
                {
                    protocol.Log($"QA{protocol.QActionID}|QA4Run|{root.Status.ErrorMessage}", LogType.Error, LogLevel.NoLogging);
                }
            }
        }
        catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void FillCategories(SLProtocolExt protocol, List<Category> categories)
	{
        Dictionary<string, CategoriesQActionRow> categoryRows = new Dictionary<string, CategoriesQActionRow>();
        foreach (Category category in categories)
        {
            CategoriesQActionRow categoryQActionRow = new CategoriesQActionRow
            {
                Categoriesid = category.Id,
                Categoriesname = category.Name,
                Categoriestitle = category.Title,
                Categoriesdescription = category.Description,
                Categoriesnumberoftokens = category.NumTokens,
                Categoriesaveragepricechange = category.AvgPriceChange,
                Categoriesmarketcap = category.MarketCap,
                Categoriesmarketcapchange = category.MarketCapChange,
                Categoriesvolume = category.Volume,
                Categoriesvolumechange = category.VolumeChange,
                Categorieslastupdated = category.LastUpdated.ToLocalTime(), //TODO fix de tijd
            };
            categoryRows.Add(category.Id, categoryQActionRow);
        }

        object[] categoriesColumns = protocol.categories.QActionRowsToObjectFillArray(categoryRows.Values.ToArray());
        protocol.FillArray(Parameter.Categories.tablePid, categoriesColumns);
        /*object succes = protocol.FillArray(Parameter.Categories.tablePid, categoriesColumns);
        protocol.Log($"QA{protocol.QActionID}|FillCategories|{succes.ToString()} SOFIAN", LogType.Error, LogLevel.NoLogging); // Kan toevoegen voor het checken op succes.*/
    }
}
