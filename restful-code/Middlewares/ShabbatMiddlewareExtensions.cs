using restful_code.Middlewares;

namespace restful_code.Middlewares
{
    public static class ShabbatMiddlewareExtensions
    {
        public static IApplicationBuilder UseShabbatCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ShabbatMiddleware>();
        }
    }
}
