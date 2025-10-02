using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QAction_1;
using Skyline.DataMiner.Scripting;
using Skyline.Protocol.MyExtension;
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
        public void GetCategoryRowTest()
        {
            // Arrange
            Mock<SLProtocol> mockProtocol = new Mock<SLProtocol>();

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecategoryrefresh_518)).Returns("HTTP/1.1 200 OK");

            string json = @"
            {
                ""status"": {
                    ""timestamp"": ""2025-10-01T14:31:51.9542069Z"",
                    ""error_code"": 0,
                    ""error_message"": null,
                    ""elapsed"": 718,
                    ""credit_count"": 1,
                    ""notice"": null
                    },
                ""data"": {
                    ""id"": ""64c7867acad87e003b856825"",
                    ""name"": ""Base Ecosystem"",
                    ""title"": ""Base Ecosystem"",
                    ""description"": ""Base Ecosystem"",
                    ""num_tokens"": 719,
                    ""avg_price_change"": -5.03898527201141,
                    ""market_cap"": 54221249522.403992,
                    ""market_cap_change"": -6.094145525531,
                    ""volume"": 5987420459.3256664,
                    ""volume_change"": -27.627310795737,
                    ""last_updated"": ""2025-10-01T13:31:51.9542069Z""
                    }
            }";

            mockProtocol.Setup(p => p.GetParameter(Parameter.jsonrsponserefreshcategory_517)).Returns(json);

            int categoriesTablePid = 0;
            string primaryKey = null;
            object[] capturedRow = null;

            mockProtocol.Setup(p => p.SetRow(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<object>()))
                .Callback<int, string, object>((pid, key, row) =>
                {
                    categoriesTablePid=pid;
                    primaryKey=key;
                    capturedRow= row as object[];
                });

            string expectedPrimaryKey = "64c7867acad87e003b856825";
            var expectedRow = new CategoriesQActionRow
            {
                Categoriesid_401 = "64c7867acad87e003b856825",
                Categoriestitle_402 = "Base Ecosystem",
                Categoriesnumberoftokens_403 = 719,
                Categoriesavreagepricechange_404 = -5.03898527201141,
                Categoriesmarketcap_405 = 54221249522.403992,
                Categoriesvolume_406 = 5987420459.3256664,
                Categorieslastupdated_407 = DateTime.Parse("2025-10-01T13:31:51.9542069Z").ToUniversalTime().ToOADate()

            }.ToObjectArray();

            // Act
            QAction.GetCategoryRow(mockProtocol.Object);

            // Assert
            Assert.AreEqual(Parameter.Categories.tablePid, categoriesTablePid);
            Assert.AreEqual(expectedPrimaryKey, primaryKey);
            CollectionAssert.AreEqual(expectedRow, capturedRow, "SetRow row data mismatch.");
        }

        [TestMethod]
        public void GetCategoryRow_InvalidHttpStatus_DoesNotCallFillArray()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();
            var helperMock = new Mock<HelperMethods>();
            string httpResponse = "HTTP/1.1 400 Bad Request";

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecategoryrefresh_518)).Returns(httpResponse);
            helperMock.Setup(h => h.CheckStatusCode(httpResponse, mockProtocol.Object)).Returns(false);

            // Act
            QAction.GetCategoryRow(mockProtocol.Object);

            // Assert
            mockProtocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), It.IsAny<NotifyProtocol.SaveOption>()), Times.Never);
        }

        [TestMethod]
        public void GetCategoryRow_InvalidJsonStatus_DoesNotCallFillArray()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();
            var helperMock = new Mock<HelperMethods>();
            string httpResponse = "HTTP/1.1 200 OK";

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecategoryrefresh_518)).Returns(httpResponse);

            string invalidJson = @"
            {
                ""status"": {
                    ""timestamp"": ""2025-10-01T11:29:27.9980015Z"",
                    ""error_code"": 1,
                    ""error_message"": ""Not good"",
                    ""elapsed"": 6,
                    ""credit_count"": 1,
                    ""notice"": null
                    },
                ""data"": 
                 {
                    ""id"": ""6823f463f4035758156a501c"",
                    ""name"": ""Internet Capital Markets"",
                    ""title"": ""Internet Capital Markets"",
                    ""description"": ""Internet Capital Markets"",
                    ""num_tokens"": 8,
                    ""avg_price_change"": -19.930721654598546,
                    ""market_cap"": 319300362.08430588,
                    ""market_cap_change"": -18.342860046142,
                    ""volume"": 490986489.37452543,
                    ""volume_change"": -0.565025088224,
                    ""last_updated"": ""2025-10-01T10:29:27.9980015Z""
                }
            }";
            mockProtocol.Setup(p => p.GetParameter(Parameter.jsonrsponserefreshcategory_517)).Returns(invalidJson);

            helperMock.Setup(h => h.CheckStatusCode(httpResponse, mockProtocol.Object)).Returns(true);
            helperMock.Setup(h => h.CheckJSONResponseStatus(It.IsAny<Status>(), mockProtocol.Object)).Returns(false);

            // Act
            QAction.GetCategoryRow(mockProtocol.Object);

            // Assert
            mockProtocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), It.IsAny<NotifyProtocol.SaveOption>()), Times.Never);
        }

        [TestMethod]
        public void GetCategoryRow_InvalidData_DoesNotCallFillArray()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();
            var helperMock = new Mock<HelperMethods>();
            string httpResponse = "HTTP/1.1 200 OK";

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecategoryrefresh_518)).Returns(httpResponse);

            string invalidJson = @"{
                ""status"": {
                    ""timestamp"": ""2025-10-01T12:00:00Z"",
                    ""error_code"": 0,
                    ""error_message"": null,
                    ""elapsed"": 10,
                    ""credit_count"": 1,
                    ""notice"": null
                },
                ""data"": {}
            }";

            mockProtocol.Setup(p => p.GetParameter(Parameter.jsonrsponserefreshcategory_517)).Returns(invalidJson);

            helperMock.Setup(h => h.CheckStatusCode(httpResponse, mockProtocol.Object)).Returns(true);
            helperMock.Setup(h => h.CheckJSONResponseStatus(It.IsAny<Status>(), mockProtocol.Object)).Returns(false);

            // Act
            QAction.GetCategoryRow(mockProtocol.Object);

            // Assert
            mockProtocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), It.IsAny<NotifyProtocol.SaveOption>()), Times.Never);
        }
    }
}