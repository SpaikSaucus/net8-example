using CoreLibrary.Specification;
using System;

namespace Invest.Domain.Portfolio.Queries
{
    public class PortfolioGetSpecificationV1 : BaseSpecification<Models.Portfolio>
    {
        public PortfolioGetSpecificationV1(long id)
        {
            if (id == default)
                throw new ArgumentNullException(nameof(id));

            base.SetCriteria(x => x.Id == id);

            base.AddInclude("Stocks");
            base.AddInclude("Stocks.Currency");
            base.AddInclude("Stocks.StockType");
        }
    }
}
