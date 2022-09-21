using MediatR;
using Openddns.Core.Interfaces;
using Openddns.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Openddns.Application.Queries.Handlers
{
    public class GetLogsQueryHandler : IRequestHandler<GetLogsQuery, List<LogModel>>
    {
        private readonly IRepository _repository;

        public GetLogsQueryHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<LogModel>> Handle(GetLogsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetLogs(request.ExcludedLogTypes, cancellationToken);
        }
    }
}
