using System.Threading;
using System.Threading.Tasks;

namespace Openddns.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken);
    }
}
