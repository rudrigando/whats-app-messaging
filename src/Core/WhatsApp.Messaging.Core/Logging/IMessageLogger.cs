using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Core.Logging
{
    public interface IMessageLogger
    {
        void Trace(string message);
        void Warn(string message, Exception? ex = null);
        void Error(string message, Exception ex);
    }
}
