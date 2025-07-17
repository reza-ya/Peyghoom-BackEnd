
using Peyghoom_BackEnd.Applications;

namespace Peyghoom_BackEnd.Services
{
    public interface IOTPService
    {
        Result SendCode(long phoneNumber, int countery);
        Result VerifyCode(long phoneNumber, int code);
    }
}