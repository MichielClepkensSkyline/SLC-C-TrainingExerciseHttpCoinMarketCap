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
        public void PollLatestListingsTest()
        {
            //Arrange
            var mockProtocol = new Mock<SLProtocol>();
            mockProtocol.Setup(p => p.GetParameter(Parameter.httpresponsecodelatestlistings_3)).Returns("HTTP/1.1 200 OK");

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
            mockProtocol.Setup(p => p.GetParameter(Parameter.jsonresponselatestlistings_4)).Returns(validJson);

            List<object[]> filledData = null;
            mockProtocol.Setup(p => p.FillArray(Parameter.Latestlistings.tablePid, It.IsAny<List<object[]>>(), NotifyProtocol.SaveOption.Full))
                        .Callback<int, List<object[]>, NotifyProtocol.SaveOption>((pid, data, opt) =>
                        {
                            filledData = data;
                        });
            var mockHelper = new Mock<HelperMethods>();
            mockHelper.Setup(h => h.CheckStatusCode(It.IsAny<object>(), It.IsAny<SLProtocol>())).Returns(true);
            mockHelper.Setup(h => h.CheckJSONResponseStatus(It.IsAny<Status>(), It.IsAny<SLProtocol>())).Returns(true);

            //Act
            QAction.PollLatestListings(mockProtocol.Object);

            //Assert
            Assert.IsNotNull(filledData, "FillArray was not called");
            Assert.AreEqual(1, filledData.Count);
        }
    }
}