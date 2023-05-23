using Application.Features;
using Application.Features.Identity;
using Application.Interfaces;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityController(
        IMediator mediator,
        ICurrentUserService currentUserService,
        SignInManager<ApplicationUser> signInManager)
    {
        _mediator = mediator;
        _signInManager = signInManager;
        _currentUserService = currentUserService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser.InputModel inputModel)
    {
        var result = await _signInManager.PasswordSignInAsync(
            inputModel.Username, 
            inputModel.Password, 
            true, 
            false);
        
        return result.Succeeded 
            ? Ok() 
            : Unauthorized();
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser.InputModel inputModel)
    {
        await _mediator.Send(inputModel);
        return Ok();
    }
    
    [HttpGet("user")]
    public IActionResult GetUser()
    {
        var userId = _currentUserService.UserId;
        return userId is null 
            ? NotFound() 
            : Ok(userId);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}