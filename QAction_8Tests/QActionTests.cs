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
		public void RunTest_ShouldSetBearerToken_ValidAccessKeyToken()
		{
			string token = "bearer-token";
			protocol.Setup(p => p.GetParameter(Parameter.acceskeytoken_312)).Returns(token);
			string result = "Bearer " + token;

			QAction.Run(protocol.Object);

			protocol.Verify(p => p.SetParameter(It.IsAny<int>(), result), Times.Once);
		}
	}
}