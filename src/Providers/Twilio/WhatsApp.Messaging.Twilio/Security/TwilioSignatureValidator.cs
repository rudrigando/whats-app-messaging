using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Twilio.Abstractions;

namespace WhatsApp.Messaging.Twilio.Security
{
    public sealed class TwilioSignatureValidator : ITwilioSignatureValidator
    {
        public bool IsValid(string fullUrl, IReadOnlyDictionary<string, string> form, string signature, string authToken)
        {
            // Concatena URL + pares de form (ordenados alfabeticamente por chave)
            var sb = new StringBuilder(fullUrl);
            foreach (var kv in form.OrderBy(kv => kv.Key, StringComparer.Ordinal))
                sb.Append(kv.Key).Append(kv.Value);

            using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(authToken));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            var computed = Convert.ToBase64String(hash);

            // Comparação constante é melhor; aqui usamos Ordinal como baseline
            return string.Equals(computed, signature, StringComparison.Ordinal);
        }
    }
}
