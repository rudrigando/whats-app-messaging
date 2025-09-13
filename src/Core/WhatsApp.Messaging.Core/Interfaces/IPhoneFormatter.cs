using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Core.Interfaces
{
    public interface IPhoneFormatter
    {
        /// <summary>
        /// Normaliza um número para E.164 (ex.: +5531999998888). Deve lançar se inválido.
        /// </summary>
        string ToE164(string input);


        /// <summary>
        /// Retorna somente dígitos (para logs/armazenamento auxiliar), preservando DDI/DDI.
        /// </summary>
        string DigitsOnly(string input);
    }
}
