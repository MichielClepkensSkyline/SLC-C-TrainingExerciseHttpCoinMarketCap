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
        public void PollCategoriesTest()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();
            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecodecategories_300)).Returns("HTTP/1.1 200 OK");

            string validJson = @"
            {
                ""status"": {
                    ""timestamp"": ""2025-10-01T11:29:27.9980015Z"",
                    ""error_code"": 0,
                    ""error_message"": null,
                    ""elapsed"": 6,
                    ""credit_count"": 1,
                    ""notice"": null
                    },
                ""data"": [
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
                ]
            }";

            mockProtocol.Setup(p => p.GetParameter(Parameter.jsonresponsecategories_301)).Returns(validJson);

            List<object[]> filledData = null;
            mockProtocol.Setup(p => p.FillArray(Parameter.Categories.tablePid, It.IsAny<List<object[]>>(), NotifyProtocol.SaveOption.Full))
                        .Callback<int, List<object[]>, NotifyProtocol.SaveOption>((pid, data, opt) =>
                        {
                            filledData = data;
                        });
            var mockHelper = new Mock<HelperMethods>();
            mockHelper.Setup(h => h.CheckStatusCode(It.IsAny<object>(), It.IsAny<SLProtocol>())).Returns(true);
            mockHelper.Setup(h => h.CheckJSONResponseStatus(It.IsAny<Status>(), It.IsAny<SLProtocol>())).Returns(true);

            // Act
            QAction.PollCategories(mockProtocol.Object);

            // Assert
            Assert.IsNotNull(filledData, "FillArray was not called");
            Assert.AreEqual(1, filledData.Count);
        }

        [TestMethod]
        public void PollCategories_InvalidHttpStatus_DoesNotCallFillArray()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();
            var helperMock = new Mock<HelperMethods>();
            string httpResponse = "HTTP/1.1 400 Bad Request";

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecodecategories_300)).Returns(httpResponse);
            helperMock.Setup(h => h.CheckStatusCode(httpResponse, mockProtocol.Object)).Returns(false);

            // Act
            QAction.PollCategories(mockProtocol.Object);

            // Assert
            mockProtocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), It.IsAny<NotifyProtocol.SaveOption>()), Times.Never);
        }

        [TestMethod]
        public void PollCategories_InvalidJsonStatus_DoesNotCallFillArray()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();
            var helperMock = new Mock<HelperMethods>();
            string httpResponse = "HTTP/1.1 200 OK";

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecodecategories_300)).Returns(httpResponse);

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
                ""data"": [
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
                ]
            }";
            mockProtocol.Setup(p => p.GetParameter(Parameter.jsonresponsecategories_301)).Returns(invalidJson);

            helperMock.Setup(h => h.CheckStatusCode(httpResponse, mockProtocol.Object)).Returns(true);
            helperMock.Setup(h => h.CheckJSONResponseStatus(It.IsAny<Status>(), mockProtocol.Object)).Returns(false);

            // Act
            QAction.PollCategories(mockProtocol.Object);

            // Assert
            mockProtocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), It.IsAny<NotifyProtocol.SaveOption>()), Times.Never);
        }

        [TestMethod]
        public void PollCategories_InvalidData_DoesNotCallFillArray()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();
            var helperMock = new Mock<HelperMethods>();
            string httpResponse = "HTTP/1.1 200 OK";

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecodecategories_300)).Returns(httpResponse);

            string invalidJson = @"{
                ""status"": {
                    ""timestamp"": ""2025-10-01T12:00:00Z"",
                    ""error_code"": 0,
                    ""error_message"": null,
                    ""elapsed"": 10,
                    ""credit_count"": 1,
                    ""notice"": null
                },
                ""data"": []
            }";

            mockProtocol.Setup(p => p.GetParameter(Parameter.jsonresponsecategories_301)).Returns(invalidJson);

            helperMock.Setup(h => h.CheckStatusCode(httpResponse, mockProtocol.Object)).Returns(true);
            helperMock.Setup(h => h.CheckJSONResponseStatus(It.IsAny<Status>(), mockProtocol.Object)).Returns(false);

            // Act
            QAction.PollCategories(mockProtocol.Object);

            // Assert
            mockProtocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), It.IsAny<NotifyProtocol.SaveOption>()), Times.Never);
        }
    }
}