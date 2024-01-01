using CoreLibrary.Specification;
using System;

namespace Invest.Domain.Portfolio.Queries
{
    public class PortfolioGetSpecification : BaseSpecification<Models.Portfolio>
    {
        public PortfolioGetSpecification(long id, Guid uuid)
        {
            if (id == default)
                throw new ArgumentNullException(nameof(id));

            if(uuid == default)
                throw new ArgumentNullException(nameof(uuid));

            base.SetCriteria(x => x.Id == id && x.UserGuid == uuid);

            base.AddInclude("Stocks");
            base.AddInclude("Stocks.Currency");
            base.AddInclude("Stocks.StockType");
        }
    }
}
