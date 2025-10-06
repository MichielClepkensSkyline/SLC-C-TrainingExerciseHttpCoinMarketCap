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
    private const int ExceptionValue = -1;
    /// <summary>
    /// The QAction entry point.
    /// </summary>
    /// <param name="protocol">Link with SLProtocol process.</param>
    public static void Run(SLProtocolExt protocol)
	{
		try
		{
            if (StatusCode.CheckStatusCode(protocol, Parameter.statuscodecategories, Parameter.responsecontentcategories, Parameter.urlcategories))
            {
                Root root = SecureNewtonsoftDeserialization.DeserializeObject<Root>(protocol.GetParameter(Parameter.responsecontentcategories).ToString());
                if (root == null)
                {
                    protocol.Log($"QA{protocol.QActionID}|Run|root is null", LogType.Error, LogLevel.NoLogging);
                }
                else if(root.Status == null)
                {
                    protocol.Log($"QA{protocol.QActionID}|Run|root.Status is null", LogType.Error, LogLevel.NoLogging);
                }
                else if (root.Status.ErrorCode == 0)
                {
                    FillCategories(protocol, root.Categories);
                }
                else
                {
                    protocol.Log($"QA{protocol.QActionID}|Run|{root.Status.ErrorMessage}", LogType.Error, LogLevel.NoLogging);
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
            if (category != null && category.Id != null)
            {
                CategoriesQActionRow categoryQActionRow = new CategoriesQActionRow
                {
                    Categoriesid = category.Id,
                    Categoriesname = category.Name == null ? string.Empty : category.Name,
                    Categoriestitle = category.Title == null ? string.Empty : category.Title,
                    Categoriesdescription = category.Description == null ? string.Empty : category.Description,
                    Categoriesnumberoftokens = category.NumTokens == null ? ExceptionValue : category.NumTokens,
                    Categoriesaveragepricechange = category.AvgPriceChange == null ? ExceptionValue : category.AvgPriceChange,
                    Categoriesmarketcap = category.MarketCap == null ? ExceptionValue : category.MarketCap,
                    Categoriesmarketcapchange = category.MarketCapChange == null ? ExceptionValue : category.MarketCapChange,
                    Categoriesvolume = category.Volume == null ? ExceptionValue : category.Volume,
                    Categoriesvolumechange = category.VolumeChange == null ? ExceptionValue : category.VolumeChange,
                    Categorieslastupdated = category.LastUpdated == null ? ExceptionValue : category.LastUpdated.ToLocalTime().ToOADate(),
                };
                categoryRows[category.Id] = categoryQActionRow;
            }
        }

        object[] categoriesColumns = protocol.categories.QActionRowsToObjectFillArray(categoryRows.Values.ToArray());
        bool succes = (bool)protocol.FillArray(Parameter.Categories.tablePid, categoriesColumns);
        if (!succes)
        {
            protocol.Log($"QA{protocol.QActionID}|FillCategories|The categories table set has not succeeded", LogType.Error, LogLevel.NoLogging);
        }
    }
}
