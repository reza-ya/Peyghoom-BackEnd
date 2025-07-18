namespace Peyghoom_BackEnd.AAA
{
    public static class AuthorizationPolicy
    {
        public static readonly string PhoneNumberPolicy = nameof(PhoneNumberPolicy);
        public static readonly string PhoneVerifiedPolicy = nameof(PhoneVerifiedPolicy);
        public static readonly string RegisteredPolicy = nameof(RegisteredPolicy);
    }
}
