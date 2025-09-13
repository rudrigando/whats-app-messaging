using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Dtos;
using WhatsApp.Messaging.Core.Interfaces;

namespace WhatsApp.Messaging.Core.Pipelines
{
    internal sealed class DefaultIncomingMessagePipeline : IIncomingMessagePipeline
    {
        private readonly IEnumerable<IIncomingMessageHandler> _handlers;
        private readonly ILogger<DefaultIncomingMessagePipeline> _logger;


        public DefaultIncomingMessagePipeline(
        IEnumerable<IIncomingMessageHandler> handlers,
        ILogger<DefaultIncomingMessagePipeline> logger)
        {
            _handlers = handlers;
            _logger = logger;
        }


        public async Task DispatchAsync(IncomingMessage message, CancellationToken ct = default)
        {
            foreach (var handler in _handlers)
            {
                try
                {
                    if (await handler.HandleAsync(message, ct).ConfigureAwait(false))
                        return; // tratado
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro no handler {Handler}", handler.GetType().Name);
                    // continua para próximos handlers
                }
            }
            _logger.LogInformation("Mensagem de {From} não tratada por nenhum handler", message.From);
        }
    }
}
