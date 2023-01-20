using CeciGoogleFirebase.Domain.DTO.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.WebApplication.Attribute
{
    [ExcludeFromCodeCoverage]
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = string.Join('\n', context.ModelState.Values.Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage));

                var responseObj = new ResultResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = errors

                };

                context.Result = new JsonResult(responseObj){
                    StatusCode = (int)System.Net.HttpStatusCode.BadRequest
                };
            }
        }
    }
}
