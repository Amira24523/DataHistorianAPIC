using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace P_Cloud_API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ReadUsernameMiddleware
    {
        private readonly RequestDelegate _next;

        public ReadUsernameMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Check if Username header exists and retrieve its value
            string username = context.Request.Headers["Username"].ToString();

            // Check if username is null or empty, and return a 400 Bad Request response if it is
            if (string.IsNullOrEmpty(username) && (context.Request.Method == "POST" || context.Request.Method == "PUT" || context.Request.Method == "DELETE"))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Username header is missing or empty.");
                return;
            }

            // Set the username in the HttpContext
            context.Items["Username"] = username;

            await _next(context);
        }
    }
}
