//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsApp.Messaging.Core.Interfaces;
using WhatsApp.Messaging.Storage.Dapper.Infrastructure;

namespace WhatsApp.Messaging.Storage.Dapper.DependencyInjection
{
    public static class DapperStorageServiceCollectionExtensions
    {
        // Registro simples com connection string (SQL Server por padrão neste exemplo)
        public static IServiceCollection AddWhatsAppDapperStorage(
            this IServiceCollection services,
            string connectionString,
            Func<IServiceProvider, IDbConnectionFactory>? factory = null)
        {
            if (factory is null)
            {
                services.AddSingleton<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
            }
            else
            {
                services.AddSingleton(factory);
            }


            services.AddScoped<IConversationStore, DapperConversationStore>();
            return services;
        }
    }
}



