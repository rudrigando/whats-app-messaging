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
    public interface ITwilioSignatureValidator
    {
        bool IsValid(string url, IReadOnlyDictionary<string, string> form, string signature, string authToken);
    }
}



