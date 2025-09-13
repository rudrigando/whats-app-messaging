using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Dtos;

namespace WhatsApp.Messaging.Core.Interfaces
{
    /// <summary>
    /// Manipuladores de mensagens de entrada. Um pipeline pode registrar vários handlers.
    /// Retorna true para indicar que a mensagem foi tratada.
    /// </summary>
    public interface IIncomingMessageHandler
    {
        Task<bool> HandleAsync(IncomingMessage message, CancellationToken ct = default);
    }
}
