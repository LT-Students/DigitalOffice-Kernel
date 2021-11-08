using System;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LT.DigitalOffice.Kernel.Middlewares.Token
{
    /// <summary>
    /// Check JW token middleware.
    /// </summary>
    public class TokenMiddleware
    {
        private const string Token = "token";
        private const string OptionsMethod = "OPTIONS";
        private const string DonNotHaveTokenMessage = "Enter token";

        private readonly RequestDelegate requestDelegate;
        private readonly TokenConfiguration tokenConfiguration;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TokenMiddleware(
            RequestDelegate requestDelegate,
            IOptions<TokenConfiguration> option)
        {
            this.requestDelegate = requestDelegate;

            tokenConfiguration = option.Value;
        }

        /// <summary>
        /// Invoke check token action.
        /// </summary>
        public async Task InvokeAsync(
            HttpContext context,
            IRequestClient<ICheckTokenRequest> client)
        {
            if (string.Equals(context.Request.Method, OptionsMethod, StringComparison.OrdinalIgnoreCase) ||
                (tokenConfiguration.SkippedEndpoints != null &&
                 tokenConfiguration.SkippedEndpoints.Any(
                     url =>
                         url.Equals(context.Request.Path, StringComparison.OrdinalIgnoreCase))))
            {
                await requestDelegate.Invoke(context);
            }
            else
            {
                var token = context.Request.Headers[Token];

                if (string.IsNullOrEmpty(token))
                {
                    throw new ForbiddenException(DonNotHaveTokenMessage);
                }

                Response<IOperationResult<Guid>> response = null;

                response = await client.GetResponse<IOperationResult<Guid>>(
                    ICheckTokenRequest.CreateObj(token),
                    timeout: RequestTimeout.After(s: 2));

                if (response.Message.IsSuccess)
                {
                    context.Items[ConstStrings.UserId] = response.Message.Body;

                    await requestDelegate.Invoke(context);
                }
                else
                {
                    throw new ForbiddenException(response.Message.Errors);
                }
            }
        }
    }
}