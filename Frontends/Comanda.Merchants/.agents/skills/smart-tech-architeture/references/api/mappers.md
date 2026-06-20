# Mappers — Conversão DTO ⇄ Model

## Por que mappers existem

A UI **nunca** consome DTOs diretamente. O mapper isola a UI de mudanças no contrato da API.

```
API muda um campo? → só o mapper precisa mudar.
UI muda um campo?  → só o mapper precisa mudar.
```

## Estrutura — Classe com métodos estáticos

Mappers são classes com métodos estáticos. Nunca são instanciados.

```ts
// services/mapper.ts

import type { OperatorDTO } from '../types/operator.dto'
import type { Operator } from '../types/operator.model'

export class OperatorMapper {
  // DTO → Model (API para UI)
  public static toModel(dto: OperatorDTO): Operator {
    return {
      id: dto.id,
      name: dto.full_name,               // renomear campos
      isActive: dto.status === 'active', // transformar valores
      createdAt: new Date(dto.created_at), // converter tipos
    }
  }

  // Model → DTO (UI para API)
  public static toDTO(model: Operator): OperatorDTO {
    return {
      id: model.id,
      full_name: model.name,
      status: model.isActive ? 'active' : 'inactive',
      created_at: model.createdAt.toISOString(),
    }
  }
}
```

## Mapeamento de Listas

Métodos de lista ficam na mesma classe:

```ts
export class OperatorMapper {
  public static toModel(dto: OperatorDTO): Operator { ... }

  public static toModelList(dtos: OperatorDTO[]): Operator[] {
    return dtos.map(OperatorMapper.toModel)
  }
}
```

Uso no service:

```ts
export async function getOperators(): Promise<Operator[]> {
  const { data } = await httpClient.get('/operators')
  return OperatorMapper.toModelList(data)
}
```

## Mapeamento com múltiplos DTOs de entrada

Quando o mapeamento exige cruzar dados de mais de uma fonte, o método recebe múltiplos parâmetros:

```ts
export class OperatorMapper {
  public static toGroupList(teams: TeamDTO[], allUsers: UserDTO[]): OperatorGroup[] {
    return teams.map((team) => {
      const members = team.members.map((tm) => {
        const fullUser = allUsers.find((u) => u.id === tm.member.id)
        return {
          id: tm.member.id,
          name: fullUser
            ? `${fullUser.firstName ?? ''} ${fullUser.lastName ?? ''}`.trim()
            : tm.member.name,
          email: fullUser?.email ?? tm.member.username,
          phone: fullUser?.attributes?.phone?.[0],
          isLeader: tm.isTeamLead,
        }
      })

      const leader = members.find((m) => m.isLeader)

      return {
        id: team.groupId,
        name: team.groupName,
        membersCount: members.length,
        leaderName: leader?.name,
        members,
      }
    })
  }
}
```

## Regras

- Mappers são **classes com métodos estáticos** — nunca instanciar com `new`
- Métodos são **puros** — sem side-effects, sem chamadas HTTP
- Não importam de outras camadas (hooks, services, infra)
- Toda feature com API obrigatoriamente tem um `mapper.ts`
- Campos opcionais da API devem ter fallback seguro com `??`:

```ts
public static toModel(dto: OperatorDTO): Operator {
  return {
    id: dto.id,
    description: dto.description ?? '',
    tags: dto.tags ?? [],
  }
}
```