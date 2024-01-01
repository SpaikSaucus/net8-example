﻿using Invest.Application.UserCases.FindOne.Queries;
using Invest.Application.UserCases.Login.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using CoreLibrary.Security;

namespace Invest.Application.UserCases.Login.Commands
{
    public class UserLoginCommand : IRequest<string>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        public UserLoginCommandValidator()
        {
            this.RuleFor(x => x.UserName).NotEmpty().WithMessage("User is requiered.");
            this.RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }


    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, string>
    {
        private readonly ILogger<UserLoginCommandHandler> logger;
        private readonly IMediator mediator;
        private readonly ITokenProvider tokenProvider;

        public UserLoginCommandHandler(ILogger<UserLoginCommandHandler> logger, IMediator mediator, ITokenProvider tokenProvider)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.tokenProvider = tokenProvider;
        }

        public async Task<string> Handle(UserLoginCommand cmd, CancellationToken cancellationToken)
        {
            var userExists = await this.mediator.Send(new UserGetQuery() { UserName = cmd.UserName, Password = cmd.Password }, cancellationToken);
            if (userExists == null)
                return string.Empty;

            var claims = new ClaimDto
            {
                Guid = userExists.UUID.ToString(),
                UserName = userExists.UserName,
                FirstName = userExists.FirstName,
                LastName = userExists.LastName,
                Email = userExists.Email
            };

            return this.tokenProvider.GenerateToken(claims);
        }
    }
}
