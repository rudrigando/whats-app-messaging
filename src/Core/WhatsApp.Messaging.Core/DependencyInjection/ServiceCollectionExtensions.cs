using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Interfaces;
using WhatsApp.Messaging.Core.Options;
using WhatsApp.Messaging.Core.Pipelines;
using WhatsApp.Messaging.Core.Utils.Phone;

namespace WhatsApp.Messaging.Core.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra serviços do núcleo agnóstico. Providers e storages adicionam extensões próprias.
        /// </summary>
        public static IServiceCollection AddWhatsAppCore(this IServiceCollection services, Action<MessagingOptions>? configure = null)
        {
            if (configure != null)
                services.Configure(configure);
            else
                services.AddOptions<MessagingOptions>();


            services.TryAddSingleton<IPhoneFormatter>(sp =>
            {
                var opts = sp.GetRequiredService<IOptions<MessagingOptions>>().Value;
                return opts.PhoneFormatter ?? new DefaultPhoneFormatter();
            });


            // Não registra IMessageSender/IWebhookParser/IConversationStore aqui — são específicos do provider/storage.


            services.TryAddSingleton<IIncomingMessagePipeline, DefaultIncomingMessagePipeline>();
            return services;
        }
    }
}
