using CoreLibrary.UnitOfWork;
using Invest.Application.Shared.DTOs;
using Invest.Domain.Portfolio.Models;
using Invest.Domain.Portfolio.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Invest.Application.UserCases.FindAll.Queries
{
    public class PortfolioGetAllQuery : IRequest<PageDto<Portfolio>>
    {
        public Guid Guid { get; set; }
        public ushort Offset { get; set; }
        public ushort Limit { get; set; }
        public string Sort { get; set; }
    }

    public class PortfolioGetAllQueryHandler : IRequestHandler<PortfolioGetAllQuery, PageDto<Portfolio>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<PortfolioGetAllQueryHandler> logger;

        public PortfolioGetAllQueryHandler(IUnitOfWork unitOfWork, ILogger<PortfolioGetAllQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public Task<PageDto<Portfolio>> Handle(PortfolioGetAllQuery req, CancellationToken cancellationToken)
        {
            this.logger.LogDebug("call handle PortfolioGetAllQueryHandler.");

            var result = new PageDto<Portfolio>();
            var spec = new PortfolioPaginatedSpecification(req.Guid, req.Offset, req.Limit);

            result.Total = this.unitOfWork.Repository<Portfolio>().Count(spec.Criteria);
            result.Limit = req.Limit;
            result.Offset = req.Offset;
            result.Items = this.unitOfWork.Repository<Portfolio>().Find(spec);

            return Task.FromResult(result);
        }
    }
}
