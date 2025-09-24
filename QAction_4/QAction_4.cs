using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Scripting.Categorie;
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

	private static void FillCategories(SLProtocolExt protocol, List<Categorie> categories)
	{
        Dictionary<string, CategoriesQActionRow> categorieRows = new Dictionary<string, CategoriesQActionRow>();
        foreach (Categorie categorie in categories)
        {
            CategoriesQActionRow categorieQActionRow = new CategoriesQActionRow
            {
                Categoriesid = categorie.Id,
                Categoriesname = categorie.Name,
                Categoriestitle = categorie.Title,
                Categoriesdescription = categorie.Description,
                Categoriesnumberoftokens = categorie.NumTokens,
                Categoriesaveragepricechange = categorie.AvgPriceChange,
                Categoriesmarketcap = categorie.MarketCap,
                Categoriesmarketcapchange = categorie.MarketCapChange,
                Categoriesvolume = categorie.Volume,
                Categoriesvolumechange = categorie.VolumeChange,
                Categorieslastupdated = categorie.LastUpdated.ToLocalTime(), //TODO fix de tijd
            };
            categorieRows.Add(categorie.Id, categorieQActionRow);
        }

        object[] categoriesColumns = protocol.categories.QActionRowsToObjectFillArray(categorieRows.Values.ToArray());
        protocol.FillArray(Parameter.Categories.tablePid, categoriesColumns);
        /*object succes = protocol.FillArray(Parameter.Categories.tablePid, categoriesColumns);
        protocol.Log($"QA{protocol.QActionID}|FillCategories|{succes.ToString()} SOFIAN", LogType.Error, LogLevel.NoLogging); // Kan toevoegen voor het checken op succes.*/
    }
}
