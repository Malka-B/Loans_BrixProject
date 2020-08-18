using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Loans.WebApi.Middleware
{
    public class LoanErrorsHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public LoanErrorsHandlerMiddleware(RequestDelegate next)
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
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Log.Error(ex.Message);
            var result = JsonConvert.SerializeObject(new { error = ex.Message });
            return context.Response.WriteAsync(result);
        }
    }
}
