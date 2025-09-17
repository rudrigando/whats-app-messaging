//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Twilio.Options
{
    public sealed class TwilioContentTemplate
    {
        public string Sid { get; set; } = default!;
        public string? Channel { get; set; }
        public string? Locale { get; set; }
        public Dictionary<string, object>? Defaults { get; set; }
    }
}



