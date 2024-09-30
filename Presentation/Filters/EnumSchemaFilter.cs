using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;


namespace Presentation.Filters;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum) {  return; }

        var enumNames = Enum.GetNames(context.Type);
        schema.Enum = enumNames
            .Select(name => (IOpenApiAny)new OpenApiString(name))
            .ToList();
        schema.Type = "string";       
    }
}
