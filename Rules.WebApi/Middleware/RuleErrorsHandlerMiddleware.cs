using Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Rules.WebApi.Middleware
{
    public class RuleErrorsHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public RuleErrorsHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionsAsync(context, ex);
            }
        }
        public static Task HandleExceptionsAsync(HttpContext context, Exception ex)
        {
            var code = new HttpStatusCode();
            var errorMessage = "";
            if (ex is DuplicateLoanProviderIdException)
            {
                code = HttpStatusCode.BadRequest;
                context.Response.StatusCode = (int)code;
                errorMessage = "Loan Provider Id already exist in system. Try again or update your policy rules";
            }           
            //write to log db the exception
            var result = JsonConvert.SerializeObject(new { error = errorMessage });
            return context.Response.WriteAsync(result);
        }
    }
}
