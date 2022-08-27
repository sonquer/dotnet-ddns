using Openddns.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Openddns.Core.Interfaces
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<StatusModel> AddStatus(StatusModel status, CancellationToken cancellationToken);

        Task<List<StatusModel>> GetStatuses(CancellationToken cancellationToken);
    }
}
