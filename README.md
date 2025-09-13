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

## Requisitos

- .NET 9 SDK
- (Opcional) Entity Framework Core
- (Opcional) Dapper
- (Opcional) Conta e credenciais Twilio

## Licença

Este projeto está licenciado sob os termos da [Licença MIT](https://opensource.org/licenses/MIT).

---

> Para detalhes sobre cada projeto, consulte os READMEs específicos nas respectivas pastas.
> Contribuições são bem-vindas! Abra uma issue ou envie um pull request.
