using Microsoft.EntityFrameworkCore;
using Openddns.Core.Interfaces;
using Openddns.Core.Models;

namespace Openddns.Infrastructure.Repositories
{
    public class Repository : IRepository
    {
        private readonly DatabaseContext _databaseContext;

        public IUnitOfWork UnitOfWork => _databaseContext;

        public Repository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<LogModel> AddLog(LogModel status, CancellationToken cancellationToken)
        {
            await _databaseContext.AddAsync(status, cancellationToken);
            return status;
        }

        public async Task<List<LogModel>> GetLogs(CancellationToken cancellationToken)
        {
            return await _databaseContext.Logs!.OrderByDescending(e => e.CreatedAt)
                .Take(100)
                .ToListAsync(cancellationToken);
        }
    }
}
