using CoreLibrary.Specification;
using System;

namespace Invest.Domain.Portfolio.Queries
{
    public class PortfolioPaginatedSpecification : BaseSpecification<Models.Portfolio>
    {
        public PortfolioPaginatedSpecification(Guid guid, int skip, int take)
        {
            base.SetCriteria(x => x.UserGuid == guid);
            base.ApplyPaging(skip, take);
        }
    }
}
