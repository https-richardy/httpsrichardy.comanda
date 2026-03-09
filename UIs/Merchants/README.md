# Nome da aplicação UI

**Propósito da aplicação**

O projeto segue princípios de **Feature-Sliced Design** e **Clean Architecture**, com forte tipagem e isolamento entre UI, domínio e infraestrutura.

---

# 📌 Visão Geral da Arquitetura

A aplicação separa responsabilidades em camadas bem definidas.

```
React Components
      ↓
Feature Hooks
      ↓
Feature Services
      ↓
Infra Layer (API / SSE / WebSocket)
      ↓
Backend
```

### Responsabilidades

| Camada     | Responsabilidade                    |
| ---------- | ----------------------------------- |
| Components | UI e interação com o utilizador     |
| Hooks      | Orquestração de dados e lógica      |
| Services   | Comunicação com backend             |
| Mappers    | Conversão DTO ⇄ Model               |
| Infra      | Clientes HTTP e comunicação externa |

Essa separação garante:

* isolamento de regras de negócio
* proteção da UI contra mudanças da API
* código previsível e escalável

---

# 🚀 Stack Tecnológica

### Core

* React 19
* React Router 7
* Vite (SWC)

### Estado

| Tipo de estado  | Tecnologia     |
| --------------- | -------------- |
| Server State    | TanStack Query |
| Global UI State | Zustand        |
| Local State     | React Hooks    |

### UI

* Tailwind CSS
* Radix UI
* Lucide Icons

### Formulários

* React Hook Form
* Zod

### Tempo real

* SignalR
* Server Sent Events (SSE)

### Testes

* Vitest
* React Testing Library

---

# 📂 Estrutura do Projeto

```
src
├── app
│   ├── layouts
│   ├── routers
│   └── theme
│
├── assets
│   ├── images
│   ├── icons
│   └── styles
│
├── features
│   ├── operators
│   ├── partners
│   └── ...
│
├── shared
│   ├── components
│   │   ├── ui
│   │   └── common
│   ├── hooks
│   ├── services
│   └── utils
│
├── infra
│   ├── api
│   └── sse
```

### Descrição

| Diretório                | Função                                         |
| ------------------------ | ---------------------------------------------- |
| app/layouts              | estruturas de página                           |
| app/routers              | rotas e guards                                 |
| app/theme                | configuração de tema                           |
| assets                   | recursos estáticos (imagens, ícones e estilos) |
| features                 | módulos de domínio da aplicação                |
| shared/components/ui     | componentes visuais primitivos                 |
| shared/components/common | componentes reutilizáveis com lógica de UI     |
| shared/hooks             | hooks reutilizáveis                            |
| shared/services          | serviços compartilhados                        |
| shared/utils             | utilitários globais                            |
| infra/api                | clientes HTTP                                  |
| infra/sse                | comunicação em tempo real                      |

---

# 🧩 Estrutura de uma Feature

Cada feature encapsula completamente o seu domínio.

Exemplo:

```
features/operators
│
├── components
│
├── hooks
│   ├── use-operators.ts
│   └── use-create-operator.ts
│
├── services
│   ├── index.ts
│   ├── mapper.ts
│   └── error-handler.ts
│
├── types
│   ├── operator.dto.ts
│   └── operator.model.ts
```

### Responsabilidades

| Arquivo                | Função                     |
| ---------------------- | -------------------------- |
| services/index         | chamadas HTTP              |
| services/mapper        | conversão DTO ⇄ Model      |
| services/error-handler | tratamento de erros        |
| hooks                  | integração com React Query |
| components             | UI da feature              |

---

# 🔄 DTO vs Model

A UI **nunca consome diretamente os DTOs da API**.

### DTO

Representa o formato da API.

```
OperatorDTO
```

### Model

Representa o modelo usado pela UI.

```
Operator
```

### Conversão

```
DTO → mapper → Model
Model → mapper → DTO
```

Isso protege a aplicação contra mudanças no backend.

---

# 🔁 Fluxo de Dados

## Leitura (Queries)

```
API
 ↓
Service
 ↓
Mapper
 ↓
React Query
 ↓
Hook
 ↓
Component
```

## Escrita (Mutations)

```
Component
 ↓
Hook
 ↓
Service
 ↓
Mapper
 ↓
API
```

---

# 🧠 Gestão de Estado

A aplicação segue regras claras:

### 1️⃣ Server State

TanStack Query

Usado para:

* listas
* detalhes
* dados vindos da API

### 2️⃣ Estado Global

Zustand

Usado para:

* token de autenticação
* tema
* estado do menu

### 3️⃣ Estado Local

React Hooks

Usado para:

* UI
* dropdowns
* inputs
* interações locais

---

# 📡 Comunicação em Tempo Real

Algumas áreas da aplicação utilizam atualizações passivas.

Tecnologias usadas:

* SignalR
* Server Sent Events (SSE)

Esses clientes atualizam diretamente o **cache do React Query**, evitando polling.

---

# 🧩 Componentes

Os componentes são divididos em três níveis.

## UI Components

```
shared/components/ui
```

Componentes visuais baseados em Radix.

Regras:

* sem lógica de negócio
* apenas props e eventos

---

## Shared Components

```
shared/components/common
```

Componentes reutilizáveis com lógica interna de UI.

Exemplos:

* Datatable
* File Picker
* Wizard

---

## Feature Components

```
features/.../components
```

Componentes específicos do domínio.

Regras:

* não acessam API diretamente
* recebem dados via hooks

---

# 🔐 Roteamento e Guards

O projeto possui dois tipos de proteção.

### Auth Guard

Protege rotas privadas.

Se o utilizador não estiver autenticado:

```
redirect → /login
```

---

### Guest Guard

Impede utilizadores autenticados de acessar páginas públicas.

```
/login → redirect → /dashboard
```

---

# 🚨 Tratamento de Erros

Erros são tratados em três níveis.

### Validação

Zod

### Domínio

`error-handler.ts` da feature.

### Infraestrutura

Interceptores HTTP.

O feedback ao utilizador é exibido via **toasts**.

---

# 🧪 Testes

Ferramentas usadas:

* Vitest
* React Testing Library

Testamos principalmente:

* utilitários
* hooks
* interações de UI

---

# 🚀 Começando

### Pré-requisitos

* Node 20+
* pnpm

### Instalação

```
pnpm install
```

### Configuração

```
cp .env .env.local
```

Preencher:

```
VITE_API_GATEWAY_URL
VITE_AUTH_API_URL
```

---

# ▶ Executar o projeto

```
pnpm dev
```

Servidor de desenvolvimento:

```
http://localhost:5173
```

---

# 🛠 Scripts

| Script             | Função                      |
| ------------------ | --------------------------- |
| pnpm dev           | ambiente de desenvolvimento |
| pnpm build         | build de produção           |
| pnpm preview       | preview da build            |
| pnpm lint          | lint do projeto             |
| pnpm test          | executar testes             |
| pnpm test:coverage | relatório de coverage       |

---

# 📏 Convenções do Projeto

### Arquivos

```
kebab-case
```

### Componentes

```
PascalCase
```

### Sufixos obrigatórios

```
*.dto.ts
*.model.ts
*.test.ts
*.mapper.ts
```

---

# 📚 Regras Arquiteturais

1️⃣ UI nunca consome DTO diretamente
2️⃣ chamadas HTTP apenas em services
3️⃣ hooks são responsáveis por React Query
4️⃣ componentes não fazem side-effects
5️⃣ lógica de domínio nunca fica em componentes

---

# 🤝 Contribuindo

Ao criar uma nova feature:

1. criar pasta em `features/`
2. definir DTOs e Models
3. criar `services`
4. criar `hooks`
5. criar `components`
6. registrar rotas
