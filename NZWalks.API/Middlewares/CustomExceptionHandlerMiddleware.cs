using System.Net;

namespace NZWalks.API.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly ILogger<CustomExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate requestDelegate;

        public CustomExceptionHandlerMiddleware(ILogger<CustomExceptionHandlerMiddleware> logger, RequestDelegate requestDelegate)
        {
            this.logger = logger;
            this.requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await requestDelegate(httpContext);
            }
            catch (Exception ex)
            {
                //Logging global Exception
                var errorId = Guid.NewGuid();

                logger.LogError($"{errorId},{ex.Message}");
                //Returning the custom response

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Soemthing went wrong.."
                };
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }

    }
}
