using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Twilio.Abstractions
{
    public interface ITwilioTemplateSender
    {
        Task SendTemplateAsync(
            string key, string to,
            IDictionary<string, object>? variables = null,
            CancellationToken ct = default);
    }
}
