using CoreLibrary.MediatR;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;

namespace Invest.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : Logging<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public LoggingBehavior(ILogger<Logging<TRequest, TResponse>> logger) : base(logger)
        {
        }

        public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Executing command {CommandName}", request.GetType().FullName);

            TResponse response = await next();

            this.logger.LogInformation("Executed command {CommandName}", request.GetType().FullName);

            return response;
        }
    }
}
