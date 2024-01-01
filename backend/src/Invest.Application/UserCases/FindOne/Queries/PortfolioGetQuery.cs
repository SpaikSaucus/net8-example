using CoreLibrary.UnitOfWork;
using Invest.Domain.Portfolio.Models;
using Invest.Domain.Portfolio.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Invest.Application.UserCases.FindOne.Queries
{
    public class PortfolioGetQuery : IRequest<Portfolio>
    {
        public long Id { get; set; }
        public Guid UserGuid { get; set; }
    }

    public class PortfolioGetQueryHandler : IRequestHandler<PortfolioGetQuery, Portfolio>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<PortfolioGetQueryHandler> logger;

        public PortfolioGetQueryHandler(IUnitOfWork unitOfWork, ILogger<PortfolioGetQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public Task<Portfolio> Handle(PortfolioGetQuery req, CancellationToken cancellationToken)
        {
            this.logger.LogDebug("call handle PortfolioGetQueryHandler.");

            var spec = new PortfolioGetSpecification(req.Id, req.UserGuid);
            return Task.FromResult(this.unitOfWork.Repository<Portfolio>().Find(spec).FirstOrDefault());
        }
    }
}
