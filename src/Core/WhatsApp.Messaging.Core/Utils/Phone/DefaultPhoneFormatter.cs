//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Interfaces;

namespace WhatsApp.Messaging.Core.Utils.Phone
{
    /// <summary>
    /// Implementação simples e neutra de E.164.
    /// </summary>
    public sealed class DefaultPhoneFormatter : IPhoneFormatter
    {
        public string ToE164(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("phone is empty", nameof(input));


            var digits = new string(input.Trim().Where(char.IsDigit).ToArray());
            if (digits.Length < 8)
                throw new FormatException("phone too short");


            // Assume que já inclui DDI; consumidor pode decorar com regras locais.
            return "+" + digits.TrimStart('+');
        }


        public string DigitsOnly(string input) => new string(input.Where(char.IsDigit).ToArray());
    }
}



