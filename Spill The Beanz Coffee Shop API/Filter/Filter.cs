using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Spill_The_Beanz_Coffee_Shop_API.Filter
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // If the action uses [FromForm], replace the requestBody schema
            if (operation.RequestBody == null ||
                !operation.RequestBody.Content.ContainsKey("multipart/form-data"))
                return;

            var schema = new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>(),
                Required = new HashSet<string>()
            };

            foreach (var param in context.MethodInfo.GetParameters())
            {
                if (param.ParameterType == typeof(Microsoft.AspNetCore.Http.IFormFile))
                {
                    schema.Properties[param.Name] = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    };
                    schema.Required.Add(param.Name);
                }
                else
                {
                    // Handle properties of [FromForm] complex object (like MenuItemDto)
                    foreach (var prop in param.ParameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        var propSchema = new OpenApiSchema { Type = MapType(prop.PropertyType) };
                        schema.Properties[prop.Name] = propSchema;

                        // Mark required if [Required] attribute is present
                        var requiredAttr = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.RequiredAttribute>();
                        if (requiredAttr != null)
                        {
                            schema.Required.Add(prop.Name);
                        }
                    }
                }
            }

            operation.RequestBody.Content["multipart/form-data"].Schema = schema;
        }

        private string MapType(System.Type type)
        {
            if (type == typeof(string)) return "string";
            if (type == typeof(int) || type == typeof(long)) return "integer";
            if (type == typeof(decimal) || type == typeof(double) || type == typeof(float)) return "number";
            if (type == typeof(bool)) return "boolean";
            return "string"; // fallback
        }
    }
}
