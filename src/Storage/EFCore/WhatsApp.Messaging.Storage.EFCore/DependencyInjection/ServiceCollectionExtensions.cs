using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Interfaces;

namespace WhatsApp.Messaging.Storage.EFCore.DependencyInjection
{
    public static class StorageServiceCollectionExtensions
    {
        public static IServiceCollection AddWhatsAppEfStorage(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> configureDb)
        {
            services.AddDbContext<WhatsAppDbContext>(configureDb);
            services.AddScoped<IConversationStore, EfConversationStore>();
            return services;
        }
    }
}
