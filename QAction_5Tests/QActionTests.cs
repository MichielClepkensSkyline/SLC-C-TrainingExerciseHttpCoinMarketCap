namespace Tests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Skyline.DataMiner.Scripting;
	using Skyline.DataMiner.Scripting.LatestQuote;

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
			protocol.Setup(p => p.GetParameter(Parameter.statuscodelatestquote_7)).Returns("HTTP/1.1 500 Internal Server Error");

			QAction.Run(protocol.Object);

			protocol.Verify(p => p.GetParameter(Parameter.responselatestquote_8), Times.Never);
			protocol.Verify(p => p.SetParameters(It.IsAny<int[]>(), It.IsAny<object[]>()), Times.Never);
		}

		[TestMethod]
		public void RunTest_ShouldReturn_ErrorCheckFails()
		{
			protocol.Setup(p => p.GetParameter(Parameter.statuscodelatestquote_7)).Returns("HTTP/1.1 500 Internal Server Error");

			var latestQuote = new LatestQuote
			{
				Status = new Status
				{
					ErrorCode = 1,
					ErrorMessage = "Error message",
				},
			};

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(latestQuote);

			protocol.Setup(p => p.GetParameter(Parameter.responselatestquote_8)).Returns(json);

			QAction.Run(protocol.Object);

			protocol.Verify(p => p.SetParameters(It.IsAny<int[]>(), It.IsAny<object[]>()), Times.Never);
		}
	}
}