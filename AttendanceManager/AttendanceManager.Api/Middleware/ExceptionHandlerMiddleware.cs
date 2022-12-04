using AttendanceManager.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AttendanceManager.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }catch(Exception ex)
            {
                await ConvertException(context,ex);
            }
        }
        
        private Task ConvertException(HttpContext context, Exception exception)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var result = string.Empty;

            switch (exception)
            {
                case BadRequestException _:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    break;
                case NotFoundException _:
                    httpStatusCode = HttpStatusCode.NotFound;
                    break;
                case Exception _:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    break;
            }
            context.Response.StatusCode = (int)httpStatusCode;

            if (result == string.Empty)
            {
                result = JsonConvert.SerializeObject(new { 
                    status = httpStatusCode,
                    error = exception.Message 
                });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
