using LT.DigitalOffice.Kernel.Middlewares.ParseEntities.Models.Requests;
using LT.DigitalOffice.Kernel.Middlewares.ParseEntities.Models.Responses;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.Broker.Consumer
{
    public class FindParseEntitiesConsumer : IConsumer<IFindParseEntitiesRequest>
    {
        public async Task Consume(ConsumeContext<IFindParseEntitiesRequest> context)
        {
            object result = null;

            try
            {
                result = new
                {
                    IsSuccess = true,
                    Body = IFindParseEntitiesResponse.CreateObj()
                };
            }
            catch (Exception exc)
            {
                result = new
                {
                    IsSuccess = false,
                    Errors = new List<string> { exc.Message }
                };
            }

            await context.RespondAsync<IOperationResult<IFindParseEntitiesResponse>>(result);
        }
    }
}
