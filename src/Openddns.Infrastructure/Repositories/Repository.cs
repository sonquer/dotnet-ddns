using Microsoft.EntityFrameworkCore;
using Openddns.Core.Enum;
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

        public async Task<List<LogModel>> GetLogs(LogType[] excludedLogTypes, CancellationToken cancellationToken)
        {
            var logTypes = excludedLogTypes.Select(e => e.ToString())
                .ToList();

            return await _databaseContext.Logs!
                .Where(e => logTypes.Contains(e.Type) == false)
                .OrderByDescending(e => e.CreatedAt)
                .Take(100)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LogModel>> GetExpiredLogs(CancellationToken cancellationToken)
        {
            return await _databaseContext.Logs!
                .Where(e => e.CreatedAt <= DateTime.Now.AddDays(-31).Date)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public void DeleteLog(LogModel log)
        {
            _databaseContext.Logs!.Remove(log);
        }
    }
}
