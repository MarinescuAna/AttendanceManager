using Microsoft.AspNetCore.Builder;

namespace AttendanceManager.Api.Middleware
{
    public static class MiddlewareException
    {
        /// <summary>
        /// Register the ExceptionHandler into the Starup
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
