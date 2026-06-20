# Estrutura Global do Projeto

## Árvore Completa

```
src/
├── app/
│   ├── layouts/        # Estruturas de página (wrappers de layout)
│   ├── routers/        # Definição de rotas e guards
│   └── theme/          # Configuração de tema (cores, tokens)
│
├── assets/
│   ├── images/
│   ├── icons/
│   └── styles/         # CSS global, variáveis
│
├── features/           # Módulos de domínio (autocontidos)
│   ├── operators/
│   ├── partners/
│   └── <nova-feature>/
│
├── shared/
│   ├── components/
│   │   ├── ui/         # Primitivos visuais (Radix-based)
│   │   └── common/     # Componentes reutilizáveis com lógica de UI
│   ├── hooks/          # Hooks globais reutilizáveis
│   ├── services/       # Serviços compartilhados entre features
│   └── utils/          # Funções utilitárias globais
│
└── infra/
    ├── api/            # Cliente HTTP base (axios + interceptores)
    └── sse/            # Clientes SSE e SignalR
```

## Onde Colocar Cada Coisa

| O que criar                         | Onde colocar                        |
|-------------------------------------|-------------------------------------|
| Componente específico de uma feature| `features/<n>/components/`          |
| Componente usado em várias features | `shared/components/common/`         |
| Componente visual sem lógica        | `shared/components/ui/`             |
| Hook específico de uma feature      | `features/<n>/hooks/`               |
| Hook reutilizável global            | `shared/hooks/`                     |
| Chamada HTTP de uma feature         | `features/<n>/services/index.ts`    |
| Serviço compartilhado               | `shared/services/`                  |
| Cliente HTTP base                   | `infra/api/`                        |
| Cliente SSE/SignalR                 | `infra/sse/`                        |
| Tipos do contrato da API            | `features/<n>/types/*.dto.ts`       |
| Tipos do modelo da UI               | `features/<n>/types/*.model.ts`     |