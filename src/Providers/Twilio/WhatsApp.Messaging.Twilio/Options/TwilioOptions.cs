//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

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
        public string? FromNumber { get; set; } 
        public string? MessagingServiceSid { get; set; }
        public Uri? BaseUrl { get; set; }

        public Dictionary<string, TwilioContentTemplate> ContentTemplates { get; set; }
           = new(StringComparer.OrdinalIgnoreCase);
    }
}



