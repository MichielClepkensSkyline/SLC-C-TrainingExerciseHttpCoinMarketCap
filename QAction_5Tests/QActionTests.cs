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
	using Skyline.DataMiner.Scripting.LatestQuote;
	using Skyline.DataMiner.Utils.Protocol.Extension;

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
			protocol.Setup(p => p.GetParameter(Parameter.statuscodelatestquote_7)).Returns("HTTP/1.1 500 Internal Server Error");

			//Act

			QAction.Run(protocol.Object);

			//Assert

			protocol.Verify(p => p.GetParameter(Parameter.responselatestquote_8), Times.Never);
			protocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<object[]>()), Times.Never);
		}

		[TestMethod]
		public void RunTest_ShouldReturn_ErrorCheckFails()
		{
			//Arrange
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

			//Act

			QAction.Run(protocol.Object);

			//Assert
			protocol.Verify(p => p.SetParameters(It.IsAny<int[]>(), It.IsAny<object[]>()), Times.Never);
		}
	}
}