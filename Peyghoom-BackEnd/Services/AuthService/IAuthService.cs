using Peyghoom_BackEnd.Services.Types;

namespace Peyghoom_BackEnd.Services
{
    public interface IAuthService
    {
        Task<GenerateTokenResponse> GenerateTokenAsync(GenerateTokenRequest generateTokenRequest);
    }
}