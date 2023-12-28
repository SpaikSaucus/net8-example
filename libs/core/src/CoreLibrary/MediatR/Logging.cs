using MediatR;
using Microsoft.Extensions.Logging;

namespace CoreLibrary.MediatR
{
    public abstract class Logging<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected readonly ILogger<Logging<TRequest, TResponse>> logger;

        public Logging(ILogger<Logging<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public virtual async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Executing command {CommandName} with request {@Command}", request.GetType().FullName, request);

            TResponse response = await next();

            this.logger.LogInformation("Executed command {CommandName} with response {@Response}", request.GetType().FullName, response);

            return response;
        }
    }
}
