//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Storage.EFCore.Entities
{
    public class ConversationEntity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = default!; // E.164
        public string? UserName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? LastMessageAt { get; set; }
        public bool? AllowsContact { get; set; }
        public string AttributesJson { get; set; } = "{}";


        public List<ConversationMessageEntity> Messages { get; set; } = [];
    }
}



