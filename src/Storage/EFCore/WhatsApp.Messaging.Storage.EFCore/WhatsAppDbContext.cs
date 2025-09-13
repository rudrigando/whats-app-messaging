using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Storage.EFCore.Entities;

namespace WhatsApp.Messaging.Storage.EFCore
{
    public class WhatsAppDbContext : DbContext
    {
        public WhatsAppDbContext(DbContextOptions<WhatsAppDbContext> options) : base(options) { }


        public DbSet<ConversationEntity> Conversations => Set<ConversationEntity>();
        public DbSet<ConversationMessageEntity> Messages => Set<ConversationMessageEntity>();


        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<ConversationEntity>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.UserId).IsRequired().HasMaxLength(40);
                e.HasIndex(x => x.UserId).IsUnique();
                e.Property(x => x.UserName).HasMaxLength(200);
                e.Property(x => x.AttributesJson).HasMaxLength(8000);
                e.HasMany(x => x.Messages).WithOne(m => m.Conversation).HasForeignKey(m => m.ConversationId).OnDelete(DeleteBehavior.Cascade);
            });


            b.Entity<ConversationMessageEntity>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Text).HasMaxLength(4000);
                e.Property(x => x.From).IsRequired().HasMaxLength(40);
                e.HasIndex(x => new { x.ConversationId, x.At });
            });
        }
    }
}
