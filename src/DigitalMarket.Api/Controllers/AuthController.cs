using DigitalMarket.Base.Response;
using DigitalMarket.Base.Session;
using DigitalMarket.Business.Services.TokenService;
using DigitalMarket.Data.Domain;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ApiResponse<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            var loginResult = await authService.Login(request);
            return loginResult;
        }

        [HttpPost("Logout")]
        [AllowAnonymous]
        public async Task<ApiResponse> Logout()
        {
            var response = await authService.Logout();
            return response;
        }

        [HttpPost("ChangePassword")]
        [AllowAnonymous]
        public async Task<ApiResponse> Logout([FromBody] ChangePasswordRequest request)
        {
            var response = await authService.ChangePassword(request);
            return response;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ApiResponse> Register([FromBody] RegisterUserRequest request)
        {
            var response = await authService.Register(request);
            return response;
        }
    }
}
