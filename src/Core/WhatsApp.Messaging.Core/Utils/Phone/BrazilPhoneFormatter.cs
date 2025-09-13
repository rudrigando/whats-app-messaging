using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Interfaces;

namespace WhatsApp.Messaging.Core.Utils.Phone
{
    /// <summary>
    /// Formatador para Brasil (código do país 55). Converte para E.164 e, opcionalmente,
    /// insere o 9º dígito em celulares quando ausente.
    /// </summary>
    public sealed class BrazilPhoneFormatter : IPhoneFormatter
    {
        /// <summary>
        /// Se true, ao detectar número com 8 dígitos após DDD (padrão fixo),
        /// adiciona automaticamente o nono dígito para celulares.
        /// </summary>
        public bool AutoAddNinthDigit { get; }


        public BrazilPhoneFormatter(bool autoAddNinthDigit = true)
        => AutoAddNinthDigit = autoAddNinthDigit;


        public string ToE164(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new System.ArgumentException("phone is empty", nameof(input));


            var digits = new string(input.Trim().ToCharArray());
            digits = new string(System.Linq.Enumerable.Where(digits, char.IsDigit).ToArray());


            // Remove zero inicial nacional (ex.: 031) se houver
            digits = digits.TrimStart('0');


            // Garante código do país 55
            if (!digits.StartsWith("55"))
            {
                // Heurística: se veio com 10 (fixo) ou 11 (móvel) dígitos nacionais, prefixa 55
                if (digits.Length is 10 or 11)
                    digits = "55" + digits;
                else if (digits.Length > 0 && !digits.StartsWith("55"))
                    digits = "55" + digits; // fallback conservador
            }


            // Agora esperamos: 55 + DDD(2) + local(8 ou 9)
            if (!digits.StartsWith("55") || digits.Length < 12)
                throw new System.FormatException("invalid BR number");


            var afterCc = digits.Substring(2); // DDD + local
            if (afterCc.Length < 10 || afterCc.Length > 11)
            {
                // Se vier além de 11, tente remover zeros à esquerda redundantes
                afterCc = afterCc.TrimStart('0');
            }


            // Recalcula
            if (afterCc.Length is < 10 or > 11)
                throw new System.FormatException("invalid BR local length");


            var ddd = afterCc.Substring(0, 2);
            var local = afterCc.Substring(2);


            // Inserção do 9º dígito (heurística): se for 8 dígitos e AutoAddNinthDigit==true
            if (local.Length == 8 && AutoAddNinthDigit)
            {
                // Adiciona '9' no começo do local → móvel de 9 dígitos
                local = '9' + local;
            }


            var normalized = "+55" + ddd + local;
            // Validações finais básicas
            if (normalized.Length != 3 + 2 + 9 && normalized.Length != 3 + 2 + 8)
                throw new System.FormatException("unexpected normalized length");


            return normalized;
        }


        public string DigitsOnly(string input)
        => new string(System.Linq.Enumerable.Where(input ?? string.Empty, char.IsDigit).ToArray());
    }
}
