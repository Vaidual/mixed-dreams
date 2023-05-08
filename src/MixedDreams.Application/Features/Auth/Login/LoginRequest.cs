using System.ComponentModel.DataAnnotations;

namespace MixedDreams.Application.Features.Auth.Login
{
    public sealed record LoginRequest(
        string Email, 
        string Password, 
        bool RememberMe);
}
