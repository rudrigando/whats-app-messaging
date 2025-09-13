using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Dtos;

namespace WhatsApp.Messaging.Core.Interfaces
{
    /// <summary>
    /// Parser de webhook: identifica e traduz uma requisição bruta em um <see cref="IncomingMessage"/>.
    /// </summary>
    public interface IWebhookParser
    {
        bool CanParse(WebhookRequest request);
        Task<IncomingMessage> ParseAsync(WebhookRequest request, CancellationToken ct = default);
    }
}
