using CoreLibrary.UnitOfWork;
using Invest.Domain.Portfolio.Models;
using Invest.Domain.Portfolio.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Invest.Application.UserCases.FindOne.Queries
{
    public class PortfolioGetQueryV1 : IRequest<Portfolio>
    {
        public long Id { get; set; }
    }

    public class PortfolioGetQueryV1Handler : IRequestHandler<PortfolioGetQueryV1, Portfolio>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<PortfolioGetQueryV1Handler> logger;

        public PortfolioGetQueryV1Handler(IUnitOfWork unitOfWork, ILogger<PortfolioGetQueryV1Handler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public Task<Portfolio> Handle(PortfolioGetQueryV1 req, CancellationToken cancellationToken)
        {
            this.logger.LogDebug("call handle PortfolioGetQueryV1Handler.");

            var spec = new PortfolioGetSpecificationV1(req.Id);
            return Task.FromResult(this.unitOfWork.Repository<Portfolio>().Find(spec).FirstOrDefault());
        }
    }
}
