using CrudPlay.Api.Helpers;
using CrudPlay.Core.DTO;
using CrudPlay.Core.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudPlay.Api.Controllers;

//TODO: Move service call to orchestrator
//TODO: Add commands through MediatR
[ApiController]
[Route("api/[controller]")]
public class AuthController(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> _roleManager,
    ITokenGeneration _tokenGeneration)
    : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = _roleManager;
    private readonly ITokenGeneration _tokenGeneration = _tokenGeneration;

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return Unauthorized("Invalid login attempt");
        }

        var user = await _userManager.FindByNameAsync(request.Email);
        if (user is null)
        {
            return Unauthorized("Invalid login attempt");
        }

        var token = _tokenGeneration.GenerateJwtToken(user);
        var refreshToken = _tokenGeneration.GenerateRefreshToken();

        return Ok(new LoginResponse
        {
            TokenType = "Bearer",
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresIn = 7200
        });
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        var user = new ApplicationUser { UserName = request.Email, Email = request.Email, EmailConfirmed = true };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _userManager.AddToRoleAsync(user, RoleConstants.User);

        return Ok("User registered successfully");
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

        if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Unauthorized("Invalid or expired refresh token");
        }

        var newAccessToken = _tokenGeneration.GenerateJwtToken(user);
        var newRefreshToken = _tokenGeneration.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return Ok(new LoginResponse
        {
            TokenType = "Bearer",
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresIn = 7200
        });
    }

    [HttpPost()]
    [Route("assign-role")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> AssignRoleAsync([FromBody] RoleAssignmentRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return NotFound("User not found");
        if (!await _roleManager.RoleExistsAsync(request.Role))
            return BadRequest("Role does not exist");
        if (await _userManager.IsInRoleAsync(user, request.Role))
            return BadRequest("User already has this role");

        await _userManager.AddToRoleAsync(user, request.Role);

        return Ok($"Role {request.Role} assigned to {request.Email}");
    }
}
