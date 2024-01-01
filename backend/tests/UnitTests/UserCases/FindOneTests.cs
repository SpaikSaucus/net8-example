using CoreLibrary.Specification;
using CoreLibrary.UnitOfWork;
using FakeItEasy;
using Invest.Application.UserCases.FindOne.Queries;
using Invest.Domain.Portfolio.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.UserCases
{
	public class FindOneTests
	{
		private static readonly long id = 1;
		private static readonly Guid uuid = Guid.NewGuid();

		[Fact]
		public async Task GetOneQuery_GuidExists_ReturnResult()
		{
			//Arrange
			var query = CreatePortfolioGetQuery();
			var portfolios = new List<Portfolio> { new Portfolio() };

			var stubUnitOfWork = A.Fake<IUnitOfWork>();
			var stubRepository = A.Fake<IRepository<Portfolio>>();
			var stubLogger = A.Fake<ILogger<PortfolioGetQueryHandler>>();

			A.CallTo(() => stubUnitOfWork.Repository<Portfolio>()).Returns(stubRepository);
			A.CallTo(() => stubRepository.Find(A<ISpecification<Portfolio>>._)).Returns(portfolios);

			var handler = new PortfolioGetQueryHandler(stubUnitOfWork, stubLogger);

			//Act
			var result = await handler.Handle(query, CancellationToken.None);

			//Assert
			Assert.NotNull(result);
		}

		[Fact]
		public async Task GetOneQuery_GuidNotExists_ReturnEmpty()
		{
			//Arrange
			var query = CreatePortfolioGetQuery();
			var portfolios = new List<Portfolio>();

			var stubUnitOfWork = A.Fake<IUnitOfWork>();
			var stubRepository = A.Fake<IRepository<Portfolio>>();
			var stubLogger = A.Fake<ILogger<PortfolioGetQueryHandler>>();

			A.CallTo(() => stubUnitOfWork.Repository<Portfolio>()).Returns(stubRepository);
			A.CallTo(() => stubRepository.Find(A<ISpecification<Portfolio>>._)).Returns(portfolios);

			var handler = new PortfolioGetQueryHandler(stubUnitOfWork, stubLogger);

			//Act
			var result = await handler.Handle(query, CancellationToken.None);

			//Assert
			Assert.Null(result);
		}

		private static PortfolioGetQuery CreatePortfolioGetQuery()
		{
			return new PortfolioGetQuery() { Id = id, UserGuid = uuid };
		}
	}
}
