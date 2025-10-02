namespace Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using QAction_1;
    using Skyline.DataMiner.Scripting;
    using Skyline.Protocol.MyExtension;

    [TestClass()]
    public class QActionTests
    {
        public Mock<SLProtocol> mockProtocol = new Mock<SLProtocol>();
        [TestMethod]
        public void PollLatestListingsTest()
        {
            // Arrange
            this.mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecodelatestlistings_3)).Returns("HTTP/1.1 200 OK");

            string validJson = @"
            {
                ""status"": {
                    ""timestamp"": ""2025-10-01T07:59:19.8730715Z"",
                    ""error_code"": 0,
                    ""error_message"": null,
                    ""elapsed"": 12,
                    ""credit_count"": 1,
                    ""notice"": null,
                    ""total_count"": 9783
            },
                ""data"": [
                {
                    ""id"": ""btc"",
                    ""name"": ""Bitcoin"",
                    ""symbol"": ""BTC"",
                    ""cmc_rank"": 1,
                    ""circulating_supply"": 19000000,
                    ""last_updated"": ""2025-10-01T08:00:00Z"",
                    ""quote"": {
                    ""USD"": {
                        ""price"": 27000.50,
                        ""market_cap"": 500000000,
                        ""percent_change_1h"": 0.5,
                        ""volume_change_24h"": 2.3
                            }
                    }
                 }
                 ]
            }";
            this.mockProtocol.Setup(p => p.GetParameter(Parameter.jsonresponselatestlistings_4)).Returns(validJson);

            List<object[]> filledData = null;
            this.mockProtocol.Setup(p => p.FillArray(Parameter.Latestlistings.tablePid, It.IsAny<List<object[]>>(), NotifyProtocol.SaveOption.Full))
                        .Callback<int, List<object[]>, NotifyProtocol.SaveOption>((pid, data, opt) =>
                        {
                            filledData = data;
                        });
            var mockHelper = new Mock<HelperMethods>();
            mockHelper.Setup(h => h.CheckStatusCode(It.IsAny<object>(), It.IsAny<SLProtocol>())).Returns(true);
            mockHelper.Setup(h => h.CheckJSONResponseStatus(It.IsAny<Status>(), It.IsAny<SLProtocol>())).Returns(true);

            // Act
            QAction.PollLatestListings(mockProtocol.Object);

            // Assert
            Assert.IsNotNull(filledData, "FillArray was not called");
            Assert.AreEqual(1, filledData.Count);
        }

        [TestMethod]
        public void PollLatestListings_InvalidHttpStatus_DoesNotCallFillArray()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();
            var helperMock = new Mock<HelperMethods>();
            string httpResponse = "HTTP/1.1 400 Bad Request";

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecodelatestlistings_3)).Returns(httpResponse);
            helperMock.Setup(h => h.CheckStatusCode(httpResponse, mockProtocol.Object)).Returns(false);

            // Act
            QAction.PollLatestListings(mockProtocol.Object);

            // Assert
            mockProtocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), It.IsAny<NotifyProtocol.SaveOption>()), Times.Never);
        }

        [TestMethod]
        public void PollLatestListings_InvalidJsonStatus_DoesNotCallFillArray()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();
            var helperMock = new Mock<HelperMethods>();
            string httpResponse = "HTTP/1.1 200 OK";

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecodelatestlistings_3)).Returns(httpResponse);

            string invalidJson = @"{
                   ""status"": {
                        ""timestamp"": ""2025-10-01T12:00:00Z"",
                        ""error_code"": 1,
                        ""error_message"": ""Not Good"",
                        ""elapsed"": 10,
                        ""credit_count"": 1,
                        ""notice"": null
                    },
                    ""data"": [
{
                    ""id"": ""btc"",
                    ""name"": ""Bitcoin"",
                    ""symbol"": ""BTC"",
                    ""cmc_rank"": 1,
                    ""circulating_supply"": 19000000,
                    ""last_updated"": ""2025-10-01T08:00:00Z"",
                    ""quote"": {
                    ""USD"": {
                        ""price"": 27000.50,
                        ""market_cap"": 500000000,
                        ""percent_change_1h"": 0.5,
                        ""volume_change_24h"": 2.3
                            }
                    }
                 }]
            }";
            mockProtocol.Setup(p => p.GetParameter(Parameter.jsonresponselatestlistings_4)).Returns(invalidJson);

            helperMock.Setup(h => h.CheckStatusCode(httpResponse, mockProtocol.Object)).Returns(true);
            helperMock.Setup(h => h.CheckJSONResponseStatus(It.IsAny<Status>(), mockProtocol.Object)).Returns(false);

            // Act
            QAction.PollLatestListings(mockProtocol.Object);

            // Assert
            mockProtocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), It.IsAny<NotifyProtocol.SaveOption>()), Times.Never);
        }

        [TestMethod]
        public void PollLatestListings_InvalidData_DoesNotCallFillArray()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();
            var helperMock = new Mock<HelperMethods>();
            string httpResponse = "HTTP/1.1 200 OK";

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecodelatestlistings_3)).Returns(httpResponse);

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

            mockProtocol.Setup(p => p.GetParameter(Parameter.jsonresponselatestlistings_4)).Returns(invalidJson);

            helperMock.Setup(h => h.CheckStatusCode(httpResponse, mockProtocol.Object)).Returns(true);
            helperMock.Setup(h => h.CheckJSONResponseStatus(It.IsAny<Status>(), mockProtocol.Object)).Returns(false);

            // Act
            QAction.PollLatestListings(mockProtocol.Object);

            // Assert
            mockProtocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), It.IsAny<NotifyProtocol.SaveOption>()), Times.Never);
        }
    }
}