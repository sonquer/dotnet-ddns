using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Openddns.Application.InternetProtocol
{
    public interface IExternalInternetProtocolAddressRecognizer
    {
        Task<string> GetExternalAddress(IConfiguration configuration);
    }
}
