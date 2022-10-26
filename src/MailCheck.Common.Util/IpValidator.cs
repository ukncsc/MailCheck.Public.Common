using System.Net;
using System.Text.RegularExpressions;

namespace MailCheck.Common.Util
{
    public interface IIpValidator
    {
        bool IsValidIp(string ip);
    }

    public class IpValidator : IIpValidator
    {
        IPAddress _address;

        public bool IsValidIp(string ip)
        {
            return IPAddress.TryParse(ip, out _address);
        }
    }
}
