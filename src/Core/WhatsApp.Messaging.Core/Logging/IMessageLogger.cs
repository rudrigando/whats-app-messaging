//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Core.Logging
{
    public interface IMessageLogger
    {
        void Trace(string message);
        void Warn(string message, Exception? ex = null);
        void Error(string message, Exception ex);
    }
}



