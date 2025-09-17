# WhatsApp Messaging Solution

Solução modular para processamento, envio, recebimento e armazenamento de mensagens WhatsApp, desenvolvida em .NET 9.  
Projetada para ser extensível, desacoplada e fácil de integrar com diferentes provedores e tecnologias de persistência.

## Visão Geral da Arquitetura

A solution é composta por múltiplos projetos, cada um com responsabilidade clara e integração via abstrações do núcleo:

- **WhatsApp.Messaging.Core**  
  Núcleo da solução. Define contratos, interfaces, DTOs, pipelines e utilitários para o processamento e manipulação de mensagens.  
  Não possui dependências concretas, permitindo que outros projetos implementem suas funcionalidades conforme as abstrações definidas aqui.

- **WhatsApp.Messaging.Storage.EFCore**  
  Implementa persistência de dados usando Entity Framework Core.  
  Permite armazenar conversas, mensagens e metadados em bancos relacionais, aproveitando recursos de migrations e consultas LINQ.

- **WhatsApp.Messaging.Storage.Dapper**  
  Implementa persistência de dados usando Dapper, focando em performance e simplicidade.  
  Ideal para cenários onde o acesso direto via SQL é desejado.

- **WhatsApp.Messaging.Twilio**  
  Provider de integração com a API Twilio para envio e recebimento de mensagens WhatsApp.  
  Implementa as interfaces do Core, permitindo que a solução se comunique com o serviço Twilio de forma transparente e segura.

## Estrutura dos Projetos

```text
src/
├── Core/
│   └── WhatsApp.Messaging.Core/
├── Storage/
│   ├── EFCore/
│   │   └── WhatsApp.Messaging.Storage.EFCore/
│   └── Dapper/
│       └── WhatsApp.Messaging.Storage.Dapper/
└── Providers/
    └── Twilio/
        └── WhatsApp.Messaging.Twilio/
```

## Principais Funcionalidades

- **Processamento de Mensagens:**  
  Pipelines e handlers para tratar mensagens recebidas e enviadas.
- **Formatação de Dados:**  
  Normalização e validação de números de telefone.
- **Persistência Flexível:**  
  Suporte a múltiplas tecnologias de armazenamento (EF Core, Dapper).
- **Integração com Twilio:**  
  Envio e recebimento de mensagens via API Twilio.
- **Extensibilidade:**  
  Fácil adição de novos providers ou storages implementando as interfaces do Core.
- **Configuração via Dependency Injection:**  
  Todos os serviços podem ser registrados e configurados via DI.

## Como Usar

1. **Clone o repositório:**
git clone https://github.com/seu-usuario/whats-app-messaging.git

2. **Restaure os pacotes NuGet:**
dotnet restore

3. **Compile a solução:**
dotnet build

4. **Configure e execute o projeto desejado:**
   - Adicione as configurações necessárias (Twilio, banco de dados, etc).
   - Registre os serviços via DI conforme a documentação de cada projeto.

## Guia de Integração — Twilio + Storage (EF Core / Dapper)

### 1) Mínimo necessário (`Program.cs`)
```csharp
using WhatsApp.Messaging.Twilio.DependencyInjection; // AddWhatsAppMessagingTwilio

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWhatsAppMessagingTwilio(builder.Configuration);

var app = builder.Build();
app.Run();
```

---

### 2) Com **EF Core** como storage (`Program.cs`)
```csharp
using Microsoft.EntityFrameworkCore;
using WhatsApp.Messaging.Twilio.DependencyInjection;
using WhatsApp.Messaging.Storage.EFCore.DependencyInjection; // AddWhatsAppEfStorage

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWhatsAppMessagingTwilio(builder.Configuration)
    .AddWhatsAppEfStorage(db =>
        db.UseSqlServer(builder.Configuration.GetConnectionString("WhatsApp")));

var app = builder.Build();
app.Run();
```

---

### 3) Com **Dapper** como storage (`Program.cs`)
```csharp
using WhatsApp.Messaging.Twilio.DependencyInjection;
using WhatsApp.Messaging.Storage.Dapper.DependencyInjection; // AddWhatsAppDapperStorage

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWhatsAppMessagingTwilio(builder.Configuration)
    .AddWhatsAppDapperStorage(builder.Configuration.GetConnectionString("WhatsApp"));

var app = builder.Build();
app.Run();
```

---

### 4) `appsettings.json` (exemplo)
```json
{
  "Twilio": {
    "AccountSid": "ACxxxxxxxx...",
    "AuthToken": "your_auth_token",
    "FromNumber": "whatsapp:+14155238886",
    "ContentTemplates": {
      "confirmacao_atendimento": {
        "sid": "HX1111111111111111111111111111111",
        "locale": "pt_BR",
        "defaults": { "empresa": "Sensia" }
      },
      "permissao_contato": { "sid": "HX2222222222222222222222222222222" },
      "recusa_agradecimento": { "sid": "HX3333333333333333333333333333333" }
    }
  },
  "ConnectionStrings": {
    "WhatsApp": "Server=.;Database=WhatsAppDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

## Handlers e webhook

### 1) Implemente o handler (no seu projeto Web/API)
```csharp
using WhatsApp.Messaging.Core;
using WhatsApp.Messaging.Twilio.Sending;

public class MeuHandler : IIncomingMessageHandler
{
    private readonly ITwilioTemplateSender _tpl;
    public MeuHandler(ITwilioTemplateSender tpl) => _tpl = tpl;

    public Task<bool> HandleAsync(IncomingMessage msg, CancellationToken ct) =>
        _tpl.SendTemplateAsync(
            key: "confirmacao_atendimento",
            to: msg.From,
            variables: new Dictionary<string, object> { ["nome"] = "Carla" },
            ct: ct
        ).ContinueWith(_ => true, ct);
}
```

### 2) Registre o handler na DI
```csharp
builder.Services.AddSingleton<IIncomingMessageHandler, MeuHandler>();
```

### 3) Endpoint de webhook que dispara o pipeline
```csharp
using WhatsApp.Messaging.Core;

app.MapPost("/webhooks/whatsapp", async (HttpRequest req, IWebhookParser parser, IIncomingMessagePipeline pipeline) =>
{
    var form    = (await req.ReadFormAsync()).ToDictionary(k => k.Key, v => v.Value.ToString());
    var headers = req.Headers.ToDictionary(h => h.Key, h => h.Value.ToString(), StringComparer.OrdinalIgnoreCase);
    var query   = req.Query.ToDictionary(q => q.Key, q => q.Value.ToString(), StringComparer.OrdinalIgnoreCase);

    var wh = new WebhookRequest { Headers = headers, Query = query, Form = form, Body = req.Body };
    if (!parser.CanParse(wh)) return Results.BadRequest("Invalid webhook provider");

    var msg = await parser.ParseAsync(wh, req.HttpContext.RequestAborted);
    await pipeline.DispatchAsync(msg, req.HttpContext.RequestAborted); // chama seu MeuHandler
    return Results.Ok();
});

app.Run();
```

## Requisitos

- .NET 9 SDK
- (Opcional) Entity Framework Core
- (Opcional) Dapper
- (Opcional) Conta e credenciais Twilio

## Licença

Este projeto está protegido por **Licença Proprietária**. Todos os direitos reservados.

**Copyright (c) 2025 Rodrigo de Freitas Oliveira**

- ✅ Uso interno permitido
- ❌ Distribuição proibida
- ❌ Revenda proibida
- ❌ Cópia proibida

Para mais informações, consulte [LICENSE_PROPRIETARY.md](LICENSE_PROPRIETARY.md).

---

> Para detalhes sobre cada projeto, consulte os READMEs específicos nas respectivas pastas.  
> Contribuições são bem-vindas! Abra uma issue ou envie um pull request.
