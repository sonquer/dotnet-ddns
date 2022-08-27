using Microsoft.EntityFrameworkCore;
using Openddns.Core.Interfaces;
using Openddns.Core.Models;

namespace Openddns.Infrastructure.SQLite.Repositories
{
    public class Repository : IRepository
    {
        private readonly DatabaseContext _databaseContext;

        public IUnitOfWork UnitOfWork => _databaseContext;

        public Repository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<StatusModel> AddStatus(StatusModel status, CancellationToken cancellationToken)
        {
            await _databaseContext.AddAsync(status, cancellationToken);
            return status;
        }

        public async Task<List<StatusModel>> GetStatuses(CancellationToken cancellationToken)
        {
            return await _databaseContext.Statuses.ToListAsync(cancellationToken);
        }
    }
}
