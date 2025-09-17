//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Dtos;
using WhatsApp.Messaging.Core.Interfaces;

namespace WhatsApp.Messaging.Storage.Dapper
{
    public sealed class DapperConversationStore : IConversationStore
    {
        private readonly Infrastructure.IDbConnectionFactory _factory;
        public DapperConversationStore(Infrastructure.IDbConnectionFactory factory) => _factory = factory;

        public async Task<Conversation?> GetByUserAsync(string userId, CancellationToken ct = default)
        {
            using var conn = _factory.Create();
            var e = await conn.QueryFirstOrDefaultAsync<dynamic>(
            "SELECT TOP 1 * FROM Conversations WHERE UserId = @userId",
            new { userId }
            );
            if (e is null) return null;
            return new Conversation
            {
                Id = e.Id,
                UserId = e.UserId,
                UserName = e.UserName,
                CreatedAt = e.CreatedAt,
                LastMessageAt = e.LastMessageAt,
                AllowsContact = e.AllowsContact,
                Attributes = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>((string)e.AttributesJson) ?? new()
            };
        }

        public async Task UpsertAsync(Conversation c, CancellationToken ct = default)
        {
            using var conn = _factory.Create();
            var sql = @"
                MERGE Conversations AS target
                USING (SELECT @UserId AS UserId) AS src
                ON (target.UserId = src.UserId)
                WHEN MATCHED THEN UPDATE SET UserName=@UserName, LastMessageAt=@LastMessageAt, AllowsContact=@AllowsContact, AttributesJson=@AttributesJson
                WHEN NOT MATCHED THEN INSERT (Id, UserId, UserName, CreatedAt, LastMessageAt, AllowsContact, AttributesJson)
                VALUES (@Id, @UserId, @UserName, @CreatedAt, @LastMessageAt, @AllowsContact, @AttributesJson);";

            var p = new
            {
                Id = c.Id == Guid.Empty ? Guid.NewGuid() : c.Id,
                c.UserId,
                c.UserName,
                c.CreatedAt,
                c.LastMessageAt,
                c.AllowsContact,
                AttributesJson = System.Text.Json.JsonSerializer.Serialize(c.Attributes)
            };

            await conn.ExecuteAsync(sql, p);
        }

        public async Task AppendMessageAsync(string userId, IncomingMessage msg, CancellationToken ct = default)
        {
            using var conn = _factory.Create();
            // Garante conversa
            await conn.ExecuteAsync(@"
                IF NOT EXISTS (SELECT 1 FROM Conversations WHERE UserId=@UserId)
                INSERT INTO Conversations (Id, UserId, CreatedAt, LastMessageAt, AllowsContact, AttributesJson)
                VALUES (@Id, @UserId, SYSUTCDATETIME(), @LastMessageAt, NULL, '{}')
                ELSE
                UPDATE Conversations SET LastMessageAt=@LastMessageAt WHERE UserId=@UserId;",
            new { Id = Guid.NewGuid(), UserId = userId, LastMessageAt = msg.ReceivedAt });


            // Insere mensagem
            await conn.ExecuteAsync(@"
                INSERT INTO ConversationMessages (ConversationId, At, [From], [Text], MetadataJson)
                SELECT Id, @At, @From, @Text, @MetadataJson FROM Conversations WHERE UserId=@UserId;",
            new
            {
                UserId = userId,
                At = msg.ReceivedAt,
                From = msg.From,
                Text = msg.Text,
                MetadataJson = System.Text.Json.JsonSerializer.Serialize(msg.Metadata ?? new Dictionary<string, string>())
            });
        }
    }
}



