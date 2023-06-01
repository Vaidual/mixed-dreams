using System.ComponentModel.DataAnnotations;

namespace MixedDreams.Infrastructure.Features.AuthFeatures.Login
{
    public sealed record LoginRequest(
        string Email, 
        string Password, 
        bool RememberMe);
}
