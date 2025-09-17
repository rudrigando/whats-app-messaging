//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Twilio.Http
{
    /// <summary>
    /// Políticas Polly para requests à API Twilio.
    /// </summary>
    public static class Policies
    {
        /// <summary>
        /// Retry com suporte a Retry-After (429) + fallback para backoff exponencial com jitter.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> RetryPolicy(int retryCount = 5)
        {
            var jitterDelays = Backoff.ExponentialBackoff(
                initialDelay: TimeSpan.FromMilliseconds(200),
                retryCount: retryCount,
                factor: 2.0,
                fastFirst: true);

            return Policy<HttpResponseMessage>
                .Handle<TimeoutRejectedException>()         // timeout da policy
                .OrResult(r => IsTransientOrRateLimited(r))
                .WaitAndRetryAsync(
                    retryCount: jitterDelays.Count(),
                    sleepDurationProvider: (attempt, outcome, ctx) =>
                    {
                        // Se 429 e tiver Retry-After, respeita
                        if (outcome.Result is { } resp && resp.StatusCode == (HttpStatusCode)429)
                        {
                            if (TryGetServerRetryAfter(resp.Headers, out var serverDelay))
                                return serverDelay;
                        }
                        // Caso contrário, usa o jitter (attempt é 1-based)
                        return jitterDelays.ElementAt(attempt - 1);
                    },
                    onRetryAsync: async (outcome, delay, attempt, ctx) =>
                    {
                        // Aqui você pode logar com ILogger via ctx (se tiver injetado)
                        // ex.: ctx.TryGetValue("Logger", out var l) ...
                        await System.Threading.Tasks.Task.CompletedTask;
                    });
        }

        /// <summary>
        /// Timeout por tentativa (além de HttpClient.Timeout).
        /// Sugestão: 10s por tentativa.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy(TimeSpan? perTryTimeout = null)
        {
            var t = perTryTimeout ?? TimeSpan.FromSeconds(10);
            // Padrão é TimeoutStrategy.Optimistic; para HttpClient isso é OK pois
            // um cancelamento de token aborta a requisição.
            return Policy.TimeoutAsync<HttpResponseMessage>(t);
        }

        /// <summary>
        /// Circuit breaker para proteger contra falhas contínuas.
        /// Sugestão: 5 falhas → abre 30s.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy(
            int failuresBeforeBreak = 5,
            TimeSpan? breakDuration = null)
        {
            var duration = breakDuration ?? TimeSpan.FromSeconds(30);

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => r.StatusCode == (HttpStatusCode)429)
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: failuresBeforeBreak,
                    durationOfBreak: duration);
        }

        /// <summary>
        /// (Opcional) Bulkhead para limitar concorrência.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> BulkheadPolicy(int maxConcurrent, int maxQueue)
            => Policy.BulkheadAsync<HttpResponseMessage>(maxConcurrent, maxQueue);

        /// <summary>
        /// (Opcional) Fallback para retornar 503 custom quando todas as policies falham.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> FallbackPolicy()
        {
            return Policy<HttpResponseMessage>
                .Handle<Exception>()
                .OrResult(r => (int)r.StatusCode >= 500)
                .FallbackAsync(
                    fallbackAction: _ =>
                    {
                        var resp = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                        {
                            Content = new StringContent("Twilio service temporarily unavailable (fallback)")
                        };
                        return System.Threading.Tasks.Task.FromResult(resp);
                    });
        }

        private static bool IsTransientOrRateLimited(HttpResponseMessage r)
        {
            if (r is null) return false;
            if ((int)r.StatusCode >= 500) return true;               // 5xx
            if (r.StatusCode == HttpStatusCode.RequestTimeout) return true; // 408
            if (r.StatusCode == (HttpStatusCode)429) return true;    // Too Many Requests
            return false;
        }

        private static bool TryGetServerRetryAfter(HttpResponseHeaders headers, out TimeSpan delay)
        {
            delay = default;
            if (headers?.RetryAfter is null) return false;

            if (headers.RetryAfter.Delta is TimeSpan delta)
            {
                delay = delta;
                return true;
            }
            if (headers.RetryAfter.Date is DateTimeOffset date)
            {
                var now = DateTimeOffset.UtcNow;
                delay = date > now ? (date - now) : TimeSpan.Zero;
                return true;
            }
            return false;
        }
    }
}



