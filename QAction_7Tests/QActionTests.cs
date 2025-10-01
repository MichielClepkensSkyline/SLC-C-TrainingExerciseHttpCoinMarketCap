namespace Tests
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Skyline.DataMiner.Scripting;
	using Skyline.DataMiner.Scripting.Category;

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
		public void RunTest_ShouldReturn_StatusCheckFails()
		{
			protocol.Setup(p => p.GetParameter(Parameter.statuscodeindividualcategory_9)).Returns("HTTP/1.1 500 Internal Server Error");

			QAction.Run(protocol.Object);

			protocol.Verify(p => p.GetParameter(Parameter.responseindividualcategory_10), Times.Never);
			protocol.Verify(p => p.SetRow(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<object[]>()), Times.Never);
		}

		[TestMethod]
		public void RunTest_ShouldReturn_ErrorCheckFails()
		{
			protocol.Setup(p => p.GetParameter(Parameter.statuscodelatestquote_7)).Returns("HTTP/1.1 500 Internal Server Error");

			var category = new Category
			{
				Status = new Status
				{
					ErrorCode = 1,
					ErrorMessage = "Error message",
				},
			};

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(category);

			protocol.Setup(p => p.GetParameter(Parameter.responseindividualcategory_10)).Returns(json);

			QAction.Run(protocol.Object);

			protocol.Verify(p => p.SetRow(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<object[]>()), Times.Never);
		}

		[TestMethod]
		public void RunTest_ShouldUpdateRow_ValidCategory()
		{
			protocol.Setup(p => p.GetParameter(Parameter.statuscodeindividualcategory_9)).Returns("HTTP/1.1 200 OK");

			var category = new Category
			{
				Status = new Status
				{
					ErrorCode = 0,
					ErrorMessage = null,
				},
				Data = new Data
				{
					Id = "cat123",
					Name = "Test Category",
					NumTokens = 5,
					AvgPriceChange = 1.23,
					MarketCap = 1000000,
					MarketCapChange = 0.12,
					Volume = 50000,
					VolumeChange = 0.05,
					LastUpdated = DateTime.Now,
				},
			};

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(category);

			protocol.Setup(p => p.GetParameter(Parameter.responseindividualcategory_10)).Returns(json);

			QAction.Run(protocol.Object);

			protocol.Verify(
				p => p.SetRow(
				Parameter.Categories.tablePid,
				category.Data.Id,
				It.Is<object[]>(row =>
					(string)row[0] == category.Data.Id &&
					(string)row[1] == category.Data.Name &&
					(int)row[2] == category.Data.NumTokens &&
					(double)row[3] == category.Data.AvgPriceChange &&
					(double)row[4] == category.Data.MarketCap &&
					(double)row[5] == category.Data.MarketCapChange &&
					(double)row[6] == category.Data.Volume &&
					(double)row[7] == category.Data.VolumeChange &&
					(double)row[8] == category.Data.LastUpdated.ToOADate())), Times.Once);
		}

		[TestMethod]
		public void RunTest_ShouldUpdateRow_ValidCategoryWithNullName()
		{
			protocol.Setup(p => p.GetParameter(Parameter.statuscodeindividualcategory_9)).Returns("HTTP/1.1 200 OK");

			var category = new Category
			{
				Status = new Status
				{
					ErrorCode = 0,
					ErrorMessage = null,
				},
				Data = new Data
				{
					Id = "cat123",
					Name = null,
					NumTokens = 5,
					AvgPriceChange = 1.23,
					MarketCap = 1000000,
					MarketCapChange = 0.12,
					Volume = 50000,
					VolumeChange = 0.05,
					LastUpdated = DateTime.Now,
				},
			};

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(category);

			protocol.Setup(p => p.GetParameter(Parameter.responseindividualcategory_10)).Returns(json);

			QAction.Run(protocol.Object);

			protocol.Verify(
				p => p.SetRow(
				Parameter.Categories.tablePid,
				category.Data.Id,
				It.Is<object[]>(row =>
					(string)row[0] == category.Data.Id &&
					(string)row[1] == "-100" &&
					(int)row[2] == category.Data.NumTokens &&
					(double)row[3] == category.Data.AvgPriceChange &&
					(double)row[4] == category.Data.MarketCap &&
					(double)row[5] == category.Data.MarketCapChange &&
					(double)row[6] == category.Data.Volume &&
					(double)row[7] == category.Data.VolumeChange &&
					(double)row[8] == category.Data.LastUpdated.ToOADate())), Times.Once);
		}
	}
}