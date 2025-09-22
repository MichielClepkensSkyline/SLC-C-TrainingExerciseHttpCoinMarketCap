using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Text;

using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.Protocol.Extension;
using QAction_3;
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
	public static void Run(SLProtocol protocol)
	{
		try
		{
			// Check if status code is 200? (Moet ik ook andere codes chechken? Checken of het begint met een 2?
			// Beter om in de content status de check te doen?
			protocol.Log($"QA{protocol.QActionID}|Run|{protocol.GetParameter(Parameter.statuscode)}", LogType.Error, LogLevel.NoLogging);
			string statusCode = (string)protocol.GetParameter(Parameter.statuscode);
			int status = Int32.Parse(statusCode.Split(' ')[1]);
			protocol.Log($"QA{protocol.QActionID}|Run|{status}", LogType.Error, LogLevel.NoLogging);
			if (status == 200)
			{
				protocol.Log($"QA{protocol.QActionID}|Run|YES, Let's go", LogType.Error, LogLevel.NoLogging);
				Root latest_listings = SecureNewtonsoftDeserialization.DeserializeObject<Root>(protocol.GetParameter(Parameter.responsecontent).ToString());
				int counter = 0;
				foreach (Data data in latest_listings.Data)
				{
					protocol.Log($"QA{protocol.QActionID}|Run|{data.Name}", LogType.Error, LogLevel.NoLogging);
					counter++;
				}
				protocol.Log($"QA{protocol.QActionID}|Run|{counter}", LogType.Error, LogLevel.NoLogging);
			}
			//Parameter.responsecontent
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void FillLastListings(SLProtocol protocol, Data data)
	{
		Dictionary<string, >
		string id = null;
		string name = null;
		string symbol = null;
		DateTime dateAdded = DateTime.Now;
		double total_supply = 0;
		string platform_name = null;
		DateTime lastUpdated = DateTime.Now;
		string quote = null;
		double quotePrice = 0;
		double quoteVolume24h = 0;
		double percentChange24h = 0;
	}
}
