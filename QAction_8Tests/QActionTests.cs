using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skyline.DataMiner.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class QActionTests
    {
        [TestMethod()]
        public void SetNewBearerTokenValueTest()
        {
            //Arrange
            var mockProtocol = new Mock<SLProtocol>();

            string token = "jktrjlmitvz=";
            string expectedBearerToken = $"Bearer {token}";

            mockProtocol.Setup(p => p.GetParameter(Parameter.bearertokenvalue_600)).Returns(token);

            int capturedPid = 0;
            string capturedValue = null;

            mockProtocol.Setup(p => p.SetParameter(It.IsAny<int>(), It.IsAny<object>()))
                .Callback<int, object>((pid, val) =>
                {
                    capturedPid = pid;
                    capturedValue = val?.ToString();
                });

            //Act
            QAction.SetNewBearerTokenValue(mockProtocol.Object);

            //Assert
            Assert.AreEqual(Parameter.apikey_5, capturedPid, "Wrong parameter ID used.");
            Assert.AreEqual(expectedBearerToken, capturedValue, "Token was not built or set correctly.");
        }
    }
}