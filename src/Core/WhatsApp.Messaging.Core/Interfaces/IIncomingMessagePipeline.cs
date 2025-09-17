//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Dtos;

namespace WhatsApp.Messaging.Core.Interfaces
{
    public interface IIncomingMessagePipeline
    {
        Task DispatchAsync(IncomingMessage message, CancellationToken ct = default);
    }
}



