namespace Peyghoom_BackEnd.Contracts.types.VerificationCode
{
    public class VerificationCodeRequest
    {
        public long PhoneNumber { get; set; }
        public int CountryCode { get; set; }
    }
}
