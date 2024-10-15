namespace QAction_2.Tests
{
	using System;
	using System.IO;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Moq;

	using QAction_2.Helpers;

	using Skyline.DataMiner.Scripting;

	[TestClass]
	public class CryptoCategoriesProcessorTests
	{
		private Mock<SLProtocolExt> mockProtocol;

		[TestInitialize]
		public void Setup()
		{
			this.mockProtocol = new Mock<SLProtocolExt>();
		}

		[TestMethod]
		public void GetAndDeserializeSingleCategoryResponseTest()
		{
			// Arrange
			var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "ExpectedResponse.json");
			var responseString = File.ReadAllText(filePath);
			this.mockProtocol.Setup(p => p.GetParameter(213)).Returns(responseString);

			// Act
			var result = CryptoCategoriesHelper.GetAndDeserializeSingleCategoryResponse(this.mockProtocol.Object);

			// Assert
			result.Should().NotBeNull();

			var expectedData = new
			{
				Id = "66fc9cad98812260d55bb735",
				Name = "Hybrid token standard",
				Title = "Hybrid token standard",
				Description = "Hybrid token standard",
				NumTokens = 12,
				LastUpdated = DateTime.ParseExact("2024-10-02T01:07:30.642Z", "yyyy-MM-ddTHH:mm:ss.fffZ",System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal),
				AvgPriceChange = 1.1550528989999997,
				MarketCap = 239698583.84,
				MarketCapChange = -1.8331500000000003,
				VolumeChange = 80.88293,
				CoinsCount = 12,
			};

			result.Data.Should().BeEquivalentTo(expectedData, options => options.Excluding(r => r.CoinsCount));
			result.Data.Coins.Should().HaveCount(12, "The 'coins' count should match the expected value.");
		}
	}
}