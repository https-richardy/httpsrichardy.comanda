# Níveis de Componentes

## Hierarquia

```
shared/components/ui      → primitivos shadcn/ui (instalados via CLI)
shared/components/common  → reutilizáveis com lógica de UI
features/.../components   → específicos do domínio
```

## 1. UI Components (`shared/components/ui/`)

Componentes primitivos do **shadcn/ui**, instalados via CLI diretamente em `shared/components/ui/`. São baseados em Radix UI e estilizados com Tailwind CSS.

> ⚠️ **Este projeto tem a skill oficial do shadcn/ui instalada.** Consulte-a para saber quais componentes estão disponíveis, como instalá-los e como compô-los corretamente.

### Adicionando um novo componente

Sempre usar o CLI do shadcn para adicionar componentes — nunca criar manualmente em `shared/components/ui/`:

```bash
pnpm dlx shadcn@latest add button
pnpm dlx shadcn@latest add dialog
pnpm dlx shadcn@latest add data-table
```

Os arquivos são instalados automaticamente em `shared/components/ui/`.

### Uso

Importar sempre de `@/shared/components/ui/<componente>`:

```tsx
import { Button } from '@/shared/components/ui/button'
import { Dialog, DialogContent, DialogHeader } from '@/shared/components/ui/dialog'
import { Input } from '@/shared/components/ui/input'
```

**Regras:**
- Sem lógica de negócio
- Apenas props e callbacks
- Não acessam hooks de dados
- **Nunca** criar componentes primitivos manualmente se existir equivalente no shadcn/ui — sempre instalar via CLI primeiro

---

## 2. Shared Components (`shared/components/common/`)

Componentes reutilizáveis que combinam primitivos shadcn/ui com lógica interna de UI.

**Regras:**
- Podem ter estado local (ex: paginação, ordenação)
- Não acessam API diretamente
- Recebem dados via props
- Compõem componentes de `shared/components/ui/`

Exemplos existentes: `Datatable`, `FilePicker`, `Wizard`

```tsx
// Exemplo: Datatable compõe primitivos shadcn
import { Table, TableBody, TableCell, TableHead, TableRow } from '@/shared/components/ui/table'
import { Button } from '@/shared/components/ui/button'

export function Datatable({ data, onRowClick }) {
  // lógica de paginação, ordenação...
  return (
    <Table>
      <TableBody>
        {data.map(row => (
          <TableRow key={row.id} onClick={() => onRowClick(row)}>
            <TableCell>{row.name}</TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  )
}
```

---

## 3. Feature Components (`features/.../components/`)

Específicos do domínio da feature.

**Regras:**
- Consomem hooks da própria feature
- Não fazem chamadas HTTP
- Não importam de outras features
- Compõem primitivos shadcn/ui e shared components

```tsx
// ✅ Correto
import { Button } from '@/shared/components/ui/button'
import { Datatable } from '@/shared/components/common/datatable'
import { useOperatorsQuery } from '../hooks/use-queries'

export function OperatorList() {
  const { data, isLoading } = useOperatorsQuery()
  return <Datatable data={data} isLoading={isLoading} />
}

// ❌ Errado — chamada HTTP direta no componente
export function OperatorList() {
  const [data, setData] = useState([])
  useEffect(() => {
    httpClient.get('/operators').then(res => setData(res.data))
  }, [])
}
```