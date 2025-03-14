using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace P_Cloud_API.Controllers
{
    [ApiController]
    [Route("api/authenticationTest")]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult ConfirmAuthentication()
        {
            return Ok("Authenticated successfully");
        }
    }
}
