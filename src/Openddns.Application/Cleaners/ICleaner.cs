using System.Threading;
using System.Threading.Tasks;

namespace Openddns.Application.Cleaners
{
    public interface ICleaner<T> where T : class
    {
        Task Execute(CancellationToken cancellationToken);
    }
}
