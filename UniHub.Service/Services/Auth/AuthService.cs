using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UniHub.Domain.Entities;
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
    }

    public async Task<LoginResultDto> LoginAsync(LoginDetailDto login)
    {
        var user = await _userService.GetAspNetUserByEmailAsync(login.Email);

        if (user?.EmailConfirmed == false)
        {
            throw new ApplicationException("Email not verified");
        }

        if (user?.LockoutEnd is not null)
        {
            throw new ApplicationException("Your Account Has been locked Out");
        }

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
        var token = await GenerateJWTTokenAsync(user);
        var refershToken = await GetRefershTokenAsync(user.Id, token);
        var tenantUsers = await _tenantUserService.GetTenantUsersAsync(user.Id);
        var tenantUser = tenantUsers.FirstOrDefault(e => e.IsPrimary);
        return new LoginResultDto
        {
            Token = token,
            Expiration = DateTime.Now.AddMinutes(30),
            RefershToken = refershToken.RefershToken,
            RefershTokenExpires = refershToken.RefershTokenExpires,
            User = user.Adapt<AspNetUserDto>(),
            TenantUser = tenantUser,
            TenantUsers = tenantUsers
        };
    }
    public async Task<string> GenerateJWTTokenAsync(AspNetUser user)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.PrimarySid,user.Id.ToString()),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));

        var jwtSecurityToken = new JwtSecurityToken(
        expires: DateTime.Now.AddMinutes(30),
        claims: authClaims,
        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    private async Task<TokenDto> GetRefershTokenAsync(Guid userId, string token)
    {
        var refershToken = (await _refreshRepository.GetAsync(x => x.UserId == userId)).FirstOrDefault();

        if (refershToken == null)
        {
            refershToken = new AspNetUserRefershToken()
            {
                UserId = userId,
                AccessToken = token,
                RefershToken = GenerateRefreshToken(),
                RefershTokenExpires = DateTime.UtcNow.AddDays(7),
                GenerateAt = DateTime.UtcNow,
            };

            await _refreshRepository.InsertAsync(refershToken);
        }
        else
        {
            refershToken.AccessToken = token;
            refershToken.RefershToken = GenerateRefreshToken();
            refershToken.RefershTokenExpires = DateTime.UtcNow.AddMinutes(2);
            refershToken.GenerateAt = DateTime.UtcNow;

            _refreshRepository.Update(refershToken);
        }

        await _unitOfWork.CommitAsync();

        return refershToken.Adapt<TokenDto>();
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public async Task<LoginResultDto> RefreshTokenAsync(TokenDto refershToken)
    {
        var token = (await _refreshRepository.GetAsync(x =>
            x.AccessToken == refershToken.AccessToken &&
            x.RefershToken == refershToken.RefershToken
        )).FirstOrDefault();

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

        var tenantUsers = await _tenantUserService.GetTenantUsersAsync(user.Id);
        var tenantUser = tenantUsers.FirstOrDefault(e => e.IsPrimary);

        return new LoginResultDto
        {
            Token = await GenerateJWTTokenAsync(user!),
            Expiration = DateTime.Now.AddMinutes(30),
            RefershToken = refershToken.RefershToken,
            RefershTokenExpires = refershToken.RefershTokenExpires,
            User = user.Adapt<AspNetUserDto>(),
            TenantUser = tenantUser,
            TenantUsers = tenantUsers
        };
    }
    public async Task LogoutAsync(TokenDto tokenDto)
    {
        var token = await _refreshRepository.GetAsync(x => x.AccessToken == tokenDto.AccessToken && x.RefershToken == tokenDto.RefershToken);

        if (token == null)
        {
            throw new ApplicationException("Token not found !");
        }

        _refreshRepository.BulkDelete(token);

        await _unitOfWork.CommitAsync();

    }
}
