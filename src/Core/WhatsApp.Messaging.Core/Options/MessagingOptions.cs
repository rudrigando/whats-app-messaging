//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Interfaces;

namespace WhatsApp.Messaging.Core.Options
{
    public class MessagingOptions
    {
        /// <summary>Remetente padrão (E.164), se aplicável para o provider em uso.</summary>
        public string? DefaultFrom { get; set; }

        /// <summary>Formatter de telefone; default = <see cref="DefaultPhoneFormatter"/>.</summary>
        public IPhoneFormatter? PhoneFormatter { get; set; }

        /// <summary>Timeouts padrão para operações externas.</summary>
        public TimeSpan SendTimeout { get; set; } = TimeSpan.FromSeconds(20);
    }
}



