namespace Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Moq;

	using Skyline.DataMiner.Scripting;
	using Skyline.DataMiner.Scripting.LatestListings;
	using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

	[TestClass]
	public class QActionTests
	{
		private static Mock<SLProtocol> protocol;

		[TestInitialize]

		public void TestInitialize()
		{
			protocol = new Mock<SLProtocol>();
		}

		[TestMethod]
		public void RunTest_ShouldReturn_StatusCheckFails()
		{
			//Arrange
			protocol.Setup(p => p.GetParameter(Parameter.statuscodelatestlistings_3)).Returns("HTTP/1.1 500 Internal Server Error");

			//Act

			QAction.Run(protocol.Object);

			//Assert

			protocol.Verify(p => p.GetParameter(Parameter.responselatestlistings_4), Times.Never);
			protocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<object[]>()), Times.Never);
		}

		[TestMethod]
		public void RunTest_ShouldReturn_ErrorCheckFails()
		{
			//Arrange
			protocol.Setup(p => p.GetParameter(Parameter.statuscodelatestlistings_3)).Returns("HTTP/1.1 500 Internal Server Error");

			var latestListing = new LatestListings
			{
				Status = new Status
				{
					ErrorCode = 1,
					ErrorMessage = "Error message",
				},
			};

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(latestListing);

			protocol.Setup(p => p.GetParameter(Parameter.responselatestlistings_4)).Returns(json);

			//Act

			QAction.Run(protocol.Object);

			//Assert

			protocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<object[]>()), Times.Never);
		}

		[TestMethod]
		public void RunTest_ShouldFillAray_ValidInputs()
		{
			//Arrange
			protocol.Setup(p => p.GetParameter(Parameter.statuscodelatestlistings_3)).Returns("HTTP/1.1 200 OK");

			var latestListing = new LatestListings
			{
				Status = new Status
				{
					ErrorCode = 0,
					ErrorMessage = null,
				},
				Listings = new List<Listing>
				{
					new Listing
					{
						Id = "1",
						Name = "Bitcoin",
						Symbol = "BTC",
						DateAdded = DateTime.Parse("2010-07-13T00:00:00Z"),
						CirculatingSupply = 19864978,
						MaxSupply = 21000000,
						CmcRank = 1,
						Platform = new Platform
									{
										Name = "Bitcoin",
									},
						LastUpdated = DateTime.Parse("2025-10-01T07:37:21.5113403Z"),
						Quote = new Quote
								{
									USD = new USD
									{
									Price = (double?)101815.90673154943,
									Volume24h = (double?)45620279708.066856,
									VolumeChange24h = (double?)-7.5884,
									PercentChange1h = (double?)-0.4304306,
									PercentChange24h = (double?)-1.83894047,
									MarketCap = (double?)2022570747272.2815,
									MarketCapDominance = (double?)61.8788,
									},
								},
					},
				},
			};

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(latestListing);

			protocol.Setup(p => p.GetParameter(Parameter.responselatestlistings_4)).Returns(json);

			//Act

			QAction.Run(protocol.Object);

			//Assert

			protocol.Verify(p => p.Log(It.IsAny<string>(), It.IsAny<LogType>(), It.IsAny<LogLevel>()), Times.Never);
			protocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), NotifyProtocol.SaveOption.Full), Times.Once);
		}

		[TestMethod]
		public void RunTest_ShouldFillAray_InvalidInputIdNull()
		{
			//Arrange
			protocol.Setup(p => p.GetParameter(Parameter.statuscodelatestlistings_3)).Returns("HTTP/1.1 200 OK");

			var latestListing = new LatestListings
			{
				Status = new Status
				{
					ErrorCode = 0,
					ErrorMessage = null,
				},
				Listings = new List<Listing>
				{
					new Listing
					{
						Id = null,
						Name = "Bitcoin",
						Symbol = "BTC",
						DateAdded = DateTime.Parse("2010-07-13T00:00:00Z"),
						CirculatingSupply = 19864978,
						MaxSupply = 21000000,
						CmcRank = 1,
						Platform = null,
						LastUpdated = DateTime.Parse("2025-10-01T07:37:21.5113403Z"),
						Quote = new Quote
								{
									USD = new USD
									{
									Price = 101815.90673154943,
									Volume24h = 45620279708.066856,
									VolumeChange24h = -7.5884,
									PercentChange1h = -0.4304306,
									PercentChange24h = -1.83894047,
									MarketCap = 2022570747272.2815,
									MarketCapDominance = 61.8788,
									},
								},
					},
				},
			};

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(latestListing);

			protocol.Setup(p => p.GetParameter(Parameter.responselatestlistings_4)).Returns(json);

			//Act

			QAction.Run(protocol.Object);

			//Assert

			protocol.Verify(p => p.Log(It.IsAny<string>(), It.IsAny<LogType>(), It.IsAny<LogLevel>()));
			protocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), NotifyProtocol.SaveOption.Full), Times.Never);
		}
	}
}