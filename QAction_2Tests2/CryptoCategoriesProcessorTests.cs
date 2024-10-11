namespace QAction_2.Tests
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using QAction_2.Helpers;
    using Skyline.DataMiner.Scripting;

    [TestClass()]
	public class CryptoCategoriesProcessorTests
	{
		private Mock<SLProtocolExt> mockProtocol;

		[TestInitialize]
		public void Setup()
		{
			this.mockProtocol = new Mock<SLProtocolExt>();
		}

		[TestMethod()]
		public void GetAndDeserializeSingleCategoryResponseTest()
		{
			// Arrange
			var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "ExpectedResponse.json");
			var responseString = File.ReadAllText(filePath);
			this.mockProtocol.Setup(p => p.GetParameter(213))
						 .Returns(responseString);

			// Act
			var result = CryptoCategoriesHelper.GetAndDeserializeSingleCategoryResponse(this.mockProtocol.Object);

			// Assert
			Assert.IsNotNull(result, "The deserialized response should not be null.");

			Assert.AreEqual("66fc9cad98812260d55bb735", result.Data.Id, "The 'id' field did not match the expected value.");
			Assert.AreEqual("Hybrid token standard", result.Data.Name, "The 'name' field did not match the expected value.");
			Assert.AreEqual("Hybrid token standard", result.Data.Title, "The 'title' field did not match the expected value.");
			Assert.AreEqual("Hybrid token standard", result.Data.Description, "The 'description' field did not match the expected value.");
			Assert.AreEqual(12, result.Data.NumTokens, "The 'num_tokens' field did not match the expected value.");
			Assert.AreEqual(
				DateTime.ParseExact("2024-10-02T01:07:30.642Z", "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal),
				result.Data.LastUpdated,
				"The 'last_updated' field did not match the expected value.");
			Assert.AreEqual(1.1550528989999997, result.Data.AvgPriceChange, "The 'avg_price_change' field did not match the expected value.");
			Assert.AreEqual(239698583.84, result.Data.MarketCap, "The 'market_cap' field did not match the expected value.");
			Assert.AreEqual(-1.8331500000000003, result.Data.MarketCapChange, "The 'market_cap_change' field did not match the expected value.");
			Assert.AreEqual(80.88293, result.Data.VolumeChange, "The 'volume_change' field did not match the expected value.");
			Assert.AreEqual(12, result.Data.Coins.Count, "The 'coins' count did not match the expected value.");
		}
	}
}