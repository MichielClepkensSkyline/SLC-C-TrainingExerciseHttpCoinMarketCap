namespace Skyline.Protocol.QAction_1.Tests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Moq;

	using Skyline.DataMiner.Scripting;

	using System;

	[TestClass]
	public class StatusCodeTests
	{
		private static Mock<SLProtocol> protocol;

		[TestInitialize]
		public void TestInitialize()
		{
			protocol = new Mock<SLProtocol>();
		}


		[TestMethod]
		public void CheckStatusCode_ShouldReturnTrue_For200Status()
		{
			// protocol returns "HTTP/1.1 200 OK" if GetParameter is called
			protocol.Setup(p => p.GetParameter(It.IsAny<int>())).Returns("HTTP/1.1 200 OK");

			bool result = StatusCode.CheckStatusCode(protocol.Object, 1);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void CheckStatusCode_ShouldReturnFalse_ForNon200Status()
		{
			protocol.Setup(p => p.GetParameter(It.IsAny<int>())).Returns("HTTP/1.1 404 Not Found");

			bool result = StatusCode.CheckStatusCode(protocol.Object, 1);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void CheckStatusCode_ShouldLogError_ForNon200Status()
		{
			protocol.Setup(p => p.GetParameter(It.IsAny<int>())).Returns("HTTP/1.1 500 Internal Server Error");

			StatusCode.CheckStatusCode(protocol.Object, 1);

			protocol.Verify(
				p => p.Log(
				It.Is<string>(msg => msg.Contains("Status of the response is: 500")),
				LogType.Error, LogLevel.NoLogging), Times.Once);
		}

		[TestMethod]
		public void CheckStatusCode_ShouldThrowException_ForInvalidFormat()
		{
			protocol.Setup(p => p.GetParameter(It.IsAny<int>())).Returns("INVALID RESPONSE");

			Assert.ThrowsException<FormatException>(() =>
			{
				StatusCode.CheckStatusCode(protocol.Object, 1);
			});
		}

		[TestMethod]
		public void CheckErrorCode_ShouldReturnTrue_For0ErrorCode()
		{

			bool result = StatusCode.CheckErrorCode(protocol.Object, 0, null);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void CheckErrorCode_ShouldReturnFalse_ForNot0ErrorCode()
		{

			bool result = StatusCode.CheckErrorCode(protocol.Object, 1, "Error message");

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void CheckErrorCode_ShouldLogError_ForNot0ErrorCode()
		{

			bool result = StatusCode.CheckErrorCode(protocol.Object, 1, "Not Authorized");

			protocol.Verify(
				p => p.Log(
				It.Is<string>(msg => msg.Contains("Status of the response is: 1 \n Error Message: Not Authorized")),
				LogType.Error, LogLevel.NoLogging), Times.Once);
		}

		[TestMethod]
		public void CheckErrorCode_ShouldHandleNullErrorMessage()
		{
			Assert.ThrowsException<NullReferenceException>(() =>
			{
				StatusCode.CheckErrorCode(protocol.Object, 888, null);
			});
		}

	}
}