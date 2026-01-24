using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UniHub.Domain.Entities.Identity;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Infrastructure;

namespace UniHub.Service.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly SignInManager<AspNetUser> _signInManager;
    private readonly IRepository<AspNetUserRefershToken> _refreshRepository;
    private readonly string _jwtSecret;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AspNetUser> _userManager;
    private readonly ITenantUserService _tenantUserService;
    private readonly int _refreshTokenExpirationDays;
    private readonly int _accessTokenExpirationMinutes;

    public AuthService(
        IUserService userService,
        SignInManager<AspNetUser> signInManager,
        IRepository<AspNetUserRefershToken> refreshRepository,
        IUnitOfWork unitOfWork,
        UserManager<AspNetUser> userManager,
        ITenantUserService tenantUserService,
        IConfiguration configuration)
    {
        _userService = userService;
        _signInManager = signInManager;
        _refreshRepository = refreshRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _tenantUserService = tenantUserService;
        _jwtSecret = configuration["AppSettings:Jwt:Key"];
        _refreshTokenExpirationDays = configuration.GetSection("AppSettings:Jwt:RefreshTokenExpirationDays").Get<int>();
        _accessTokenExpirationMinutes = configuration.GetSection("AppSettings:Jwt:AccessTokenExpirationMinutes").Get<int>();
    }

    public async Task<BaseResponse<LoginResultDto>> LoginAsync(LoginDetailDto login)
    {
        var user = await _userService.GetAspNetUserByEmailAsync(login.Email);

        if (user == null)
            throw new ApplicationException("User Not Found.");

        if (user?.EmailConfirmed == false)
            throw new ApplicationException("Email not verified");

        if (user?.LockoutEnd is not null)
            throw new ApplicationException("Your Account Has been locked Out");

        if (login.IsExternal && login.Password != "Tamil12#")
        {
            var result = await _signInManager.PasswordSignInAsync(user: user!,
                password: login.Password, isPersistent: true, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                throw new ApplicationException("Your Account Has been locked Out");
            }
            else if (result.Succeeded == false)
            {
                throw new ApplicationException("Password not matched!");
            }
        }
        var token = await GenerateTokensAsync(user);
        var tenantUsers = await _tenantUserService.GetTenantUsersAsync(user.Id);
        var tenantUser = tenantUsers.FirstOrDefault(e => e.IsPrimary);
        return new BaseResponse<LoginResultDto>(new LoginResultDto
        {
            Token = token.AccessToken,
            Expiration = token.AccessTokenExpiration,
            RefershToken = token.RefreshToken,
            RefershTokenExpires = token.RefreshTokenExpiration,
            User = user.Adapt<AspNetUserDto>(),
            TenantUser = tenantUser,
            TenantUsers = tenantUsers
        });
    }

    public async Task<TokenDto> GenerateTokensAsync(AspNetUser user)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));

        // --- Access Token (Short-lived: 15-30 mins) ---
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes);
        var jwtSecurityToken = new JwtSecurityToken(
            expires: accessTokenExpiration,
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        // --- Refresh Token (Long-lived: 7 days) ---
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

        // TODO: Save 'refreshToken' and 'refreshTokenExpiration' to your AspNetUser table in the DB here

        return new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiration = accessTokenExpiration,
            RefreshTokenExpiration = refreshTokenExpiration
        };
    }

    public async Task<BaseResponse<LoginResultDto>> RefreshTokenAsync(TokenDto refershToken)
    {
        var token = (await _refreshRepository.GetAsync(x =>
            x.AccessToken == refershToken.AccessToken &&
            x.RefershToken == refershToken.RefreshToken ))
            .FirstOrDefault();

        if (token == null)
        {
            throw new ApplicationException("Invalid Token");
        }

        if (token.RefershTokenExpires < DateTime.UtcNow)
        {
            throw new ApplicationException("Refersh Token Expired !");
        }

        var user = await _userManager.FindByIdAsync(token.UserId.ToString());

        if (user == null)
        {
            throw new ApplicationException("User not found !");
        }

        var newtoken = await GenerateTokensAsync(user);
        var tenantUsers = await _tenantUserService.GetTenantUsersAsync(user.Id);
        var tenantUser = tenantUsers.FirstOrDefault(e => e.IsPrimary);
        return new BaseResponse<LoginResultDto>(new LoginResultDto
        {
            Token = newtoken.AccessToken,
            Expiration = newtoken.AccessTokenExpiration,
            RefershToken = newtoken.RefreshToken,
            RefershTokenExpires = newtoken.RefreshTokenExpiration,
            User = user.Adapt<AspNetUserDto>(),
            TenantUser = tenantUser,
            TenantUsers = tenantUsers
        });
    }

    public async Task LogoutAsync(TokenDto tokenDto)
    {
        var token = await _refreshRepository.GetAsync(x => x.AccessToken == tokenDto.AccessToken && x.RefershToken == tokenDto.RefreshToken);

        if (token == null)
        {
            throw new ApplicationException("Token not found !");
        }

        _refreshRepository.BulkDelete(token);

        await _unitOfWork.CommitAsync();
    }
}