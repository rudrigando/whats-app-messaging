//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WhatsApp.Messaging.Twilio.Abstractions;
using WhatsApp.Messaging.Twilio.Options;

namespace WhatsApp.Messaging.Twilio.Sending
{
    public sealed class TwilioTemplateSender : ITwilioTemplateSender
    {
        private readonly ITwilioRestClient _client;
        private readonly TwilioOptions _opts;
        private readonly ILogger<TwilioTemplateSender> _logger;

        public TwilioTemplateSender(ITwilioRestClient client, IOptions<TwilioOptions> opts, ILogger<TwilioTemplateSender> logger)
        {
            _client = client; _opts = opts.Value; _logger = logger;
        }

        public async Task SendTemplateAsync(string key, string to, IDictionary<string, object>? variables = null, CancellationToken ct = default)
        {
            if (!_opts.ContentTemplates.TryGetValue(key, out var tpl))
                throw new InvalidOperationException($"Template '{key}' não configurado em Twilio:ContentTemplates.");

            var from = _opts.FromNumber;
            if (string.IsNullOrWhiteSpace(from) && string.IsNullOrWhiteSpace(_opts.MessagingServiceSid))
                throw new InvalidOperationException("Configure Twilio:FromNumber ou Twilio:MessagingServiceSid.");

            var merged = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            if (tpl.Defaults is not null) foreach (var kv in tpl.Defaults) merged[kv.Key] = kv.Value;
            if (variables is not null) foreach (var kv in variables) merged[kv.Key] = kv.Value;

            var create = new CreateMessageOptions(new PhoneNumber(to))
            {
                ContentSid = tpl.Sid,
                ContentVariables = JsonSerializer.Serialize(merged)
            };
            if (!string.IsNullOrWhiteSpace(_opts.MessagingServiceSid)) create.MessagingServiceSid = _opts.MessagingServiceSid;
            else create.From = new PhoneNumber(from!);

            var msg = await MessageResource.CreateAsync(create, _client);
            _logger.LogInformation("Template '{Key}' enviado. MessageSid={Sid}", key, msg.Sid);
        }
    }
}



