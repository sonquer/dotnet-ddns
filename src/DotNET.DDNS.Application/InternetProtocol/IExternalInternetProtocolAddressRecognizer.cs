using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DotNET.DDNS.Application.InternetProtocol
{
    public interface IExternalInternetProtocolAddressRecognizer
    {
        Task<string> GetExternalAddress(IConfiguration configuration);
    }
}
