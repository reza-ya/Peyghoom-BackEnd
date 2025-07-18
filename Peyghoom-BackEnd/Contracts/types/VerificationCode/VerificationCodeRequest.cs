namespace Peyghoom_BackEnd.Contracts.types
{
    public class VerificationCodeRequest
    {
        public long PhoneNumber { get; set; }
        public int CountryCode { get; set; }
    }
}
