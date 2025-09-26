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
            if (StatusCode.CheckStatusCode(protocol, Parameter.statuscodeonecategory, Parameter.responsecontentonecategory, Parameter.urlonecategory))
            {
                RootOneCategory root = SecureNewtonsoftDeserialization.DeserializeObject<RootOneCategory>(protocol.GetParameter(Parameter.responsecontentonecategory).ToString());
                if (root.Status == null)
                {
                    protocol.Log($"QA{protocol.QActionID}|Run|root.Status is null", LogType.Error, LogLevel.NoLogging);
                }
                else if (root.Status.ErrorCode == 0)
                {
                    FillCategory(protocol, root.Category);
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

	private static void FillCategory(SLProtocolExt protocol, OneCategory category)
    {
        if (category != null)
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
                Categorieslastupdated = category.LastUpdated.ToLocalTime().ToOADate(),
            };
            bool succes = (bool)protocol.categories.SetRow(category.Id, categoryQActionRow)[0];
            if (!succes)
            {
                protocol.Log($"QA{protocol.QActionID}|FillCategory|Setting the category row ({category.Id}) has not succeeded", LogType.Error, LogLevel.NoLogging);
            }
        }
    }
}
