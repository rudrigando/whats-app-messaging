//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Core.Dtos
{
    /// <summary>
    /// Representa uma requisição de webhook genérica (sem dependência de ASP.NET).
    /// Providers convertem do ambiente (ex.: HttpRequest) para este contrato.
    /// </summary>
    public sealed class WebhookRequest
    {
        public required IReadOnlyDictionary<string, string> Headers { get; init; }
        public required IReadOnlyDictionary<string, string> Query { get; init; }
        public required IReadOnlyDictionary<string, string> Form { get; init; }
        public Stream? Body { get; init; }
        public string? RawBody { get; init; }
    }
}


