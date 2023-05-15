namespace WebApi.Features;

public static class LoginUser
{
    public class InputModel
    {
        public required string Username { get; set; } = string.Empty;

        public required string Password { get; set; } = string.Empty;
    }
}