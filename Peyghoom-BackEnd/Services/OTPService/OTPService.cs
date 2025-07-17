using Microsoft.Extensions.Caching.Memory;
using Peyghoom_BackEnd.Applications;

namespace Peyghoom_BackEnd.Services
{
    public class OTPService : IOTPService
    {
        private IMemoryCache _memoryCache;
        private Random _rnd;

        private readonly int _randomMaxValue = 100_000;
        private readonly int _randomMinVlaue = 999_999;

        public OTPService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _rnd = new Random();
        }

        public Result SendCode(long phoneNumber, int countery)
        {
            int randomNumber = _rnd.Next(_randomMaxValue, _randomMinVlaue);
            _memoryCache.Set(phoneNumber, randomNumber, TimeSpan.FromSeconds(120));

            return Result.Success();
        }


        public Result VerifyCode(long phoneNumber, int code)
        {
            var cacheCode = _memoryCache.Get<int>(phoneNumber);
            if (cacheCode == code)
            {
                return Result.Success();
            }
            else
            {
                return Result.Failure(OTPServiceErrors.NotMatch);
            }
        }
    }
}
