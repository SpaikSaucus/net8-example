using FluentValidation;
using MediatR;

namespace CoreLibrary.MediatR
{
    public abstract class Validator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected readonly IEnumerable<IValidator<TRequest>> validators;

        public Validator(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public virtual async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var failures = this.validators
                .Select(validator => validator.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Count > 0)
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
