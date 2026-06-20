# 🚀 Guia de Onboarding: Smart Tech Innovate Template

Bem-vindo(a) à equipa! 🎉

Este documento foi desenhado para te ajudar a entender como construímos aplicações front-end na Smart Tech Innovate. O nosso objetivo com este template é que não percas tempo a configurar projetos, a debater arquitetura ou a reinventar a roda.

Aqui, o caminho já está pavimentado. Vamos começar?

---

# 🛠️ 1. O Teu Primeiro Dia (Setup)

Antes de escrever qualquer código de negócio, precisamos preparar o template para a **nova aplicação**.

---

## 1️⃣ Clonar o repositório

Clona o template e renomeia a pasta para o nome da nova aplicação.

Exemplo:

```
smart-tech-innovate-rh
```

---

## 2️⃣ Atualizar o `package.json`

Abre o ficheiro:

```
package.json
```

Atualiza o campo **name** para refletir o nome da nova aplicação.

Exemplo:

```json
{
  "name": "smart-tech-innovate-rh"
}
```

Isso garante que:

* pipelines
* builds
* logs
* artefactos

utilizam o nome correto da aplicação.

---

## 3️⃣ Atualizar o README da aplicação

O README deve refletir **o propósito da aplicação**.

Atualiza o título e a descrição inicial.

### Exemplo

#### Antes (template)

```
# Smart Tech Admin Template
```

#### Depois

```
# SmartTechInnovate.Administration.UI
```

Adicionar também uma breve descrição.

Exemplo:

```
Painel administrativo da plataforma SmartTech Innovate.

A aplicação é responsável pela gestão de operadores, parceiros,
integrações e monitorização operacional do sistema.
```

O README deve responder rapidamente:

* **O que é a aplicação**
* **Qual problema resolve**
* **Quem usa**

---

## 4️⃣ Instalar dependências

Usamos **estritamente `pnpm`**.

```bash
pnpm install
```

---

## 5️⃣ Configurar o ambiente

Duplica o ficheiro:

```
.env.example
```

Renomeia para:

```
.env.local
```

Preenche as URLs das APIs do ambiente de desenvolvimento.

Exemplo:

```
VITE_API_GATEWAY_URL=
VITE_AUTH_API_URL=
```

---

## 6️⃣ Arrancar a aplicação

```bash
pnpm dev
```

A aplicação estará disponível em:

```
http://localhost:5173
```

Se tudo correu bem, deverás ver **o ecrã de Login padrão da empresa**.

---

# 🧠 2. O Modelo Mental (Feature-Sliced Design)

Esquece o padrão **MVC**, `containers` ou `views`.

Nós organizamos o código pelo **domínio do negócio**.

O fluxo de dependência é sempre **de cima para baixo**.

---

## `src/app/` — O Esqueleto

Configuração global da aplicação.

Usado para:

* layouts
* rotas
* providers
* tema

Estrutura típica:

```
app
├── layouts
├── routers
└── theme
```

---

## `src/features/` — O Coração

Onde vais passar **90% do teu tempo**.

Cada pasta representa um **módulo de domínio**.

Exemplo:

```
features
├── auth
├── clients
├── invoices
```

⚠️ Regra importante:

**Uma feature nunca importa diretamente outra feature.**

---

## `src/shared/` — A Base

A tua **caixa de Lego**.

Contém componentes e utilitários reutilizáveis.

```
shared
├── components
├── hooks
├── services
└── utils
```

O `shared` é **agnóstico ao domínio da aplicação**.

---

# 🔁 3. Fluxo de Dados da Aplicação

A aplicação segue um fluxo previsível:

```
Component
   ↓
Hook
   ↓
Service
   ↓
Infra
   ↓
API
```

Isso garante:

* separação de responsabilidades
* UI desacoplada da API
* manutenção simples

---

# 🏗️ 4. Como Criar uma Nova Feature (Smart Tech Way)

Imagina que precisas criar o módulo **Clients**.

Nunca comeces pelo ecrã visual.

Segue esta ordem.

---

## Passo 1 — Contratos (`types/`)

Define o que entra e sai da aplicação.

```
types
├── dtos
└── models
```

DTO → formato da API
Model → formato usado pela UI

---

## Passo 2 — Tradução e Serviço (`services/`)

A UI não fala diretamente com a API.

```
services
├── mapper.ts
└── index.ts
```

Mapper converte:

```
DTO → Model
Model → DTO
```

---

## Passo 3 — Estado do Servidor (`hooks/`)

A UI não faz `fetch`.

```
hooks
├── use-queries.ts
└── use-mutations.ts
```

Usamos **TanStack Query**.

---

## Passo 4 — UI (`components/` e `pages/`)

Agora constrói os visuais.

Estrutura típica:

```
features/clients
│
├── components
├── hooks
├── pages
├── services
└── types
```

Passos finais:

1. criar componentes
2. usar hooks
3. criar página
4. registrar rota em `app/routers`

---

# 🧰 5. Os Teus Superpoderes

Antes de criar algo novo, verifica `src/shared`.

---

### Tabelas complexas

```
shared/components/global/datatable
```

Inclui:

* paginação
* filtros
* pesquisa
* ordenação

---

### Upload de ficheiros

```
shared/components/global/file-picker
```

Inclui:

* drag & drop
* preview

---

### Formulários multi-etapas

```
shared/components/global/wizard
```

---

### Máscaras e formatação

```
shared/utils/masks
```

---

# 🚨 6. As 3 Regras de Ouro

O CI/CD pode bloquear commits que quebrem estas regras.

---

### 1️⃣ DTO nunca chega à UI

```
DTO → mapper → Model → Component
```

---

### 2️⃣ Imports proibidos

Dependências permitidas:

```
shared → não importa nada
features → podem importar shared
app → pode importar tudo
```

Nunca faça:

```
feature → feature
shared → feature
```

---

### 3️⃣ TanStack Query gere server state

Nunca uses:

```
useState para dados da API
```

Use sempre **React Query**.

---

# 📦 7. Quando usar `shared`

Antes de mover algo para `shared`, pergunta:

1. é genérico?
2. será reutilizado?
3. não depende de domínio?

Se **sim para tudo**, pode ir para `shared`.

Caso contrário, fica na feature.

---

# ❓ 8. FAQ

### Posso chamar API diretamente no componente?

Não.

Sempre use:

```
Component → Hook → Service
```

---

### Posso importar uma feature dentro de outra?

Não.

Features devem ser isoladas.

Se algo precisa ser reutilizado:

```
mover para shared
```

---

### Posso usar `useState` para guardar dados da API?

Não.

Dados do servidor devem ser geridos por:

```
TanStack Query
```

---

### Onde criar novos componentes?

| Situação                         | Local                     |
| -------------------------------- | ------------------------- |
| componente específico da feature | `features/.../components` |
| componente reutilizável          | `shared/components`       |
| componente visual base           | `shared/components/ui`    |

---

### Posso instalar uma nova biblioteca?

Antes de instalar:

1. verifica `shared`
2. verifica dependências existentes

Evita duplicação de soluções.

---

### Onde ver exemplos reais?

Explora as features que vêm no template:

```
auth
dashboard
notification
```

Elas demonstram como aplicar toda a arquitetura.

---

Bom código! 🚀
