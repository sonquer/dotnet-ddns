using MediatR;
using Openddns.Core.Enum;
using Openddns.Core.Models;
using System.Collections.Generic;

namespace Openddns.Application.Queries
{
    public class GetLogsQuery : IRequest<List<LogModel>>
    {
        public LogType[] ExcludedLogTypes { get; }

        public GetLogsQuery(LogType[] excludedLogTypes)
        {
            ExcludedLogTypes = excludedLogTypes;
        }
    }
}
