# WhatsApp.Messaging.Core

Projeto central para abstrações e funcionalidades essenciais do sistema de mensagens WhatsApp, desenvolvido em .NET 9.

## Visão Geral

O **WhatsApp.Messaging.Core** é o núcleo da solução, responsável por definir contratos, interfaces, DTOs, pipelines e utilitários para o processamento, formatação e manipulação de mensagens. Ele não depende de implementações concretas, permitindo que outros projetos (providers e storages) implementem suas funcionalidades conforme as abstrações definidas aqui.

## Principais Componentes

- **Interfaces:** Contratos para formatação de números, pipelines de mensagens, handlers, envio de mensagens, armazenamento de conversas, etc.
- **Formatters:** Implementações para normalização e validação de números de telefone (ex.: E.164, Brasil).
- **Pipelines:** Fluxos de processamento para mensagens recebidas, permitindo o tratamento por múltiplos handlers.
- **DTOs:** Estruturas de dados para transporte de informações entre camadas e serviços.
- **Options:** Classes de configuração para parametrização do comportamento do sistema.
- **Utils:** Extensões e utilitários para manipulação de dados.

## Exemplos de Funcionalidades

- Normalização de números de telefone para o padrão internacional.
- Pipeline para processamento assíncrono de mensagens recebidas.
- Registro e configuração de serviços via Dependency Injection.

## Estrutura de Pastas

- `Interfaces/`
- `Formatters/`
- `Pipelines/`
- `Dtos/`
- `Options/`
- `Utils/`
- `DependencyInjection/`
- `Logging/`

## Requisitos

- .NET 9 SDK

## Licença

Este projeto está licenciado sob os termos da [Licença MIT](https://opensource.org/licenses/MIT).
