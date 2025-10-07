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

	/// <summary>
	/// Unit Test Class.
	/// </summary>
	[TestClass]
	public class StatusCodeTests
	{
		private const string ResponseContent = "{This is an empty response body for testing}";
		private const string Url = "url/unit/testing";
		private const int QActionID = 0;
		private const int StatusCodePID = 50;
		private const int ResponseContentPID = 51;
		private const int UrlPID = 52;
		private static Mock<SLProtocol> fakeProtocol = new Mock<SLProtocol>();

		/// <summary>
		/// Testmethod with a correct statuscode.
		/// </summary>
		[TestMethod]
		public void CheckStatusCodeTest()
		{
			// Arrange
			string statusCode = "HTTP/1.1 200 OK";
			fakeProtocol.Setup(p => p.GetParameters(new uint[] { StatusCodePID, ResponseContentPID, UrlPID })).Returns(new object[] { statusCode, ResponseContent, Url });

			// Act
			bool result = StatusCode.CheckStatusCode(fakeProtocol.Object, StatusCodePID, ResponseContentPID, UrlPID);

			// Assert
			Assert.IsTrue(result);
		}

		/// <summary>
		/// Testmethod with a correct statuscode but without OK.
		/// </summary>
		[TestMethod]
		public void CheckStatusCodeTestWithoutOK()
		{
			// Arrange
			string statusCode = "HTTP/1.1 200";
			fakeProtocol.Setup(p => p.GetParameters(new uint[] { StatusCodePID, ResponseContentPID, UrlPID })).Returns(new object[] { statusCode, ResponseContent, Url });

			// Act
			bool result = StatusCode.CheckStatusCode(fakeProtocol.Object, StatusCodePID, ResponseContentPID, UrlPID);

			// Assert
			Assert.IsTrue(result);
		}

		/// <summary>
		/// Testmethod with a statuscode different than 200.
		/// </summary>
		[TestMethod]
		public void CheckStatusCodeNot200()
		{
			// Arrange
			string statusCode = "HTTP/1.1 204 No Content";
			fakeProtocol.Setup(p => p.GetParameters(new uint[] { StatusCodePID, ResponseContentPID, UrlPID })).Returns(new object[] { statusCode, ResponseContent, Url });
			fakeProtocol.Setup(p => p.QActionID).Returns(QActionID);
			string expectedMessage = this.BuildExpectedMessage(statusCode);

			// Act
			bool result = StatusCode.CheckStatusCode(fakeProtocol.Object, StatusCodePID, ResponseContentPID, UrlPID);

			// Assert
			Assert.IsFalse(result);
			fakeProtocol.Verify(p => p.Log(It.Is<string>(msg => msg == expectedMessage), LogType.Error, LogLevel.NoLogging));
		}

		/// <summary>
		/// Testmethod with a badly formed statuscode.
		/// </summary>
		[TestMethod]
		public void CheckWrongFormedStatusCode()
		{
			// Arrange
			string statusCode = "This 200 does not make sense!";
			fakeProtocol.Setup(p => p.GetParameters(new uint[] { StatusCodePID, ResponseContentPID, UrlPID })).Returns(new object[] { statusCode, ResponseContent, Url });
			fakeProtocol.Setup(p => p.QActionID).Returns(QActionID);
			string expectedMessage = this.BuildExpectedMessage(statusCode);

			// Act
			bool result = StatusCode.CheckStatusCode(fakeProtocol.Object, StatusCodePID, ResponseContentPID, UrlPID);

			// Assert
			Assert.IsFalse(result);
			fakeProtocol.Verify(p => p.Log(It.Is<string>(msg => msg == expectedMessage), LogType.Error, LogLevel.NoLogging));
		}

		/// <summary>
		/// Testmethod with an empty statuscode.
		/// </summary>
		[TestMethod]
		public void CheckEmptyStatusCode()
		{
			// Arrange
			string statusCode = string.Empty;
			fakeProtocol.Setup(p => p.GetParameters(new uint[] { StatusCodePID, ResponseContentPID, UrlPID })).Returns(new object[] { statusCode, ResponseContent, Url });
			fakeProtocol.Setup(p => p.QActionID).Returns(QActionID);
			string expectedMessage = this.BuildExpectedMessage(statusCode);

			// Act
			bool result = StatusCode.CheckStatusCode(fakeProtocol.Object, StatusCodePID, ResponseContentPID, UrlPID);

			// Assert
			Assert.IsFalse(result);
			fakeProtocol.Verify(p => p.Log(It.Is<string>(msg => msg == expectedMessage), LogType.Error, LogLevel.NoLogging));
		}

		private string BuildExpectedMessage(string statusCode)
		{
			return $"QA{QActionID}|CheckStatusCode|Bad statuscode:\nURL API call: {Url} \nStatuscode: {statusCode}\nResponse content: {ResponseContent}";
		}
	}
}