using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Core.Dtos
{
    /// <summary>
    /// Representa uma mensagem de entrada recebida via webhook.
    /// </summary>
    public sealed record IncomingMessage(
        string From,
        string Text,
        DateTimeOffset ReceivedAt,
        IReadOnlyDictionary<string, string>? Metadata = null
    );
}
