using Openddns.Core.Enum;
using System;

namespace Openddns.Core.Models
{
    public class LogModel
    {
        public Guid Id { get; set; }

        public string Message { get; set; }

        public string Provider { get; set; }

        public string Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public LogModel(string provider, string message, LogType type)
        {
            Id = Guid.NewGuid();
            Message = message;
            Type = type.ToString();
            Provider = provider;
            CreatedAt = DateTime.UtcNow;
        }

        protected LogModel()
        {
        }
    }
}
