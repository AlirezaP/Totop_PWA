using OtpNet;
using System.Text;

namespace BlazorApp2.Client.Services
{
    public class TotpService
    {
        public static TotpService Create(string key1, string loginCode, int step = 30, int totpSize = 6)
        {
            return new TotpService(key1, loginCode, step, totpSize);
        }

        private string _loginCode;
        private string _keysec;
        private string _activeCode;
        private int _step;
        private int _totpSize;
        private Totp _totp;

        public string LoginCode { get { return _loginCode; } }

        public TotpService(string key, string loginCode, int step = 30, int totpSize = 6)
        {
            _step = step;
            _keysec = key;
            _loginCode = loginCode;
            _totpSize = totpSize;

            var buf = Encoding.UTF8.GetBytes(key);

            _totp = new Totp(buf, step: step, totpSize: totpSize);
        }
        public string GetOtp()
        {
            var totpCode = _totp.ComputeTotp(DateTime.UtcNow);
            
            return totpCode;
        }

        public int Remaining()
        {
           return _totp.RemainingSeconds();
        }
    }
}
