using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Storage.EFCore.Entities
{
    public class ConversationMessageEntity
    {
        public long Id { get; set; }
        public Guid ConversationId { get; set; }
        public ConversationEntity Conversation { get; set; } = default!;
        public DateTimeOffset At { get; set; }
        public string From { get; set; } = default!; // E.164 do remetente
        public string Text { get; set; } = default!;
        public string MetadataJson { get; set; } = "{}";
    }
}
