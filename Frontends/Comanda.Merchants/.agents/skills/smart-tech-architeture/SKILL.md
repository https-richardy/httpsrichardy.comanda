---
name: smart-tech-architecture
description: >
  Guia arquitetural completo para projetos React da Smart Tech Innovate. Use esta skill em QUALQUER
  tarefa de código — criar features, componentes, hooks, services, tipos, rotas, ou qualquer outro
  arquivo. Sempre consulte esta skill antes de gerar, editar ou revisar código para garantir
  conformidade com Feature-Sliced Design, Clean Architecture e separação de camadas do projeto.
compatibility: Designed for Claude Code or similar agentic coding tools with filesystem access.
metadata:
  author: smart-tech-innovate
  version: "1.0"
---

# Smart Tech Innovate – Índice de Skills

Este arquivo é o ponto de entrada. Consulte os arquivos em `references/` conforme a tarefa.

## Skills Externas do Projeto

Este projeto utiliza a skill oficial do **shadcn/ui** instalada separadamente. Ela fornece ao agente conhecimento profundo dos componentes disponíveis, CLI e padrões de composição:

```bash
pnpm dlx skills add shadcn/ui
```

> Ao trabalhar com componentes de UI primitivos, consulte a skill do shadcn/ui em conjunto com `references/react/components.md`.

## Regra Geral

Antes de gerar qualquer código, consulte ao menos:
1. [`references/anti-patterns/forbidden-patterns.md`](references/anti-patterns/forbidden-patterns.md) — padrões proibidos
2. O arquivo específico da tarefa conforme o mapa abaixo

## Mapa de Navegação

| Situação                                        | Arquivo                                                          |
|-------------------------------------------------|------------------------------------------------------------------|
| Entender como as camadas se comunicam           | [`references/architecture/layers.md`](references/architecture/layers.md) |
| Criar ou organizar uma nova feature             | [`references/architecture/feature-structure.md`](references/architecture/feature-structure.md) |
| Entender o fluxo de dados (query/mutation)      | [`references/architecture/data-flow.md`](references/architecture/data-flow.md) |
| Criar uma feature do zero (passo a passo)       | [`references/workflow/feature-creation.md`](references/workflow/feature-creation.md) |
| Nomear arquivos, funções, tipos                 | [`references/coding/naming.md`](references/coding/naming.md) |
| Organizar pastas dentro de `/src`               | [`references/coding/file-structure.md`](references/coding/file-structure.md) |
| Definir imports e dependências entre módulos    | [`references/coding/imports.md`](references/coding/imports.md) |
| Criar componentes (UI, Shared, Feature)         | [`references/react/components.md`](references/react/components.md) |
| Implementar uma página                          | [`references/react/pages.md`](references/react/pages.md) |
| Criar hooks com TanStack Query                  | [`references/react/hooks.md`](references/react/hooks.md) |
| Decidir entre Zustand, React Query ou useState  | [`references/react/state-management.md`](references/react/state-management.md) |
| Criar services e clientes HTTP                  | [`references/api/services.md`](references/api/services.md) |
| Criar mappers (DTO ⇄ Model)                     | [`references/api/mappers.md`](references/api/mappers.md) |
| Tratar erros e exibir feedback                  | [`references/api/error-handling.md`](references/api/error-handling.md) |
| Escrever testes                                 | [`references/quality/testing.md`](references/quality/testing.md) |
| Configurar lint e formatação                    | [`references/quality/linting.md`](references/quality/linting.md) |
| Verificar o que NÃO fazer                       | [`references/anti-patterns/forbidden-patterns.md`](references/anti-patterns/forbidden-patterns.md) |