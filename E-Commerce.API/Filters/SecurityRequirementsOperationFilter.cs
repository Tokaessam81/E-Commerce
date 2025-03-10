﻿namespace E_Commerce.API.Filters
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    namespace EduCredit.Service.Filters
    {
        public class SecurityRequirementsOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                    .OfType<AuthorizeAttribute>().Any() ||
                                 context.MethodInfo.GetCustomAttributes(true)
                                    .OfType<AuthorizeAttribute>().Any();

                if (hasAuthorize)
                {
                    operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    }
                };
                }
            }
        }
    }

}
