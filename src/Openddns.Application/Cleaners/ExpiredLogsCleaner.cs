using Microsoft.Extensions.DependencyInjection;
using Openddns.Core.Interfaces;
using Openddns.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Openddns.Application.Cleaners
{
    public class ExpiredLogsCleaner : ICleaner<LogModel>
    {
        private readonly IServiceProvider _serviceProvider;

        public ExpiredLogsCleaner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository>();

            var logsToRemove = await repository.GetExpiredLogs(cancellationToken);
            logsToRemove.ForEach(log => repository.DeleteLog(log));

            await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
