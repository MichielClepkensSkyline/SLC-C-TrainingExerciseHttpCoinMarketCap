namespace Tests
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Skyline.DataMiner.Scripting;

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
		public void FormUrlTest_ReturnsCorrectUrl_ValidRowKey()
		{
			string rowPK = "abei52klf";

			string expected = "api/custom/coinmarketcap?content=category&id=abei52klf";

			string result = QAction.FormUrl(rowPK);

			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void FormUrlTest_ReturnsCorrectUrl_EmptyRowKey()
		{
			string rowPK = string.Empty;

			string expected = "api/custom/coinmarketcap?content=category&id=";

			string result = QAction.FormUrl(rowPK);

			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void RunMethodTest_SetsParameter_InvalidRowKey()
		{
			protocol.Setup(p => p.RowKey()).Throws<InvalidOperationException>();

			protocol.Setup(p => p.SetParameter(It.IsAny<int>(), It.IsAny<string>()));

			QAction.Run(protocol.Object);

			protocol.Verify(p => p.Log(It.IsAny<string>(), It.IsAny<LogType>(), It.IsAny<LogLevel>()));
		}
	}
}