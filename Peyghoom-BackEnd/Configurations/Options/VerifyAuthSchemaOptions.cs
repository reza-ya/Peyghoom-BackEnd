namespace Peyghoom_BackEnd.Options
{
    public class VerifyAuthSchemaOptions
    {
        public static readonly string VerifyAuthSchema = "VerifyAuthSchema";

        public string? Issuer { get; set; }
        public string? SecretKey { get; set; }
    }
}
