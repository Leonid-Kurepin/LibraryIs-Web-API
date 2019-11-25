using LibraryIS.WebApplication.ExceptionHandling;
using Microsoft.AspNetCore.Builder;

namespace LibraryIS.WebApplication.Helpers
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
