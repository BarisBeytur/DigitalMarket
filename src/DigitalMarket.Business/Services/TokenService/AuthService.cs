using DigitalMarket.Base.Response;
using DigitalMarket.Base.Session;
using DigitalMarket.Base.Token;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DigitalMarket.Business.Services.TokenService;

public class AuthService : IAuthService
{
    private readonly JwtConfig jwtConfig;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly ISessionContext sessionContext;
    private readonly IUnitOfWork<DigitalWallet> _digitalWalletUnitOfWork;


    public AuthService(JwtConfig jwtConfig, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ISessionContext sessionContext, IUnitOfWork<DigitalWallet> digitalWalletUnitOfWork)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.jwtConfig = jwtConfig;
        this.sessionContext = sessionContext;
        _digitalWalletUnitOfWork = digitalWalletUnitOfWork;
    }

    public async Task<ApiResponse<AuthResponse>> Login(AuthRequest request)
    {
        var loginResult = await signInManager.PasswordSignInAsync(request.UserName, request.Password, true, false);
        if (!loginResult.Succeeded)
        {
            return new ApiResponse<AuthResponse>("Login Faild");
        }

        var user = await userManager.FindByNameAsync(request.UserName);
        if (user == null)
        {
            return new ApiResponse<AuthResponse>("Login Failed");
        }
        
        var responseToken = await GenerateToken(user);
        AuthResponse authResponse = new AuthResponse()
        {
            AccessToken = responseToken,
            UserName = request.UserName,
            ExpireTime = DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration)
        };

        return new ApiResponse<AuthResponse>(authResponse);
    }

    public async Task<ApiResponse> Logout()
    {
       await signInManager.SignOutAsync();
       return new ApiResponse();
    }

    public async Task<ApiResponse> ChangePassword(ChangePasswordRequest request)
    {
        ApplicationUser applicationUser = await userManager.GetUserAsync(sessionContext.HttpContext.User);
        if (applicationUser == null)
        {
            return new ApiResponse("Login Faild");
        }
        var user = await userManager.FindByNameAsync(applicationUser.UserName);
        if (user == null)
        {
            return new ApiResponse("Login Faild");
        }

        await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Register(RegisterUserRequest request)
    {

        var digitalWallet = new DigitalWallet
        {
            PointBalance = 0m,
            IsActive = true,
            InsertDate = DateTime.Now,
            InsertUser = "System",
        };

        await _digitalWalletUnitOfWork.Repository.Insert(digitalWallet);
        await _digitalWalletUnitOfWork.CommitWithTransaction();

        var newUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            Name = request.FirstName,
            Surname = request.LastName,
            EmailConfirmed = true,
            TwoFactorEnabled = false,
            Role = request.Role,
            DigitalWalletId = digitalWallet.Id,
            Status = true
        };

        var newUserResponse = await userManager.CreateAsync(newUser, request.Password);
        if (!newUserResponse.Succeeded)
        {
            return new ApiResponse("Register Failed");
        }

        
        digitalWallet.UserId = newUser.Id;
        _digitalWalletUnitOfWork.Repository.Update(digitalWallet);
        await _digitalWalletUnitOfWork.CommitWithTransaction();

        return new ApiResponse();
    }

    public async Task<string> GenerateToken(ApplicationUser user)
    {
        Claim[] claims = GetClaims(user);
        var secret = Encoding.ASCII.GetBytes(jwtConfig.Secret);

        JwtSecurityToken jwtToken = new JwtSecurityToken(
            jwtConfig.Issuer,
            jwtConfig.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
        );

        string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return token;
    }

    private Claim[] GetClaims(ApplicationUser user)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim("UserName", user.UserName),
            new Claim("UserId", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        return claims.ToArray();
    }
}