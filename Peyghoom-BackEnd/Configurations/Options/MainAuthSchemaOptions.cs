namespace Peyghoom_BackEnd.Options
{
    public class MainAuthSchemaOptions
    {
        public static readonly string MainAuthSchema = "MainAuthSchema";

        public string? Issuer { get; set; }
        public string? SecretKey { get; set; }
    }
}
