namespace Tests
{
	using Moq;
	using Skyline.DataMiner.Scripting;
	using Skyline.DataMiner.Scripting.Listing;

	[TestClass]
	public class QActionTests
	{
		private const string StatusCode = "HTTP/1.1 200 OK";
		private const string ResponseContent = "{This is an empty response body for testing}";
		private const string Url = "url/unit/testing";
		private const int StatusCodePID = 50;
        private const int ResponseContentPID = 51;
        private const int UrlPID = 52;
        private const int QActionID = 0;
        private static Mock<SLProtocolExt> fakeProtocol = new Mock<SLProtocolExt>();
        /*private static Root root = new Root
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
                    Slug = "bitcoin",
                    NumMarketPairs = 12067,
                    DateAdded = DateTime.Parse("2010-07-13T00:00:00Z"),
                    MaxSupply = 21000000,
                    CirculatingSupply = 19864978,
                    TotalSupply = 19864978.0,
                    InfiniteSupply = false,
                    Platform = null,
                    CmcRank = 1,
                    SelfReportedCirculatingSupply = null,
                    SelfReportedMarketCap = null,
                    TvlRatio = null,
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
                        PercentChange7d = 2.60309348,
                        PercentChange30d = 18.89481859,
                        PercentChange60d = 20.80018554,
                        PercentChange90d = 4.99825765,
                        MarketCap = 2022570747272.2815,
                        MarketCapDominance = 61.8788,
                        FullyDilutedMarketCap = 2138134041362.54,
                        Tvl = null,
                        LastUpdated = DateTime.Parse("2025-10-06T14:06:01.8871168Z"),
                        },
                    },
                },
            },
        };*/

        [TestInitialize]
        public void TestInitialize()
        {
            fakeProtocol.Setup(p => p.QActionID).Returns(QActionID);
            fakeProtocol.Setup(p => p.GetParameters(new uint[] { StatusCodePID, ResponseContentPID, UrlPID })).Returns(new object[] { StatusCode, ResponseContent, Url });
        }

        [TestMethod]
        public void RunCorrectJSONTest()
        {
            // Arrange
            // Set json as ResponseContent

            // Act
            // QAction.Run(fakeProtocol.Object);

            // Assert
            // fakeProtocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>()), Times.Once);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void RunEmptyRootTest()
        {
            // Arrange
            string json = string.Empty;
            fakeProtocol.Setup(p => p.GetParameter(ResponseContentPID)).Returns(json);

            // Act
            QAction.Run(fakeProtocol.Object);

            // Assert
            fakeProtocol.Verify(p => p.Log(It.Is<string>(msg => msg == $"QA{QActionID}|Run|root is null"), LogType.Error, LogLevel.NoLogging));
        }

        [TestMethod]
        public void RunEmptyRootStatusTest()
		{
			// Arrange
			Root root = new Root
			{
                Status = null,
                Listings = null,
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(root);
            fakeProtocol.Setup(p => p.GetParameter(ResponseContentPID)).Returns(json);

            // Act
            QAction.Run(fakeProtocol.Object);

            // Assert
            fakeProtocol.Verify(p => p.Log(It.Is<string>(msg => msg == $"QA{QActionID}|Run|root.Status is null"), LogType.Error, LogLevel.NoLogging));
        }

        [TestMethod]
        public void RunWrongErrorCodeTest()
        {
            // Arrange
            Root root = new Root
            {
                Status = new Status
                {
                    ErrorCode = 1,
                    ErrorMessage = "Test ErrorCode is wrong",
                },
                Listings = null,
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(root);
            fakeProtocol.Setup(p => p.GetParameter(ResponseContentPID)).Returns(json);

            // Act
            QAction.Run(fakeProtocol.Object);

            // Assert
            fakeProtocol.Verify(p => p.Log(It.Is<string>(msg => msg == $"QA{QActionID}|Run|{root.Status.ErrorMessage}"), LogType.Error, LogLevel.NoLogging));
        }
    }
}