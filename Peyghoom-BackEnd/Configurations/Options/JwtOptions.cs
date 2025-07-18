namespace Peyghoom_BackEnd.Options
{
    public class JwtOptions
    {
        public static readonly string Jwt = "Jwt";

        public string? Issuer { get; set; }
        public string? AccessTokenSecretKey { get; set; }
        public string? RefreshTokenSecretKey { get; set; }
    }
}
