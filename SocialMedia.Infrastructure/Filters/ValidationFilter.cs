using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Filters
{
    //instala un nuget Microsoft.AspNetCore.Mvc.Filters; e implementa una interfaz IAsyncActionFilter
    public class ValidationFilter : IAsyncActionFilter
    { 
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.ModelState.IsValid)
            {
                //Microsoft.AspNetCore.Mvc se instala con BadRequestObjectResult;
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }
            await next();
        }
    }

}
