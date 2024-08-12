using DigitalMarket.Base.Response;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;

namespace DigitalMarket.Business.Services.TokenService;

public interface IAuthService
{
    Task<ApiResponse<AuthResponse>> Login(AuthRequest request);
    Task<ApiResponse> Logout();
    Task<ApiResponse> ChangePassword(ChangePasswordRequest request);
    Task<ApiResponse> Register(RegisterUserRequest request);
}