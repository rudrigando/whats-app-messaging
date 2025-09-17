//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Core.Utils.Text
{
    public static class MessageNormalizer
    {
        /// <summary>
        /// Normaliza texto para matching: lower, trim, sem acentos, sem pontuação e espaços compactados.
        /// </summary>
        public static string Normalize(
        string text,
        bool toLower = true,
        bool trim = true,
        bool removeDiacritics = true,
        bool removePunctuation = true,
        bool collapseWhitespace = true)
        {
            if (text == null) return string.Empty;
            var t = text;
            if (trim) t = t.Trim();
            if (removeDiacritics) t = RemoveDiacritics(t);
            if (removePunctuation) t = RemovePunctuation(t);
            if (collapseWhitespace) t = CollapseWhitespace(t);
            if (toLower) t = t.ToLowerInvariant();
            return t;
        }

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text ?? string.Empty;
            var formD = text.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder(formD.Length);
            foreach (var ch in formD)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

        public static string RemovePunctuation(string text)
        {
            if (string.IsNullOrEmpty(text)) return text ?? string.Empty;
            var sb = new System.Text.StringBuilder(text.Length);
            foreach (var ch in text)
            {
                if (!char.IsPunctuation(ch) && !char.IsControl(ch))
                    sb.Append(ch);
            }
            return sb.ToString();
        }


        public static string CollapseWhitespace(string text)
        {
            if (string.IsNullOrEmpty(text)) return text ?? string.Empty;
            var sb = new System.Text.StringBuilder(text.Length);
            bool prevWs = false;
            foreach (var ch in text)
            {
                var isWs = char.IsWhiteSpace(ch);
                if (isWs)
                {
                    if (!prevWs) sb.Append(' ');
                    prevWs = true;
                }
                else
                {
                    sb.Append(ch);
                    prevWs = false;
                }
            }
            return sb.ToString().Trim();
        }

        // Helpers para fluxos de conversa comuns
        public static string NormalizeForMatch(string text) => Normalize(text, true, true, true, true, true);
        public static bool IsYes(string text)
        {
            var t = NormalizeForMatch(text);
            return t is "sim" or "s" or "yes" or "y" or "ok" or "okay";
        }
        public static bool IsNo(string text)
        {
            var t = NormalizeForMatch(text);
            return t is "nao" or "não" or "n" or "no";
        }
        public static bool IsStop(string text)
        {
            var t = NormalizeForMatch(text);
            return t is "parar" or "pare" or "stop" or "cancelar" or "cancel";
        }
    }
}



