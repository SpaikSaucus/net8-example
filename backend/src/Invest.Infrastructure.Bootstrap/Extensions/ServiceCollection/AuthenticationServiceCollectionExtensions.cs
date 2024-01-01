using CoreLibrary.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Invest.Infrastructure.Bootstrap.Extensions.ServiceCollection
{
    public static class AuthenticationServiceCollectionExtensions
    {
        private const string msgErrorEmptySecretKey = "JWT: Secret key 'SymmetricKey' is empty";

        public static void AddAuthenticationExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = Environment.GetEnvironmentVariable("JWT.SymmetricKey");
            if (secretKey == null || secretKey == string.Empty) throw new TechnicalException(msgErrorEmptySecretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
#warning ValidateIssuer and ValidateAudience, change to true in production.
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["AppSettings:Domain"],
                        ValidAudience = configuration["AppSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    };
                });
        }
    }
}
