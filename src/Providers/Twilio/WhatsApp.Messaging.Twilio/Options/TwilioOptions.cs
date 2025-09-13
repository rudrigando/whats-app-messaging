using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Options;

namespace WhatsApp.Messaging.Twilio.Options
{
    public sealed class TwilioOptions : MessagingOptions
    {
        public string AccountSid { get; set; } = default!;
        public string AuthToken { get; set; } = default!;
        public string? FromNumber { get; set; }              // e.g. "whatsapp:+14155238886"
        public string? MessagingServiceSid { get; set; }     // alternativa ao FromNumber
        public Uri? BaseUrl { get; set; }                    // override para testes/local]
    }
}
