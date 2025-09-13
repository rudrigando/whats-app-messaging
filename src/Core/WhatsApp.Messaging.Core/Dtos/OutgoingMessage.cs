using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Core.Dtos
{
    /// <summary>
    /// Representa uma mensagem de saída que será enviada ao destinatário.
    /// </summary>
    public sealed record OutgoingMessage(
        string To,
        string Body,
        Uri? MediaUrl = null,
        string? Caption = null,
        IReadOnlyDictionary<string, string>? Metadata = null
    );
}
