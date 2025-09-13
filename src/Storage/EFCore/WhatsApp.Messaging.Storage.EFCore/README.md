# WhatsApp.Messaging.Storage.EFCore

Persistência de dados para o sistema de mensagens WhatsApp, utilizando Entity Framework Core e .NET 9.

## Visão Geral

O **WhatsApp.Messaging.Storage.EFCore** implementa o armazenamento de mensagens, conversas e outros dados relevantes, aproveitando o poder do Entity Framework Core para mapeamento objeto-relacional e operações de banco de dados.

## Principais Componentes

- **DbContext:** Gerencia o ciclo de vida das entidades e a comunicação com o banco de dados.
- **Entidades:** Modelos que representam mensagens, conversas e outros dados persistidos.
- **Repositórios:** Classes para abstração do acesso aos dados, facilitando consultas e operações CRUD.
- **Migrations:** Scripts para evolução e versionamento do esquema do banco de dados.
- **DependencyInjection:** Extensões para registro dos serviços no DI.

## Funcionalidades

- Persistência de mensagens recebidas e enviadas.
- Armazenamento de conversas e metadados.
- Consultas otimizadas via LINQ.
- Suporte a migrations para atualização do banco.

## Estrutura de Pastas

- `Entities/`
- `DependencyInjection/`
- `Migrations/`
- `DbContext/`

## Requisitos

- .NET 9 SDK
- Entity Framework Core

## Licença

Este projeto está licenciado sob os termos da [Licença MIT](https://opensource.org/licenses/MIT).
