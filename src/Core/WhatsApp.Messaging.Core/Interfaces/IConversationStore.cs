using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Dtos;

namespace WhatsApp.Messaging.Core.Interfaces
{
    /// <summary>
    /// Armazenamento de conversas opcional (EFCore, Dapper, memória, etc.).
    /// </summary>
    public interface IConversationStore
    {
        Task<Conversation?> GetByUserAsync(string userId, CancellationToken ct = default);
        Task UpsertAsync(Conversation conversation, CancellationToken ct = default);
        Task AppendMessageAsync(string userId, IncomingMessage message, CancellationToken ct = default);
    }
}
