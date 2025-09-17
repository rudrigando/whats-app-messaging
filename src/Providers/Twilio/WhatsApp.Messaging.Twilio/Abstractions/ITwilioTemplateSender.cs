//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

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



