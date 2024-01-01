using CoreLibrary.Specification;
using CoreLibrary.UnitOfWork;
using FakeItEasy;
using Invest.Application.UserCases.FindAll.Queries;
using Invest.Domain.Portfolio.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.UserCases
{
	public class FindAllTests
    {
        const ushort LIMIT = 200;
        const ushort OFFSET = 0;

        [Fact]
        public async Task GetAllQuery_FilterEmpty_ReturnEmpty()
        {
            //Arrange
            var query = CreatePortfolioGetAllQuery();
            var stubUnitOfWork = A.Fake<IUnitOfWork>();
            var stubRepository = A.Fake<IRepository<Portfolio>>();
            var stubLogger = A.Fake<ILogger<PortfolioGetAllQueryHandler>>();

            A.CallTo(() => stubUnitOfWork.Repository<Portfolio>()).Returns(stubRepository);
            A.CallTo(() => stubRepository.Count(A<Expression<Func<Portfolio, bool>>>._)).Returns(0);
            A.CallTo(() => stubRepository.Find(A<ISpecification<Portfolio>>._)).Returns(null);

            var handler = new PortfolioGetAllQueryHandler(stubUnitOfWork, stubLogger);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.Equal(0, result.Total);
        }

        [Fact]
        public async Task GetAllQuery_FilterEmpty_ReturnOneResult()
        {
            //Arrange
            var query = CreatePortfolioGetAllQuery();
            var portfolios = new List<Portfolio> { new Portfolio() };

            var stubUnitOfWork = A.Fake<IUnitOfWork>();
            var stubRepository = A.Fake<IRepository<Portfolio>>();
            var stubLogger = A.Fake<ILogger<PortfolioGetAllQueryHandler>>();

            A.CallTo(() => stubUnitOfWork.Repository<Portfolio>()).Returns(stubRepository);
            A.CallTo(() => stubRepository.Count(A<Expression<Func<Portfolio, bool>>>._)).Returns(portfolios.Count);
            A.CallTo(() => stubRepository.Find(A<ISpecification<Portfolio>>._)).Returns(portfolios);

            var handler = new PortfolioGetAllQueryHandler(stubUnitOfWork, stubLogger);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.Equal(1, result.Total);
        }

        private static PortfolioGetAllQuery CreatePortfolioGetAllQuery()
        {
            var query = new PortfolioGetAllQuery()
            {
                Limit = LIMIT,
                Offset = OFFSET,
                Sort = string.Empty
            };
            return query;
        }
    }
}
