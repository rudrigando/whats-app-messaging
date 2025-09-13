using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Core.Dtos
{
    /// <summary>
    /// Registro de conversa simples/agnóstico para armazenamento opcional.
    /// </summary>
    public sealed class Conversation
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string UserId { get; set; } = default!;
        public string? UserName { get; set; }
        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? LastMessageAt { get; set; }
        public bool? AllowsContact { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new();
    }
}
