using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DigitalOffice.Kernel.OpenApi.OperationFilters
{
  public class TokenOperationFilter : IOperationFilter
  {
    public const string ParameterName = "token";

    public OpenApiSchema TokenScheme = new OpenApiSchema
    {
      Type = "string",
      Example = new OpenApiString("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkw")
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      operation.Parameters ??= new List<OpenApiParameter>();

      if (!context.ApiDescription.ActionDescriptor.RouteValues.TryGetValue("controller", out string _))
      {
        return;
      }

      operation.Parameters.Add(new OpenApiParameter
      {
        Name = ParameterName,
        In = ParameterLocation.Header,
        Schema = TokenScheme,
        Description = "JWT access token.",
        Required = true,
      });
    }
  }
}
