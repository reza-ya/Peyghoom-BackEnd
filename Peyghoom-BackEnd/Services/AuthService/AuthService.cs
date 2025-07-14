
using Peyghoom_BackEnd.Services.Types;

namespace Peyghoom_BackEnd.Services
{
    public class AuthService : IAuthService
    {
        public AuthService() { }


        public async Task<GenerateTokenResponse> GenerateTokenAsync(GenerateTokenRequest generateTokenRequest)
        {
            return new GenerateTokenResponse();
        }
    }
}
