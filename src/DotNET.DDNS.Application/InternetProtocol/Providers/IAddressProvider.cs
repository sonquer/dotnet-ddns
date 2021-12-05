using System.Threading.Tasks;

namespace DotNET.DDNS.Application.InternetProtocol.Providers
{
    public interface IAddressProvider
    {
        Task<string> GetExternalAddress();
    }
}
