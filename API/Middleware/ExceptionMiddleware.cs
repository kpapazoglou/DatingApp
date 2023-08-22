using System.Net;
using System.Text.Json;
using API.Errors;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace API.Middleware
{
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _loggger;
        private readonly IHostingEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> loggger, IHostingEnvironment env)
        { 
            _env = env;
            _next = next;
            _loggger = loggger;
           
        }

        /*InvokeAsync-->this method has to be called invoke async because we're relying on the framework

            to recognize or we're going to tell our framework that this is middleware.

            And the middleware, the framework is going to expect to see a method called invoke async as that's

            what it uses to decide what's going to happen next.

            Because middleware goes from one bit of middleware to the next bit of middleware to the next bit of

            middleware always calling next.

            And that's what this request delegate is.*/
        public async Task InvokeAsync(HttpContext context)
        {
            try{
                await _next(context);

            }
            catch(Exception ex){
                _loggger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                  ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())//*if we are in dev mode*/
                  : new ApiException(context.Response.StatusCode, ex.Message, "InternalServerError");//if we are not in dev mode

                  var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                 
                  var json = JsonSerializer.Serialize(response, options);
                  
                  await context.Response.WriteAsync(json);
            }
        }
    }
}