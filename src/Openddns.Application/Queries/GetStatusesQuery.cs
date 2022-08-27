using MediatR;
using Openddns.Core.Models;
using System.Collections.Generic;

namespace Openddns.Application.Queries
{
    public class GetStatusesQuery : IRequest<List<StatusModel>>
    {
    }
}
