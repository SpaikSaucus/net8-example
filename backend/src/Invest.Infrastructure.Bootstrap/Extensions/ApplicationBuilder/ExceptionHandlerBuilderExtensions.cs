using CoreLibrary.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invest.Infrastructure.Bootstrap.Extensions.ApplicationBuilder
{
    public static class ExceptionHandlerBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder, bool includeErrorDetailInResponse = false)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>(includeErrorDetailInResponse);
        }
    }

    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly bool includeErrorDetailInResponse;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, bool includeErrorDetailInResponse)
        {
            this.logger = logger;
            this.includeErrorDetailInResponse = includeErrorDetailInResponse;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var exceptionHandlerPathFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is ValidationException validationException)
            {
                var errorObject = JsonConvert.SerializeObject(new
                {
                    errors = validationException.Errors.Select(error => new
                    {
                        property = error.PropertyName,
                        message = error.ErrorMessage
                    })
                });

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(errorObject, Encoding.UTF8);
            }
            else if (exceptionHandlerPathFeature?.Error is ApiException apiException)
            {
                this.logger.LogError(
                    "API Error: error en llamada a {api}: {status} {reason} \n{result}",
                    apiException.RequestMessage.RequestUri,
                    (int)apiException.StatusCode,
                    apiException.ReasonPhrase,
                    apiException.Content);

                await this.WriteGenericErrorToResponse(
                    httpContext,
                    new { Exception = apiException.ToString(), Content = apiException.Content });
            }
            else if (exceptionHandlerPathFeature?.Error is BusinessException businessException)
            {
                var errorObject = JsonConvert.SerializeObject(new
                {
                    errors = new
                    {
                        message = CutArgumentMessage(businessException.Message)
                    }
                });

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(errorObject, Encoding.UTF8);
            }
            else if (exceptionHandlerPathFeature?.Error is ArgumentException argumentException)
            {
                var errorObject = JsonConvert.SerializeObject(new
                {
                    errors = new
                    {
                        property = argumentException.ParamName,
                        message = CutArgumentMessage(argumentException.Message)
                    }
                });

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(errorObject, Encoding.UTF8);
            }
            else
            {
                await this.WriteGenericErrorToResponse(
                    httpContext,
                    new Exception(
                        $"Unhandled by {nameof(ExceptionHandlerMiddleware)}",
                        exceptionHandlerPathFeature?.Error).ToString());
            }
        }

        private static string CutArgumentMessage(string msg)
        {
            return msg.Split(Environment.NewLine).FirstOrDefault();
        }

        private async Task WriteGenericErrorToResponse(HttpContext httpContext, object errorDetail)
        {
            var errorObject = JsonConvert.SerializeObject(
                new
                {
                    Error = "Ocurrio un error en la Aplicacion. Por favor intentá mas tarde.",
                    Detail = this.includeErrorDetailInResponse ? errorDetail : null
                },
                new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                });

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(errorObject, Encoding.UTF8);
        }
    }
}
