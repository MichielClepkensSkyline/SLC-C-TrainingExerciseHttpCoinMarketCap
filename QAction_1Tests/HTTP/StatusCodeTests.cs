namespace Skyline.DataMiner.Scripting.HTTP.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Skyline.DataMiner.Scripting.HTTP;

	[TestClass]
	public class StatusCodeTests
	{
		private const string ResponseContent = "{This is an empty response body for testing}";
		private const string Url = "url/unit/testing";
		private const int QActionID = 0;
		private static Mock<SLProtocol>? fakeProtocol;

		[TestInitialize]
		public void TestInitialize()
		{
			fakeProtocol = new Mock<SLProtocol>();
			fakeProtocol.Setup(p => p.GetParameter(51)).Returns(ResponseContent);
			fakeProtocol.Setup(p => p.GetParameter(52)).Returns(Url);
		}

		private string BuildExpectedMessage(string statusCode)
		{
			return $"QA{QActionID}|CheckStatusCode|Bad statuscode:\nURL API call: {Url} \nStatuscode: {statusCode}\nResponse content: {ResponseContent}";
		}

		[TestMethod()]
		public void CheckStatusCodeTest()
		{
			// Arrange
			string statusCode = "HTTP/1.1 200 OK";
			fakeProtocol.Setup(p => p.GetParameter(50)).Returns(statusCode);

			// Act
			bool result = StatusCode.CheckStatusCode(fakeProtocol.Object, 50, 51, 52);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod()]
		public void CheckStatusCodeNot200()
		{
			// Arrange
			string statusCode = "HTTP/1.1 204 No Content";
			fakeProtocol.Setup(p => p.GetParameter(50)).Returns(statusCode);
			fakeProtocol.Setup(p => p.QActionID).Returns(QActionID);
			string expectedMessage = this.BuildExpectedMessage(statusCode);

			// Act
			bool result = StatusCode.CheckStatusCode(fakeProtocol.Object, 50, 51, 52);

			// Assert
			Assert.IsFalse(result);
			fakeProtocol.Verify(p => p.Log(It.Is<string>(msg => msg == expectedMessage), LogType.Error, LogLevel.NoLogging));
		}

		[TestMethod()]
		public void CheckWrongFormedStatusCode()
		{
			// Arrange
			string statusCode = "This 200 does not make sense!";
			fakeProtocol.Setup(p => p.GetParameter(50)).Returns(statusCode);
			fakeProtocol.Setup(p => p.QActionID).Returns(QActionID);
			string expectedMessage = this.BuildExpectedMessage(statusCode);

			// Act
			bool result = StatusCode.CheckStatusCode(fakeProtocol.Object, 50, 51, 52);

			// Assert
			Assert.IsFalse(result);
			fakeProtocol.Verify(p => p.Log(It.Is<string>(msg => msg == expectedMessage), LogType.Error, LogLevel.NoLogging));
		}
	}
}