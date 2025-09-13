using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WhatsApp.Messaging.Core.Interfaces;
using WhatsApp.Messaging.Twilio.Options;

namespace WhatsApp.Messaging.Twilio.Sending
{
    public sealed class TwilioMessageSender : IMessageSender
    {
        private readonly ITwilioRestClient _client;
        private readonly TwilioOptions _opts;
        private readonly ILogger<TwilioMessageSender> _logger;

        public TwilioMessageSender(IOptions<TwilioOptions> opts, ILogger<TwilioMessageSender> logger, ITwilioRestClient client)
        {
            _opts = opts.Value;
            _logger = logger;
            _client = client;
        }

        public async Task SendTextAsync(string to, string body, CancellationToken ct = default)
        {
            var from = _opts.FromNumber ?? throw new InvalidOperationException("TwilioOptions.FromNumber not set");
            var msg = await MessageResource.CreateAsync(
                from: new PhoneNumber(from),
                to: new PhoneNumber(to),
                body: body,
                client: _client
            );

            _logger.LogInformation("Twilio msg sent: Sid={Sid}", msg.Sid);
        }

        public async Task SendMediaAsync(string to, Uri mediaUrl, string? caption = null, CancellationToken ct = default)
        {
            var from = _opts.FromNumber ?? throw new InvalidOperationException("TwilioOptions.FromNumber not set");
            var msg = await MessageResource.CreateAsync(
                from: new PhoneNumber(from),
                to: new PhoneNumber(to),
                body: caption,
                mediaUrl: new List<Uri> { mediaUrl },
                client: _client
            );

            _logger.LogInformation("Twilio media msg sent: Sid={Sid}", msg.Sid);
        }
    }
}
