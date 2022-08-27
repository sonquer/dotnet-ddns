using System.Threading.Tasks;

namespace Openddns.Application.InternetProtocol.Providers
{
    public interface IAddressProvider
    {
        Task<string> GetExternalAddress();
    }
}
