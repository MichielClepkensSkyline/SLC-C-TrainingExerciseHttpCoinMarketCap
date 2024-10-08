using Moq;

using QAction_2.Dtos.CategoriesResponse;
using Skyline.DataMiner.Scripting;

namespace QAction_2.Tests
{
	[TestClass()]
	public class CryptoCategoriesProcessorTests
	{
		[TestMethod()]
		public static SingleCategoryResponseDto GetAndDeserializeSingleCategoryResponseTest(SLProtocolExt protocol)
		{
			// Arrange
			var fakeProtocol = new Mock<SLProtocol>();
			fakeProtocol.Setup()

			// Act

			// Assert
			Assert.Fail();
		}
	}
}