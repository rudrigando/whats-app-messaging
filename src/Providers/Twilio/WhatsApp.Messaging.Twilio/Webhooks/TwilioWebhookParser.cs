//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Dtos;
using WhatsApp.Messaging.Core.Interfaces;
using WhatsApp.Messaging.Twilio.Abstractions;
using WhatsApp.Messaging.Twilio.Options;

namespace WhatsApp.Messaging.Twilio.Webhooks
{
    public sealed class TwilioWebhookParser : IWebhookParser
    {
        private readonly ITwilioSignatureValidator _validator;
        private readonly TwilioOptions _opts;

        public TwilioWebhookParser(ITwilioSignatureValidator validator, IOptions<TwilioOptions> opts)
        {
            _validator = validator;
            _opts = opts.Value;
        }

        public bool CanParse(WebhookRequest request)
            => request.Headers.ContainsKey("X-Twilio-Signature");

        public Task<IncomingMessage> ParseAsync(WebhookRequest request, CancellationToken ct = default)
        {
            if (!request.Headers.TryGetValue("X-Twilio-Signature", out var sig))
                throw new InvalidOperationException("Missing X-Twilio-Signature header");

            // Você passa a URL pública do endpoint do webhook (configurada no Twilio)
            var fullUrl = request.Headers.TryGetValue("X-Webhook-Url", out var url) ? url : string.Empty;
            if (!_validator.IsValid(fullUrl, request.Form, sig, _opts.AuthToken))
                throw new UnauthorizedAccessException("Invalid Twilio signature");

            var from = request.Form.TryGetValue("From", out var f) ? f : "";
            var body = request.Form.TryGetValue("Body", out var b) ? b : "";

            var msg = new IncomingMessage(
                From: from,
                Text: body ?? string.Empty,
                ReceivedAt: DateTimeOffset.UtcNow,
                Metadata: request.Form
            );
            return Task.FromResult(msg);
        }
    }
}



