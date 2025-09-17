# Informações de Copyright - WhatsApp Messaging Solution

## Proprietário
**Rodrigo de Freitas Oliveira**

## Ano de Criação
2025

## Tipo de Licença
**Licença Proprietária** - Todos os direitos reservados

## Arquivos Protegidos
Todos os arquivos de código-fonte (.cs, .csproj, .sln, .json, .md) estão protegidos por copyright.

## Cabeçalho Padrão
```csharp
//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.
```

## Aplicação do Copyright
O cabeçalho de copyright deve ser adicionado em todos os arquivos de código-fonte, incluindo:

- ✅ Interfaces (.cs)
- ✅ Classes (.cs)
- ✅ DTOs (.cs)
- ✅ Handlers (.cs)
- ✅ Endpoints (.cs)
- ✅ Configurações (.cs)
- ✅ Testes (.cs)

## Script de Automação
Use o script `scripts/add-copyright-headers.ps1` para adicionar automaticamente os cabeçalhos em todos os arquivos .cs do projeto.

### Como usar:
```powershell
# Na raiz do projeto
.\scripts\add-copyright-headers.ps1

# Ou especificando um diretório
.\scripts\add-copyright-headers.ps1 -RootPath "src"
```

## Verificação
Para verificar se todos os arquivos possuem copyright:
```powershell
Get-ChildItem -Path "src" -Filter "*.cs" -Recurse | Select-String -Pattern "Copyright \(c\) 2025" | Measure-Object
```

## Atualização Anual
Lembre-se de atualizar o ano do copyright quando necessário (ex: 2025 → 2026).

---

**Última atualização**: Janeiro 2025
