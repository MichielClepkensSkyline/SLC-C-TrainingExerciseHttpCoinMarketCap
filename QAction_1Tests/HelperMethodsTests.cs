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

namespace Skyline.Protocol.MyExtension.Tests
{

    [TestClass()]
    public class HelperMethodsTests
    {
      Mock<SLProtocol> mock = new Mock<SLProtocol>();

        [TestMethod()]
        public void CheckStatusCodeTestTrueResponse()
        {
            //Arrange
            var helpermethods = new HelperMethods();
            string input = "HTTP/1.1 200 OK";

            //Act
            bool isValid = helpermethods.CheckStatusCode(input, mock.Object);
            
            //Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod()]
        public void CheckStatusCodeTestFalseResponse()
        {
            //Arrange
            var helpermethods = new HelperMethods();
            string input = "HTTP/1.1 400 Bad Request";

            //Act
            bool isValid = helpermethods.CheckStatusCode(input, mock.Object);

            //Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod()]
        public void CheckJSONResponseStatusTestTrueResponse()
        {
            //Arrange
            var helpermethods = new HelperMethods();
            string json = "{\"status\":{\"timestamp\":\"2025-10-01T07:59:19.8730715Z\",\"error_code\":0,\"error_message\":null,\"elapsed\":12,\"credit_count\":1,\"notice\":null,\"total_count\":9783}}";
            LatestListings deserializedStatus = Newtonsoft.Json.JsonConvert.DeserializeObject<LatestListings>(json);
            //Act
            bool isValid = helpermethods.CheckJSONResponseStatus(deserializedStatus.Status, mock.Object);

            //Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod()]
        public void CheckJSONResponseStatusTestFalseResponse()
        {
            //Arrange
            var helpermethods = new HelperMethods();
            string jsonFail = "{\"status\":{\"timestamp\":\"2025-10-01T07:59:19.8730715Z\",\"error_code\":1,\"error_message\":\"Not Good\",\"elapsed\":12,\"credit_count\":1,\"notice\":null,\"total_count\":9783}}";
            LatestListings deserializedStatusFail = Newtonsoft.Json.JsonConvert.DeserializeObject<LatestListings>(jsonFail);
            //Act
            bool isValid = helpermethods.CheckJSONResponseStatus(deserializedStatusFail.Status, mock.Object);

            //Assert
            Assert.IsFalse(isValid);
        }
    }
}