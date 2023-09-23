using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Market.Application.Middlewares;
using Market.Core.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Filters
{
    public class InputValidationActionFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                //Customize your error message
                string messages = string.Join("; ", context.ModelState.Values
                         .SelectMany(x => x.Errors)
                         .Select(x => !string.IsNullOrWhiteSpace(x.ErrorMessage) ? x.ErrorMessage : x.Exception?.Message.ToString()));
                context.RouteData.Values.Add("message", messages);

                throw new AppException(messages);
            }

            await next();
        }
    }
}
