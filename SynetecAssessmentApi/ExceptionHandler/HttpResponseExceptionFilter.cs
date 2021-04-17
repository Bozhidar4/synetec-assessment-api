using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace SynetecAssessmentApi.ExceptionHandler
{
    public class HttpResponseExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                context.Result = new JsonResult(new
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = nameof(HttpStatusCode.BadRequest),
                    Message = context.Exception.Message
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
