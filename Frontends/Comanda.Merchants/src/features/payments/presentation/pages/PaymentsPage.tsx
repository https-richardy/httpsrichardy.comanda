import { useState, useMemo } from 'react';
import {
  AlertTriangle,
  Banknote,
  CheckCircle2,
  Clock,
  CreditCard,
  RotateCcw,
  ShieldCheck,
  Smartphone,
  TrendingUp,
  Wallet,
} from 'lucide-react';
import {
  Bar,
  BarChart,
  CartesianGrid,
  Cell,
  XAxis,
  YAxis,
} from 'recharts';

import { Badge } from '@/shared/components/ui/badge';
import { Card, CardContent, CardHeader, CardTitle } from '@/shared/components/ui/card';
import { ScrollArea } from '@/shared/components/ui/scroll-area';
import { Separator } from '@/shared/components/ui/separator';
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
} from '@/shared/components/ui/sheet';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/shared/components/ui/table';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/shared/components/ui/tabs';
import {
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
  type ChartConfig,
} from '@/shared/components/ui/chart';
import { cn } from '@/shared/utils';

import { MOCK_PAYMENTS } from '../../data/mock';
import type { Payment, PaymentMethod, PaymentStatus } from '../../domain/entities';

const STATUS_CONFIG: Record<
  PaymentStatus,
  { label: string; className: string; icon: React.ReactNode }
> = {
  Paid: {
    label: 'Pago',
    className: 'bg-emerald-100 text-emerald-700 border border-emerald-200 dark:bg-emerald-950/30 dark:text-emerald-400',
    icon: <CheckCircle2 className="size-3" />,
  },
  Pending: {
    label: 'Aguardando',
    className: 'bg-amber-100 text-amber-700 border border-amber-200 dark:bg-amber-950/30 dark:text-amber-400',
    icon: <Clock className="size-3" />,
  },
  Failed: {
    label: 'Falhou',
    className: 'bg-red-100 text-red-700 border border-red-200 dark:bg-red-950/30 dark:text-red-400',
    icon: <AlertTriangle className="size-3" />,
  },
  Refunded: {
    label: 'Reembolsado',
    className: 'bg-orange-100 text-orange-700 border border-orange-200 dark:bg-orange-950/30 dark:text-orange-400',
    icon: <RotateCcw className="size-3" />,
  },
};

const METHOD_CONFIG: Record<
  PaymentMethod,
  { label: string; icon: React.ReactNode; color: string }
> = {
  Pix: { label: 'Pix', icon: <Smartphone className="size-3.5" />, color: '#00B4D8' },
  Card: { label: 'Cartão', icon: <CreditCard className="size-3.5" />, color: '#7C3AED' },
  Cash: { label: 'Dinheiro', icon: <Banknote className="size-3.5" />, color: '#059669' },
  Unspecified: { label: '-', icon: <Wallet className="size-3.5" />, color: '#6B7280' },
};

const CHART_CONFIG = {
  total: { label: 'Total (R$)', color: 'hsl(var(--primary))' },
} satisfies ChartConfig;

type StatusFilter = 'all' | PaymentStatus;

const TAB_OPTIONS: { value: StatusFilter; label: string }[] = [
  { value: 'all', label: 'Todos' },
  { value: 'Pending', label: 'Aguardando' },
  { value: 'Paid', label: 'Pagos' },
  { value: 'Failed', label: 'Falhos' },
  { value: 'Refunded', label: 'Reembolsados' },
];

function formatCurrency(value: number) {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(value);
}

function getInitials(name: string) {
  return name
    .split(' ')
    .slice(0, 2)
    .map((n) => n[0])
    .join('')
    .toUpperCase();
}

function MetricCard({
  icon,
  label,
  value,
  sub,
  iconClass,
}: {
  icon: React.ReactNode;
  label: string;
  value: string;
  sub?: string;
  iconClass?: string;
}) {
  return (
    <Card className="shadow-sm">
      <CardContent className="p-4 flex items-center gap-3">
        <div className={cn('rounded-lg p-2.5 bg-muted/60 shrink-0', iconClass)}>
          {icon}
        </div>
        <div className="min-w-0">
          <p className="text-xs text-muted-foreground font-medium uppercase tracking-wide truncate">
            {label}
          </p>
          <p className="text-xl font-bold leading-tight mt-0.5 truncate">{value}</p>
          {sub && <p className="text-xs text-muted-foreground mt-0.5">{sub}</p>}
        </div>
      </CardContent>
    </Card>
  );
}

function PayerAvatar({ name }: { name: string }) {
  return (
    <div className="size-8 rounded-full bg-primary/10 text-primary flex items-center justify-center text-xs font-bold shrink-0 select-none">
      {getInitials(name)}
    </div>
  );
}

function PaymentStatusBadge({ status }: { status: PaymentStatus }) {
  const config = STATUS_CONFIG[status];
  return (
    <span
      className={cn(
        'inline-flex items-center gap-1 rounded-full px-2 py-0.5 text-[11px] font-medium whitespace-nowrap',
        config.className,
      )}
    >
      {config.icon}
      {config.label}
    </span>
  );
}

function PaymentMethodBadge({ method }: { method: PaymentMethod }) {
  const config = METHOD_CONFIG[method];
  return (
    <span className="inline-flex items-center gap-1.5 text-xs text-muted-foreground font-medium">
      {config.icon}
      {config.label}
    </span>
  );
}

function PaymentDetailSheet({
  payment,
  onClose,
}: {
  payment: Payment | null;
  onClose: () => void;
}) {
  if (!payment) return null;

  const status = STATUS_CONFIG[payment.status];
  const method = METHOD_CONFIG[payment.method];

  return (
    <Sheet open={!!payment} onOpenChange={onClose}>
      <SheetContent className="w-full sm:max-w-[400px] p-0 flex flex-col">
        <SheetHeader className="p-5 pb-4 border-b shrink-0">
          <div className="flex items-center gap-2.5 flex-wrap">
            <SheetTitle className="font-mono text-lg">{payment.reference}</SheetTitle>
            <PaymentStatusBadge status={payment.status} />
          </div>
          <SheetDescription className="flex items-center gap-1.5">
            {method.icon}
            {method.label}
          </SheetDescription>
        </SheetHeader>

        <ScrollArea className="flex-1">
          <div className="p-5 space-y-5">
            <section className="rounded-xl border bg-muted/20 p-4 space-y-3">
              <div className="flex items-center gap-3">
                <PayerAvatar name={payment.payer.name} />
                <div>
                  <p className="text-sm font-semibold">{payment.payer.name}</p>
                  <p className="text-xs text-muted-foreground">Pagador</p>
                </div>
              </div>
            </section>

            <Separator />

            <section className="space-y-3">
              <h3 className="text-xs font-semibold text-muted-foreground uppercase tracking-wider">
                Detalhes da transação
              </h3>

              <div className="space-y-2.5">
                <div className="flex justify-between text-sm">
                  <span className="text-muted-foreground">Referência</span>
                  <span className="font-mono font-semibold">{payment.reference}</span>
                </div>
                <div className="flex justify-between text-sm">
                  <span className="text-muted-foreground">Método</span>
                  <PaymentMethodBadge method={payment.method} />
                </div>
                <div className="flex justify-between text-sm">
                  <span className="text-muted-foreground">Status</span>
                  <PaymentStatusBadge status={payment.status} />
                </div>
              </div>
            </section>

            <Separator />

            <section className="rounded-xl border bg-card p-4 flex justify-between items-center">
              <span className="text-sm font-semibold">Valor total</span>
              <span className="text-2xl font-bold">{formatCurrency(payment.amount)}</span>
            </section>
          </div>
        </ScrollArea>
      </SheetContent>
    </Sheet>
  );
}

export function PaymentsPage() {
  const [selected, setSelected] = useState<Payment | null>(null);
  const [activeTab, setActiveTab] = useState<StatusFilter>('all');

  const paid = useMemo(
    () => MOCK_PAYMENTS.filter((p) => p.status === 'Paid'),
    [],
  );
  const pending = useMemo(
    () => MOCK_PAYMENTS.filter((p) => p.status === 'Pending'),
    [],
  );
  const failed = useMemo(
    () => MOCK_PAYMENTS.filter((p) => p.status === 'Failed'),
    [],
  );
  const refunded = useMemo(
    () => MOCK_PAYMENTS.filter((p) => p.status === 'Refunded'),
    [],
  );

  const totalPaid = paid.reduce((s, p) => s + p.amount, 0);
  const totalPending = pending.reduce((s, p) => s + p.amount, 0);
  const totalLoss = [...failed, ...refunded].reduce((s, p) => s + p.amount, 0);
  const successRate = Math.round((paid.length / MOCK_PAYMENTS.length) * 100);

  const methodChartData = useMemo(() => {
    const groups: Record<string, number> = {};
    for (const p of MOCK_PAYMENTS) {
      if (p.status !== 'Paid') continue;
      const key = p.method;
      groups[key] = (groups[key] ?? 0) + p.amount;
    }
    return (['Pix', 'Card', 'Cash'] as PaymentMethod[]).map((m) => ({
      method: METHOD_CONFIG[m].label,
      total: parseFloat((groups[m] ?? 0).toFixed(2)),
      fill: METHOD_CONFIG[m].color,
    }));
  }, []);

  const filtered = useMemo(
    () =>
      activeTab === 'all'
        ? MOCK_PAYMENTS
        : MOCK_PAYMENTS.filter((p) => p.status === activeTab),
    [activeTab],
  );

  return (
    <>
      <div className="flex flex-col gap-5 p-4 lg:p-6">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">Pagamentos</h1>
          <p className="text-sm text-muted-foreground mt-0.5">
            Acompanhe as transações e o desempenho financeiro
          </p>
        </div>

        {/* Métricas */}
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-3">
          <MetricCard
            icon={<CheckCircle2 className="size-4 text-emerald-600" />}
            label="Total recebido"
            value={formatCurrency(totalPaid)}
            sub={`${paid.length} transações`}
            iconClass="text-emerald-600"
          />
          <MetricCard
            icon={<Clock className="size-4 text-amber-600" />}
            label="Aguardando"
            value={formatCurrency(totalPending)}
            sub={`${pending.length} pendentes`}
            iconClass="text-amber-600"
          />
          <MetricCard
            icon={<AlertTriangle className="size-4 text-red-500" />}
            label="Perdas"
            value={formatCurrency(totalLoss)}
            sub={`${failed.length} falhas · ${refunded.length} reembolsos`}
            iconClass="text-red-500"
          />
          <MetricCard
            icon={<ShieldCheck className="size-4 text-blue-600" />}
            label="Taxa de sucesso"
            value={`${successRate}%`}
            sub={`${paid.length} de ${MOCK_PAYMENTS.length} transações`}
            iconClass="text-blue-600"
          />
        </div>

        {/* Gráfico */}
        <Card className="shadow-sm">
          <CardHeader className="pb-2">
            <div className="flex items-center gap-2">
              <TrendingUp className="size-4 text-muted-foreground" />
              <CardTitle className="text-base font-semibold">Receita por método de pagamento</CardTitle>
            </div>
            <p className="text-xs text-muted-foreground">Apenas transações pagas</p>
          </CardHeader>
          <CardContent>
            <ChartContainer config={CHART_CONFIG} className="h-52 w-full">
              <BarChart
                data={methodChartData}
                margin={{ top: 8, right: 8, left: 8, bottom: 4 }}
                barSize={52}
              >
                <CartesianGrid vertical={false} strokeDasharray="3 3" stroke="hsl(var(--border))" />
                <XAxis
                  dataKey="method"
                  axisLine={false}
                  tickLine={false}
                  tick={{ fontSize: 12, fill: 'hsl(var(--muted-foreground))' }}
                />
                <YAxis
                  axisLine={false}
                  tickLine={false}
                  tick={{ fontSize: 11, fill: 'hsl(var(--muted-foreground))' }}
                  tickFormatter={(v) => `R$ ${v}`}
                  width={72}
                />
                <ChartTooltip
                  cursor={{ fill: 'hsl(var(--muted))', radius: 6 }}
                  content={
                    <ChartTooltipContent
                      formatter={(value) => [formatCurrency(value as number), 'Receita']}
                    />
                  }
                />
                <Bar dataKey="total" radius={[6, 6, 0, 0]}>
                  {methodChartData.map((entry, index) => (
                    <Cell key={index} fill={entry.fill} />
                  ))}
                </Bar>
              </BarChart>
            </ChartContainer>

            {/* Legenda manual */}
            <div className="flex justify-center gap-6 mt-3 pt-3 border-t">
              {(['Pix', 'Card', 'Cash'] as PaymentMethod[]).map((m) => (
                <div key={m} className="flex items-center gap-1.5">
                  <span
                    className="size-2.5 rounded-full shrink-0"
                    style={{ backgroundColor: METHOD_CONFIG[m].color }}
                  />
                  <span className="text-xs text-muted-foreground">{METHOD_CONFIG[m].label}</span>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Tabela com filtros */}
        <div>
          <Tabs value={activeTab} onValueChange={(v) => setActiveTab(v as StatusFilter)}>
            <TabsList className="w-fit">
              {TAB_OPTIONS.map((tab) => {
                const count =
                  tab.value === 'all'
                    ? MOCK_PAYMENTS.length
                    : MOCK_PAYMENTS.filter((p) => p.status === tab.value).length;
                return (
                  <TabsTrigger key={tab.value} value={tab.value} className="gap-1.5">
                    {tab.label}
                    {count > 0 && (
                      <Badge
                        variant={tab.value === 'all' ? 'secondary' : 'outline'}
                        className="h-4 px-1.5 text-[10px] font-bold rounded-full"
                      >
                        {count}
                      </Badge>
                    )}
                  </TabsTrigger>
                );
              })}
            </TabsList>

            {TAB_OPTIONS.map((tab) => (
              <TabsContent key={tab.value} value={tab.value} className="mt-4">
                <Card className="shadow-sm overflow-hidden">
                  <Table>
                    <TableHeader>
                      <TableRow className="bg-muted/30 hover:bg-muted/30">
                        <TableHead className="pl-4 font-semibold text-xs uppercase tracking-wider">
                          Referência
                        </TableHead>
                        <TableHead className="font-semibold text-xs uppercase tracking-wider">
                          Pagador
                        </TableHead>
                        <TableHead className="font-semibold text-xs uppercase tracking-wider">
                          Método
                        </TableHead>
                        <TableHead className="font-semibold text-xs uppercase tracking-wider">
                          Status
                        </TableHead>
                        <TableHead className="text-right pr-4 font-semibold text-xs uppercase tracking-wider">
                          Valor
                        </TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {filtered.length === 0 ? (
                        <TableRow>
                          <TableCell
                            colSpan={5}
                            className="text-center text-muted-foreground py-12 text-sm"
                          >
                            Nenhuma transação encontrada.
                          </TableCell>
                        </TableRow>
                      ) : (
                        filtered.map((payment) => (
                          <TableRow
                            key={payment.id}
                            className="cursor-pointer hover:bg-muted/40 transition-colors"
                            onClick={() => setSelected(payment)}
                          >
                            <TableCell className="pl-4">
                              <span className="font-mono text-sm font-semibold">
                                {payment.reference}
                              </span>
                            </TableCell>
                            <TableCell>
                              <div className="flex items-center gap-2.5">
                                <PayerAvatar name={payment.payer.name} />
                                <span className="text-sm font-medium">{payment.payer.name}</span>
                              </div>
                            </TableCell>
                            <TableCell>
                              <PaymentMethodBadge method={payment.method} />
                            </TableCell>
                            <TableCell>
                              <PaymentStatusBadge status={payment.status} />
                            </TableCell>
                            <TableCell className="text-right pr-4">
                              <span
                                className={cn(
                                  'text-sm font-bold tabular-nums',
                                  payment.status === 'Paid' && 'text-emerald-700 dark:text-emerald-400',
                                  payment.status === 'Failed' && 'text-red-600 dark:text-red-400',
                                  payment.status === 'Refunded' && 'text-orange-600 dark:text-orange-400',
                                )}
                              >
                                {formatCurrency(payment.amount)}
                              </span>
                            </TableCell>
                          </TableRow>
                        ))
                      )}
                    </TableBody>
                  </Table>

                  {filtered.length > 0 && (
                    <div className="border-t px-4 py-3 bg-muted/20 flex items-center justify-between">
                      <p className="text-xs text-muted-foreground">
                        {filtered.length} {filtered.length === 1 ? 'transação' : 'transações'}
                      </p>
                      <p className="text-sm font-bold">
                        {formatCurrency(filtered.reduce((s, p) => s + p.amount, 0))}
                      </p>
                    </div>
                  )}
                </Card>
              </TabsContent>
            ))}
          </Tabs>
        </div>
      </div>

      <PaymentDetailSheet payment={selected} onClose={() => setSelected(null)} />
    </>
  );
}
