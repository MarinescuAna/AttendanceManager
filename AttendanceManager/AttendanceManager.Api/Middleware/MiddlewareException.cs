﻿using Microsoft.AspNetCore.Builder;

namespace AttendanceManager.Api.Middleware
{
    public static class MiddlewareException
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
