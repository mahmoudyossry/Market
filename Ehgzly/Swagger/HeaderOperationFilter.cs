using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Market.API.Swagger
{
    public class HeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            //if (context.MethodInfo.GetCustomAttribute(typeof(SwaggerHeaderAttribute)) is SwaggerHeaderAttribute attribute)
            //{
            var parameters = new string[] { "lang" };

            var existingParams = operation.Parameters.Where(p =>
                p.In == ParameterLocation.Header && parameters.Contains(p.Name)).ToList();

            foreach (var param in existingParams)// remove description from [FromHeader] argument attribute
            {
                operation.Parameters.Remove(param);
            }

           
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "lang",
                In = ParameterLocation.Header,
                Description = "current acive lang",
                Required = true,
                Content = new Dictionary<string, OpenApiMediaType>
                        {
                            {
                                "text/plain", new OpenApiMediaType
                                {
                                    Schema = new OpenApiSchema
                                    {
                                        Type = "string",
                                        Required = new HashSet<string>{ "string" },
                                        Properties = new Dictionary<string, OpenApiSchema>
                                        {
                                            {
                                                "string", new OpenApiSchema()
                                                {
                                                    Type = "string",
                                                    Format = "text"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
            });

            //}
        }
    }

    public class SwaggerHeaderAttribute : Attribute
    {
        public string HeaderName { get; }
        public string Description { get; }
        public string DefaultValue { get; }
        public bool IsRequired { get; }

        public SwaggerHeaderAttribute(string headerName, string description = null, string defaultValue = null, bool isRequired = false)
        {
            HeaderName = headerName;
            Description = description;
            DefaultValue = defaultValue;
            IsRequired = isRequired;
        }
    }
}
