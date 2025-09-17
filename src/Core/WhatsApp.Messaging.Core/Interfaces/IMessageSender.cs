//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Core.Interfaces
{
    /// <summary>
    /// Envio de mensagens (texto/mídia). Providers (Twilio, Meta Cloud, etc.) implementam esta interface.
    /// </summary>
    public interface IMessageSender
    {
        Task SendTextAsync(string to, string body, CancellationToken ct = default);
        Task SendMediaAsync(string to, Uri mediaUrl, string? caption = null, CancellationToken ct = default);
    }
}
