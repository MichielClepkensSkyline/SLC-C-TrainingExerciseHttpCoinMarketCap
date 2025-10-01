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
        public void SetGetCategoryURLTest()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();

            string testRowKey = "123abc";
            string expectedUrl = $"api/custom/coinmarketcap?content=category&id={testRowKey}";

            mockProtocol.Setup(p => p.RowKey()).Returns(testRowKey);

            int capturedPid = 0;
            string capturedValue = null;

            mockProtocol.Setup(p => p.SetParameter(It.IsAny<int>(), It.IsAny<object>()))
                .Callback<int, object>((pid, val) =>
                {
                    capturedPid = pid;
                    capturedValue = val?.ToString();
                });

            // Act
            QAction.SetGetCategoryURL(mockProtocol.Object);

            // Assert
            Assert.AreEqual(Parameter.refreshcategoryurl_516, capturedPid, "Wrong parameter ID used.");
            Assert.AreEqual(expectedUrl, capturedValue, "URL was not built or set correctly.");
        }
    }
}