using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniHub.Api.Extension;
using UniHub.Domain.Interface;
using UniHub.Dto;

namespace UniHub.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("logIn")]
    [SkipTenantHeader]
    public async Task<IActionResult> LoginAsync(LoginDetailDto login)
    {
        return Ok(await _authService.LoginAsync(login));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync([FromBody] TokenDto token)
    {
        await _authService.LogoutAsync(token);
        return Ok();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenDto model)
    {
        return Ok(await _authService.RefreshTokenAsync(model));
    }
}
