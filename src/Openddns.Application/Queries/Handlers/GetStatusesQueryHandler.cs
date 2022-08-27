using MediatR;
using Openddns.Core.Interfaces;
using Openddns.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Openddns.Application.Queries.Handlers
{
    public class GetStatusesQueryHandler : IRequestHandler<GetStatusesQuery, List<StatusModel>>
    {
        private readonly IRepository _repository;

        public GetStatusesQueryHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<StatusModel>> Handle(GetStatusesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetStatuses(cancellationToken);
        }
    }
}
