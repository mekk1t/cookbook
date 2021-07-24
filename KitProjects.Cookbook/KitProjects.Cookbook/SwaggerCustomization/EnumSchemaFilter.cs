using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KitProjects.Cookbook.SwaggerCustomization
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (type.IsEnum)
                schema.Enum.RemoveAt(0);
        }
    }
}
