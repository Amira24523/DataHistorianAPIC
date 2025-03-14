using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace P_Cloud_API.Middleware
{
public class RemoteIpAddressMiddleware
{
    private readonly RequestDelegate _next;

    public RemoteIpAddressMiddleware(RequestDelegate next)
    {
        _next = next;
    }

        public async Task Invoke(HttpContext context)
        {
            string forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            string remoteIpAddress = string.Empty;

            if (!string.IsNullOrEmpty(forwardedFor))
            {
                remoteIpAddress = forwardedFor.Split(new char[] { ',' }).FirstOrDefault().Trim();
            }

            if (string.IsNullOrEmpty(remoteIpAddress))
            {
                remoteIpAddress = context.Connection.RemoteIpAddress.ToString();
            }

            // Parse the IP address string into an IPAddress object
            var parsedAddress = IPAddress.Parse(remoteIpAddress);

            // Remove the leading ::ffff: part from IPv4-mapped IPv6 addresses
            if (parsedAddress.IsIPv4MappedToIPv6)
            {
                remoteIpAddress = parsedAddress.MapToIPv4().ToString();
            }

            context.Items["RemoteIpAddress"] = remoteIpAddress;

            await _next(context);
        }
    }
}

