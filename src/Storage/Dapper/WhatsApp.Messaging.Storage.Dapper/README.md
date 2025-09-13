# WhatsApp.Messaging.Storage.Dapper

Persistência de dados para o sistema de mensagens WhatsApp, utilizando Dapper e .NET 9.

## Visão Geral

O **WhatsApp.Messaging.Storage.Dapper** implementa o armazenamento e consulta de dados do WhatsApp de forma leve e eficiente, utilizando o micro-ORM Dapper para acesso direto ao banco de dados via SQL.

## Principais Componentes

- **Repositórios:** Executam comandos SQL para operações CRUD de mensagens, conversas e outros dados.
- **Modelos/Entidades:** Estruturas que representam os dados persistidos e recuperados do banco.
- **Configuração de Conexão:** Gerenciamento da string de conexão e integração com o sistema de Dependency Injection.
- **Infrastructure:** Fábricas de conexão para abstrair o acesso ao banco.
- **DependencyInjection:** Extensões para registro dos serviços no DI.

## Funcionalidades

- Persistência e consulta de mensagens e conversas via SQL.
- Operações rápidas e eficientes, aproveitando o desempenho do Dapper.
- Facilidade de integração com outros componentes da solução.

## Estrutura de Pastas

- `Repositories/`
- `Models/`
- `Infrastructure/`
- `DependencyInjection/`

## Requisitos

- .NET 9 SDK
- Dapper

## Licença

Este projeto está licenciado sob os termos da [Licença MIT](https://opensource.org/licenses/MIT).
