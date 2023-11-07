using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DigitalOffice.Kernel.OpenApi.SchemaFilters;

public class JsonPatchDocumentSchemaFilter : ISchemaFilter
{
  public void Apply(OpenApiSchema schema, SchemaFilterContext context)
  {
    if (context.Type == typeof(Operation[]))
    {
      Type argumentType = context.ParameterInfo.ParameterType.GetGenericArguments().FirstOrDefault();

      if (argumentType is not null)
      {
        schema.Type = "array";

        schema.Items = new OpenApiSchema
        {
          Type = "object",
          Properties = new Dictionary<string, OpenApiSchema>
          {
            {
              "op", new OpenApiSchema
              {
                Type = "string",
                Enum = new List<IOpenApiAny> { new OpenApiString("replace") }
              }
            },
            {
              "value", new OpenApiSchema
              {
                Type = "object",
                Nullable = true
              }
            },
            {
              "path", new OpenApiSchema
              {
                Type = "string",
                Enum = argumentType.GetProperties().Select(p => new OpenApiString("/" + p.Name) as IOpenApiAny).ToList()
              }
            }
          }
        };

        schema.Description = "Array of operations to perform.";
      }
    }
  }
}
