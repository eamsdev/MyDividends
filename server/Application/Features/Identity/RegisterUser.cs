using Application.Interfaces;
using MediatR;

namespace Application.Features.Identity;

public static class RegisterUser
{
    public class InputModel : IRequest
    {
        public required string Username { get; set; } = string.Empty;

        public required string Password { get; set; } = string.Empty;
    }

    public class ResponseModel
    { }

    public class Handler : IRequestHandler<InputModel>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task Handle(InputModel request, CancellationToken cancellationToken)
        {
            await _identityService.CreateUserAsync(request.Username, request.Password);
        }
    }
}