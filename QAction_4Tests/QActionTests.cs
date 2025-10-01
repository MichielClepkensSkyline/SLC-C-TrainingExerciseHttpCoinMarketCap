namespace Tests
{
	using System;
	using System.Collections.Generic;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Skyline.DataMiner.Scripting;
	using Skyline.DataMiner.Scripting.Categories;

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
			protocol.Setup(p => p.GetParameter(Parameter.statuscodecategories_5)).Returns("HTTP/1.1 500 Internal Server Error");

			QAction.Run(protocol.Object);

			protocol.Verify(p => p.GetParameter(Parameter.responsecategories_6), Times.Never);
			protocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<object[]>()), Times.Never);
		}

		[TestMethod]
		public void RunTest_ShouldReturn_ErrorCheckFails()
		{
			protocol.Setup(p => p.GetParameter(Parameter.statuscodecategories_5)).Returns("HTTP/1.1 500 Internal Server Error");

			var latestListing = new Categories
			{
				Status = new Status
				{
					ErrorCode = 1,
					ErrorMessage = "Error message",
				},
			};

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(latestListing);

			protocol.Setup(p => p.GetParameter(Parameter.responsecategories_6)).Returns(json);

			QAction.Run(protocol.Object);

			protocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<object[]>()), Times.Never);
		}

		[TestMethod]
		public void RunTest_ShouldFillAray_InvalidInputIdNull()
		{
			protocol.Setup(p => p.GetParameter(Parameter.statuscodecategories_5)).Returns("HTTP/1.1 200 OK");

			var categories = new Categories
			{
				Status = new Status
				{
					ErrorCode = 0,
					ErrorMessage = null,
				},
				CategoryList = new List<Category>
				{
					new Category
					{
						Id = null,
						Name = "Internet Capital Markets",
						Title = "Internet Capital Markets",
						Description = "Internet Capital Markets",
						NumTokens = 8,
						AvgPriceChange = -19.930721654598546,
						MarketCap = 319300362.08430588,
						MarketCapChange = -18.342860046142,
						Volume = 490986489.37452543,
						VolumeChange = -0.565025088224,
						LastUpdated = DateTime.Parse("2025-10-01T09:44:30.1890849Z"),
					},
				},
			};

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(categories);

			protocol.Setup(p => p.GetParameter(Parameter.responsecategories_6)).Returns(json);

			QAction.Run(protocol.Object);

			protocol.Verify(p => p.Log(It.IsAny<string>(), It.IsAny<LogType>(), It.IsAny<LogLevel>()));
			protocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), NotifyProtocol.SaveOption.Full), Times.Never);
		}

		[TestMethod]
		public void RunTest_ShouldFillAray_ValidInputs()
		{
			protocol.Setup(p => p.GetParameter(Parameter.statuscodecategories_5)).Returns("HTTP/1.1 200 OK");

			var categories = new Categories
			{
				Status = new Status
				{
					ErrorCode = 0,
					ErrorMessage = null,
				},
				CategoryList = new List<Category>
		{
			new Category
			{
				Id = "6823f463f4035758156a501c",
				Name = "Internet Capital Markets",
				Title = "Internet Capital Markets",
				Description = "Internet Capital Markets",
				NumTokens = 8,
				AvgPriceChange = -19.930721654598546,
				MarketCap = 319300362.08430588,
				MarketCapChange = -18.342860046142,
				Volume = 490986489.37452543,
				VolumeChange = -0.565025088224,
				LastUpdated = DateTime.Parse("2025-10-01T09:44:30.1890849Z"),
			},
		},
			};

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(categories);

			protocol.Setup(p => p.GetParameter(Parameter.responsecategories_6)).Returns(json);

			QAction.Run(protocol.Object);

			protocol.Verify(p => p.Log(It.IsAny<string>(), It.IsAny<LogType>(), It.IsAny<LogLevel>()), Times.Never);
			protocol.Verify(p => p.FillArray(It.IsAny<int>(), It.IsAny<List<object[]>>(), NotifyProtocol.SaveOption.Full), Times.Once);
		}
	}
}