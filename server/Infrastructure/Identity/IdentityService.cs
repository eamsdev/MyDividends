using System.Security.Authentication;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task CreateUserAsync(string username, string password)
    {
        var createResult = await _userManager.CreateAsync(
            new ApplicationUser { UserName = username }, password);
        
        if (!createResult.Succeeded)
        {
            throw new AuthenticationException();
        }
    }
}