using CoreLibrary.MediatR;
using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace Invest.Application.Behaviors
{
    public class ValidatorBehavior<TRequest, TResponse> : Validator<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators) : base(validators)
        {
        }
    }
}
