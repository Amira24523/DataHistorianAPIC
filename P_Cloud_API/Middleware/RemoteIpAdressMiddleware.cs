using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging; // Füge Logging hinzu
using System.Net;
using System.Threading.Tasks;

namespace P_Cloud_API.Middleware
{
    public class RemoteIpAddressMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RemoteIpAddressMiddleware> _logger; // Füge Logger hinzu

        public RemoteIpAddressMiddleware(RequestDelegate next, ILogger<RemoteIpAddressMiddleware> logger) // Injiziere Logger
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            string remoteIpAddress = string.Empty;

            if (!string.IsNullOrEmpty(forwardedFor))
            {
                remoteIpAddress = forwardedFor.Split(new char[] { ',' }).FirstOrDefault()?.Trim();
            }

            if (string.IsNullOrEmpty(remoteIpAddress))
            {
                remoteIpAddress = context.Connection.RemoteIpAddress?.ToString();
            }

            if (!string.IsNullOrEmpty(remoteIpAddress) && IPAddress.TryParse(remoteIpAddress, out var parsedAddress))
            {
                if (parsedAddress.IsIPv4MappedToIPv6)
                {
                    remoteIpAddress = parsedAddress.MapToIPv4().ToString();
                }
                context.Items["RemoteIpAddress"] = remoteIpAddress;
            }
            else
            {
                _logger.LogWarning($"Ungültige IP-Adresse erkannt: '{remoteIpAddress}'");
                // Hier kannst du entscheiden, wie du fortfahren möchtest, z.B.:
                // context.Items["RemoteIpAddress"] = "0.0.0.0"; // Standardwert setzen
                // Oder die Middleware ohne die IP-Adresse fortfahren lassen
            }

            await _next(context);
        }
    }
}