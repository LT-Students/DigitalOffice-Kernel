using System;
using System.Net;
using MassTransit;
using MassTransit.Clients;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MassTransit.Clients.Contexts;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.AccessValidator.Requests;
using LT.DigitalOffice.Kernel.AccessValidator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.Kernel.AccessValidator
{
    public class AccessValidator : IAccessValidator
    {
        private Guid userId;
        private readonly IBus bus;
        private readonly RabbitMQOptions options;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccessValidator(
            [FromServices] IBus bus,
            [FromServices] IOptions<RabbitMQOptions> options,
            [FromServices] IHttpContextAccessor httpContextAccessor)
        {
            this.bus = bus;
            this.options = options.Value;
            this.httpContextAccessor = httpContextAccessor;
        }

        private Guid GetCurrentUserId()
        {
            if (httpContextAccessor.HttpContext.Request.Headers["UserId"].Count == 0)
            {
                throw new NullReferenceException("There are no UserId headers in the request.");
            }

            var headersUserId = httpContextAccessor.HttpContext.Request.Headers["UserId"];

            if (headersUserId.Count > 1)
            {
                throw new HttpListenerException(400, "There can't be more than one UserId in the request.");
            }

            if (!Guid.TryParse(headersUserId, out Guid userIdFromHeaders))
            {
                throw new FormatException("Incorrect GUID format.");
            }

            return userIdFromHeaders;
        }

        private IRequestClient<IAccessValidatorRequest> CreateRequestClient(IBus bus, string url)
        {
            var requestClientFactory = new ClientFactory(new BusClientFactoryContext(bus));

            return requestClientFactory.CreateRequestClient<IAccessValidatorRequest>(
                new Uri(url),
                new RequestTimeout());
        }

        public async Task<bool> HasRights(int rightId)
        {
            userId = GetCurrentUserId();

            var requestClient = CreateRequestClient(bus, options.AccessValidatorCheckRightsServiceURL);

            var result = await requestClient.GetResponse<IOperationResult<bool>>(new
            {
                UserId = userId,
                RightId = rightId
            });

            return result.Message.Body;
        }

        public async Task<bool> IsAdmin()
        {
            userId = GetCurrentUserId();

            var requestClient = CreateRequestClient(bus, options.AccessValidatorUserServiceURL);

            var result = await requestClient.GetResponse<IOperationResult<bool>>(new
            {
                UserId = userId
            });

            return result.Message.Body;
        }
    }
}