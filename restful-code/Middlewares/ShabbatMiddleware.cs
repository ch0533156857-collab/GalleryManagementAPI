namespace restful_code.Middlewares
{
    public class ShabbatMiddleware
    {
        private readonly RequestDelegate _next;

        public ShabbatMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday) 
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errorResponse = new { error = "The service is not available on Shabbat." };
                await context.Response.WriteAsJsonAsync(errorResponse);
                return;
            }

            await _next(context);
        }
    }
}

