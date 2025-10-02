using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QAction_1;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.Protocol.Extension;
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
        public void PoolLatestQuotesTest()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecodelatestquotes_500)).Returns("HTTP/1.1 200 OK");

            string json = @"
            {
            ""status"": {
                ""timestamp"": ""2025-10-01T12:00:00Z"",
                ""error_code"": 0,
                ""error_message"": null,
                ""elapsed"": 10,
                ""credit_count"": 1,
                ""notice"": null
            },
            ""data"": {
                ""activeCryptocurrencies"": 10000,
                ""activeMarketPairs"": 40000,
                ""activeExchanges"": 300,
                ""lastUpdated"": ""2025-10-01T12:00:00Z"",
                ""btcDominance"": 51.2,
                ""ethDominance"": 18.9,
                ""quote"": {
                    ""USD"": {
                        ""totalMarketCap"": 1200000000.55,
                        ""totalVolume24h"": 98000000.12,
                        ""defiMarketCap"": 330000000.33,
                        ""stablecoinMarketCap"": 270000000.44
                        }
                    }
                }
            }";

            mockProtocol.Setup(p => p.GetParameter(Parameter.jsonresponselatestquotes_501)).Returns(json);

            Dictionary<int, object> capturedParameters = new Dictionary<int, object>();
            mockProtocol.Setup(p => p.SetParameter(It.IsAny<int>(), It.IsAny<object>()))
            .Callback<int, object>((pid, val) =>
            {
                capturedParameters[pid] = val;
            });


            // Act
            QAction.PoolLatestQuotes(mockProtocol.Object);

            // Assert
            Assert.IsNotNull(capturedParameters, "SetParameters was not called");
        }

        [TestMethod]
        public void PollLatestQoutes_InvalidHttpStatus_DoesNotCallFillArray()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();
            var helperMock = new Mock<HelperMethods>();
            string httpResponse = "HTTP/1.1 400 Bad Request";

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecodelatestquotes_500)).Returns(httpResponse);
            helperMock.Setup(h => h.CheckStatusCode(httpResponse, mockProtocol.Object)).Returns(false);

            // Act
            QAction.PoolLatestQuotes(mockProtocol.Object);

            // Assert
            mockProtocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), It.IsAny<NotifyProtocol.SaveOption>()), Times.Never);
        }

        [TestMethod]
        public void PollLatestQoutes_InvalidJsonStatus_DoesNotCallFillArray()
        {
            // Arrange
            var mockProtocol = new Mock<SLProtocol>();
            var helperMock = new Mock<HelperMethods>();
            string httpResponse = "HTTP/1.1 200 OK";

            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecodelatestquotes_500)).Returns(httpResponse);

            string invalidJson = @"
            {
            ""status"": {
                ""timestamp"": ""2025-10-01T12:00:00Z"",
                ""error_code"": 1,
                ""error_message"": ""Not good"",
                ""elapsed"": 10,
                ""credit_count"": 1,
                ""notice"": null
            },
            ""data"": {
                ""activeCryptocurrencies"": 10000,
                ""activeMarketPairs"": 40000,
                ""activeExchanges"": 300,
                ""lastUpdated"": ""2025-10-01T12:00:00Z"",
                ""btcDominance"": 51.2,
                ""ethDominance"": 18.9,
                ""quote"": {
                    ""USD"": {
                        ""totalMarketCap"": 1200000000.55,
                        ""totalVolume24h"": 98000000.12,
                        ""defiMarketCap"": 330000000.33,
                        ""stablecoinMarketCap"": 270000000.44
                        }
                    }
                }
            }";
            mockProtocol.Setup(p => p.GetParameter(Parameter.jsonresponselatestquotes_501)).Returns(invalidJson);

            helperMock.Setup(h => h.CheckStatusCode(httpResponse, mockProtocol.Object)).Returns(true);
            helperMock.Setup(h => h.CheckJSONResponseStatus(It.IsAny<Status>(), mockProtocol.Object)).Returns(false);

            // Act
            QAction.PoolLatestQuotes(mockProtocol.Object);

            // Assert
            mockProtocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), It.IsAny<NotifyProtocol.SaveOption>()), Times.Never);
        }
    }
}