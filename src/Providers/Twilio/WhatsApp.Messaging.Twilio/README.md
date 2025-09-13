# WhatsApp.Messaging.Twilio

Provider de integração do sistema de mensagens WhatsApp com a API Twilio, desenvolvido em .NET 9.

## Visão Geral

O **WhatsApp.Messaging.Twilio** implementa as interfaces e contratos definidos no projeto central (**WhatsApp.Messaging.Core**), permitindo enviar e receber mensagens WhatsApp via Twilio. Atua como um provider, seguindo o padrão de abstração estabelecido pelo Core, garantindo flexibilidade e desacoplamento.

## Relação com WhatsApp.Messaging.Core

- O projeto **Twilio** referencia o **Core** e implementa suas interfaces (ex.: envio, recebimento, pipelines).
- Utiliza os DTOs e pipelines do Core para garantir compatibilidade.
- Pode ser facilmente substituído ou complementado por outros providers (como Storage.EFCore ou Storage.Dapper).

## Principais Componentes

- **Serviços de Integração:** Implementam as interfaces do Core para enviar e receber mensagens utilizando a API do Twilio.
- **Configuração:** Gerenciamento de credenciais, tokens e parâmetros necessários para autenticação e uso da API.
- **Modelos de Mensagem:** Estruturas para representar mensagens enviadas e recebidas via Twilio, compatíveis com os DTOs do Core.
- **Abstractions:** Interfaces específicas para funcionalidades Twilio (ex.: validação de assinatura, envio de templates).
- **DependencyInjection:** Extensões para registro dos serviços no DI.

## Funcionalidades

- Envio de mensagens WhatsApp via Twilio.
- Recebimento e processamento de mensagens recebidas.
- Tratamento de erros e logs de integração.

## Estrutura de Pastas

- `Services/`
- `Models/`
- `Configuration/`
- `Abstractions/`
- `DependencyInjection/`
- `Exceptions/`
- `Http/`
- `Security/`
- `Sending/`
- `Webhooks/`
- `Options/`

## Requisitos

- .NET 9 SDK
- Conta e credenciais Twilio
- Pacote NuGet Twilio
- Referência ao projeto WhatsApp.Messaging.Core

## Licença

Este projeto está licenciado sob os termos da [Licença MIT](https://opensource.org/licenses/MIT).
