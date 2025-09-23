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
			int status = Int32.Parse(statusCode);

			if(status == 200)
			{

			}
			else
			{

			}
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}