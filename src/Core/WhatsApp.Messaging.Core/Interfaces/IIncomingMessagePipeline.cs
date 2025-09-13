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
