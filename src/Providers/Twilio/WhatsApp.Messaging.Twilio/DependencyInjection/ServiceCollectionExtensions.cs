//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Net;
using Twilio.Clients;
using Twilio.Http;
using WhatsApp.Messaging.Core.DependencyInjection;
using WhatsApp.Messaging.Core.Interfaces;
using WhatsApp.Messaging.Twilio.Abstractions;
using WhatsApp.Messaging.Twilio.Http;
using WhatsApp.Messaging.Twilio.Options;
using WhatsApp.Messaging.Twilio.Security;
using WhatsApp.Messaging.Twilio.Sending;
using WhatsApp.Messaging.Twilio.Webhooks;

namespace WhatsApp.Messaging.Twilio.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private const string TwilioClientName = "twilio";

        /// <summary>
        /// Registra o Core + Provider Twilio lendo de appsettings (seção "Twilio").
        /// </summary>
        public static IServiceCollection AddWhatsAppMessagingTwilio(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddWhatsAppCore()
                .AddTwilioWhatsApp(configuration.GetSection("Twilio"));
        }

        /// <summary>
        /// Variante com delegate (sem IConfiguration), útil para testes/segredos externos.
        /// </summary>
        public static IServiceCollection AddWhatsAppMessagingTwilio(
            this IServiceCollection services,
            Action<TwilioOptions> configure)
        {
            return services
                .AddWhatsAppCore()
                .AddTwilioWhatsApp(configure);
        }

        /// <summary>
        /// Registro via IConfiguration (ex.: builder.Configuration.GetSection("Twilio")).
        /// </summary>
        private static IServiceCollection AddTwilioWhatsApp(
            this IServiceCollection services,
            IConfiguration section,
            Action<IHttpClientBuilder>? configureHttpClient = null)
        {
            services.AddOptions<TwilioOptions>()
                    .Bind(section)
                    .Validate(o => !string.IsNullOrWhiteSpace(o.AccountSid), "Twilio:AccountSid ausente")
                    .Validate(o => !string.IsNullOrWhiteSpace(o.AuthToken), "Twilio:AuthToken ausente")
                    .Validate(o => !string.IsNullOrWhiteSpace(o.FromNumber) || !string.IsNullOrWhiteSpace(o.MessagingServiceSid),
                              "Configure Twilio:FromNumber ou Twilio:MessagingServiceSid");

            return AddTwilioInternal(services, configureHttpClient);
        }

        /// <summary>
        /// Registro via delegate (útil para testes ou valores calculados).
        /// </summary>
        private static IServiceCollection AddTwilioWhatsApp(
            this IServiceCollection services,
            Action<TwilioOptions> configure,
            Action<IHttpClientBuilder>? configureHttpClient = null)
        {
            services.AddOptions<TwilioOptions>()
                    .Configure(configure)
                    .Validate(o => !string.IsNullOrWhiteSpace(o.AccountSid), "Twilio:AccountSid ausente")
                    .Validate(o => !string.IsNullOrWhiteSpace(o.AuthToken), "Twilio:AuthToken ausente")
                    .Validate(o => !string.IsNullOrWhiteSpace(o.FromNumber) || !string.IsNullOrWhiteSpace(o.MessagingServiceSid),
                              "Configure Twilio:FromNumber ou Twilio:MessagingServiceSid");

            return AddTwilioInternal(services, configureHttpClient);
        }

        private static IServiceCollection AddTwilioInternal(
            IServiceCollection services,
            Action<IHttpClientBuilder>? configureHttpClient)
        {
            // HttpClient nomeado: aplica BaseAddress/Timeout de TwilioOptions e configura o handler
            var httpBuilder = services
                .AddHttpClient(TwilioClientName, static (sp, http) =>
                {
                    var opts = sp.GetRequiredService<IOptions<TwilioOptions>>().Value;

                    http.Timeout = opts.SendTimeout > TimeSpan.Zero
                        ? opts.SendTimeout
                        : TimeSpan.FromSeconds(20);

                    if (opts.BaseUrl is not null)
                        http.BaseAddress = opts.BaseUrl;
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                    new SocketsHttpHandler
                    {
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli,
                        PooledConnectionLifetime = TimeSpan.FromMinutes(2), // renova conexões para respeitar DNS/endpoint changes
                        // Você pode ajustar Proxy/SSL/etc. aqui se precisar
                    });

            // ORDEM IMPORTA (outer -> inner)
            httpBuilder
                .AddPolicyHandler(Policies.CircuitBreakerPolicy()) // outer
                .AddPolicyHandler(Policies.RetryPolicy())          // middle
                .AddPolicyHandler(Policies.TimeoutPolicy());       // inner

            // Permite ao host aplicar policies adicionais (ex.: bulkhead/fallback)
            configureHttpClient?.Invoke(httpBuilder);

            // ITwilioRestClient usando o HttpClient nomeado
            services.TryAddSingleton<ITwilioRestClient>(sp =>
            {
                var opts = sp.GetRequiredService<IOptions<TwilioOptions>>().Value;
                var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient(TwilioClientName);
                var systemNet = new SystemNetHttpClient(http);

                return new TwilioRestClient(
                    username: opts.AccountSid,
                    password: opts.AuthToken,
                    httpClient: systemNet,
                    accountSid: opts.AccountSid
                // region/edge podem ser configurados aqui, se necessário
                );
            });

            // Segurança e integrações do provider
            services.TryAddSingleton<ITwilioSignatureValidator, TwilioSignatureValidator>();
            services.AddSingleton<IWebhookParser, TwilioWebhookParser>();
            services.AddSingleton<IMessageSender, TwilioMessageSender>();
            services.AddSingleton<ITwilioTemplateSender, TwilioTemplateSender>();

            return services;
        }
    }
}



