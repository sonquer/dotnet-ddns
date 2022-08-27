using System;

namespace Openddns.Core.Models
{
    public class StatusModel
    {
        public Guid Id { get; set; }

        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; }

        public StatusModel(string? message)
        {
            Id = Guid.NewGuid();
            Message = message;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
