namespace Tests
{
	using Moq;
	using Skyline.DataMiner.Scripting;
	using Skyline.DataMiner.Scripting.Listing;

	/// <summary>
	/// Unit test class.
	/// </summary>
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

		/// <summary>
		/// Method to set some parameters on the protocol.
		/// </summary>
		[TestInitialize]
		public void TestInitialize()
		{
			fakeProtocol.Setup(p => p.QActionID).Returns(QActionID);
			fakeProtocol.Setup(p => p.GetParameters(new uint[] { StatusCodePID, ResponseContentPID, UrlPID })).Returns(new object[] { StatusCode, ResponseContent, Url });
		}

		/// <summary>
		/// Testmethod with an empty root.
		/// </summary>
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

		/// <summary>
		/// Testmethod with an empty root status.
		/// </summary>
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

		/// <summary>
		/// Testmethod with an error code different than 0.
		/// </summary>
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