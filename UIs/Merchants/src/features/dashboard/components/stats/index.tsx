import { Badge } from '@/shared/components/ui/badge';
import {
  Card,
  CardAction,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle
} from '@/shared/components/ui/card';
import { TrendingUp } from 'lucide-react';

export function StatsCards() {
  return (
    <section className="*:data-[slot=card]:from-primary/5 *:data-[slot=card]:to-card dark:*:data-[slot=card]:bg-card grid grid-cols-1 gap-4 px-4 *:data-[slot=card]:bg-gradient-to-t *:data-[slot=card]:shadow-xs lg:px-6 @xl/main:grid-cols-2 @5xl/main:grid-cols-4">
      <Card className="@container/card">
        <CardHeader>
          <CardDescription>Receita Total</CardDescription>
          <CardTitle className="text-2xl font-semibold tabular-nums @[250px]/card:text-3xl">
            $1,250.00
          </CardTitle>
          <CardAction>
            <Badge variant="outline">
              <TrendingUp />
              +12.5%
            </Badge>
          </CardAction>
        </CardHeader>
        <CardFooter className="flex-col items-start gap-1.5 text-sm">
          <div className="line-clamp-1 flex gap-2 font-medium">
            Crescimento este mês <TrendingUp className="size-4" />
          </div>
          <div className="text-muted-foreground">Faturamento do último mês</div>
        </CardFooter>
      </Card>
      <Card className="@container/card">
        <CardHeader>
          <CardDescription>Pedidos Realizados</CardDescription>
          <CardTitle className="text-2xl font-semibold tabular-nums @[250px]/card:text-3xl">
            1,234
          </CardTitle>
          <CardAction>
            <Badge variant="outline">
              <TrendingUp />
              +18%
            </Badge>
          </CardAction>
        </CardHeader>
        <CardFooter className="flex-col items-start gap-1.5 text-sm">
          <div className="line-clamp-1 flex gap-2 font-medium">
            Crescimento de pedidos <TrendingUp className="size-4" />
          </div>
          <div className="text-muted-foreground">
            Total de pedidos este mês
          </div>
        </CardFooter>
      </Card>
      <Card className="@container/card">
        <CardHeader>
          <CardDescription>Ticket Médio</CardDescription>
          <CardTitle className="text-2xl font-semibold tabular-nums @[250px]/card:text-3xl">
            $45.67
          </CardTitle>
          <CardAction>
            <Badge variant="outline">
              <TrendingUp />
              +5.2%
            </Badge>
          </CardAction>
        </CardHeader>
        <CardFooter className="flex-col items-start gap-1.5 text-sm">
          <div className="line-clamp-1 flex gap-2 font-medium">
            Aumentando valor por pedido <TrendingUp className="size-4" />
          </div>
          <div className="text-muted-foreground">
            Valor médio dos pedidos
          </div>
        </CardFooter>
      </Card>
      <Card className="@container/card">
        <CardHeader>
          <CardDescription>Taxa de Crescimento</CardDescription>
          <CardTitle className="text-2xl font-semibold tabular-nums @[250px]/card:text-3xl">
            8.3%
          </CardTitle>
          <CardAction>
            <Badge variant="outline">
              <TrendingUp />
              +8.3%
            </Badge>
          </CardAction>
        </CardHeader>
        <CardFooter className="flex-col items-start gap-1.5 text-sm">
          <div className="line-clamp-1 flex gap-2 font-medium">
            Performance em crescimento <TrendingUp className="size-4" />
          </div>
          <div className="text-muted-foreground">
            Crescimento mês a mês
          </div>
        </CardFooter>
      </Card>
    </section>
  );
}
