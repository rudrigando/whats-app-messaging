using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Dtos;
using WhatsApp.Messaging.Core.Interfaces;
using WhatsApp.Messaging.Storage.EFCore.Entities;

namespace WhatsApp.Messaging.Storage.EFCore
{
    public sealed class EfConversationStore : IConversationStore
    {
        private readonly WhatsAppDbContext _db;
        public EfConversationStore(WhatsAppDbContext db) => _db = db;

        public async Task<Conversation?> GetByUserAsync(string userId, CancellationToken ct = default)
        {
            var e = await _db.Conversations.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, ct);
            return e is null ? null : MapToDomain(e);
        }

        public async Task UpsertAsync(Conversation c, CancellationToken ct = default)
        {
            var existing = await _db.Conversations.FirstOrDefaultAsync(x => x.UserId == c.UserId, ct);
            if (existing is null)
            {
                var entity = MapToEntity(c);
                _db.Conversations.Add(entity);
            }
            else
            {
                existing.UserName = c.UserName;
                existing.LastMessageAt = c.LastMessageAt;
                existing.AllowsContact = c.AllowsContact;
                existing.AttributesJson = Serialize(c.Attributes);
            }
            await _db.SaveChangesAsync(ct);
        }

        public async Task AppendMessageAsync(string userId, IncomingMessage msg, CancellationToken ct = default)
        {
            var conv = await _db.Conversations.FirstOrDefaultAsync(x => x.UserId == userId, ct);
            if (conv is null)
            {
                conv = new ConversationEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CreatedAt = DateTimeOffset.UtcNow,
                    LastMessageAt = msg.ReceivedAt,
                    AttributesJson = "{}"
                };
                _db.Conversations.Add(conv);
            }
            else
            {
                conv.LastMessageAt = msg.ReceivedAt;
            }


            _db.Messages.Add(new ConversationMessageEntity
            {
                ConversationId = conv.Id,
                At = msg.ReceivedAt,
                From = msg.From,
                Text = msg.Text,
                MetadataJson = Serialize(msg.Metadata?.ToDictionary(kv => kv.Key, kv => kv.Value) ?? new Dictionary<string, string>())
            });


            await _db.SaveChangesAsync(ct);
        }

        private static Conversation MapToDomain(ConversationEntity e) => new()
        {
            Id = e.Id,
            UserId = e.UserId,
            UserName = e.UserName,
            CreatedAt = e.CreatedAt,
            LastMessageAt = e.LastMessageAt,
            AllowsContact = e.AllowsContact,
            Attributes = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(e.AttributesJson) ?? new()
        };


        private static ConversationEntity MapToEntity(Conversation c) => new()
        {
            Id = c.Id == Guid.Empty ? Guid.NewGuid() : c.Id,
            UserId = c.UserId,
            UserName = c.UserName,
            CreatedAt = c.CreatedAt,
            LastMessageAt = c.LastMessageAt,
            AllowsContact = c.AllowsContact,
            AttributesJson = Serialize(c.Attributes)
        };

        private static string Serialize(Dictionary<string, string> d) => System.Text.Json.JsonSerializer.Serialize(d);
    }
}
