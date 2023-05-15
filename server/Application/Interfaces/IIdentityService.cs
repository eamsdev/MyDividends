namespace Application.Interfaces;

public interface IIdentityService
{
    Task CreateUserAsync(string username, string password);
}