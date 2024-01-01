using IntegrationTests.Setup;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Controllers.V1
{
	public class PortfoliosControllerTests : ScenarioBase
    {
        private const string apiVersion = "api/v1/";

		[Fact]
		public async Task GetOne_Exists_ReturnOk()
		{
			//Arrange
			using var server = CreateServer();

			//Act
			var response = await server.CreateClient().GetAsync(apiVersion + Get.Portfolios + 1);

			//Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Theory]
		[InlineData(999)]
		[InlineData(9999)]
		public async Task GetOne_NotExists_ReturnNotFound(int portfolioId)
		{
			//Arrange
			using var server = CreateServer();

			//Act
			var response = await server.CreateClient().GetAsync(apiVersion + Get.Portfolios + portfolioId);

			//Assert
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetOne_InvalidValues_ReturnBadRequest(int portfolioId)
        {
            //Arrange
            using var server = CreateServer();

            //Act
            var response = await server.CreateClient().GetAsync(apiVersion + Get.Portfolios + portfolioId);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
	}
}
