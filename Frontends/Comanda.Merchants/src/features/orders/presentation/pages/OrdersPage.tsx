import { useEffect, useState } from 'react';
import {
  AlertTriangle,
  ChefHat,
  CheckCircle2,
  Clock,
  CreditCard,
  Flame,
  MapPin,
  PackageCheck,
  RotateCcw,
  ShoppingBag,
  Store,
  Truck,
  Utensils,
  XCircle,
  Tag as TagIcon,
  Plus,
  Smartphone,
  CreditCard as CreditCardIcon,
  Banknote,
  Search,
  Minus,
  ShoppingCart,
} from 'lucide-react';

import { Badge } from '@/shared/components/ui/badge';
import { Button } from '@/shared/components/ui/button';
import { Card, CardContent } from '@/shared/components/ui/card';
import { Input } from '@/shared/components/ui/input';
import { Label } from '@/shared/components/ui/label';
import { ScrollArea } from '@/shared/components/ui/scroll-area';
import { Separator } from '@/shared/components/ui/separator';
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
} from '@/shared/components/ui/sheet';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/shared/components/ui/tabs';
import {
  Board,
  BoardColumn,
  BoardColumnHeader,
  BoardEmpty,
  BoardItem,
} from '@/shared/components/common/board';
import { cn } from '@/shared/utils';

import { MOCK_ORDERS } from '../../data/mock';
import type { Order, OrderStatus } from '../../domain/entities';
import { MOCK_PRODUCTS } from '@/features/products/data/mock';
import type { Product } from '@/features/products/domain/entities';

// ─── Types ───────────────────────────────────────────────────────────────────

type WizardStep = 1 | 2 | 3;
type OrderType = 'counter' | 'tab' | null;
type CartItem = { product: Product; quantity: number };

// ─── Kanban config ────────────────────────────────────────────────────────────

const KANBAN_COLUMNS: {
  id: OrderStatus;
  label: string;
  icon: React.ReactNode;
  colorClass: string;
  headerClass: string;
  dotClass: string;
}[] = [
  {
    id: 'Pending',
    label: 'Pendentes',
    icon: <Clock className="size-3.5" />,
    colorClass: 'text-amber-600',
    headerClass: 'border-amber-200/60 bg-amber-50/40 dark:bg-amber-950/20',
    dotClass: 'bg-amber-500',
  },
  {
    id: 'Confirmed',
    label: 'Confirmados',
    icon: <ShoppingBag className="size-3.5" />,
    colorClass: 'text-blue-600',
    headerClass: 'border-blue-200/60 bg-blue-50/40 dark:bg-blue-950/20',
    dotClass: 'bg-blue-500',
  },
  {
    id: 'InPreparation',
    label: 'Em Preparo',
    icon: <ChefHat className="size-3.5" />,
    colorClass: 'text-violet-600',
    headerClass: 'border-violet-200/60 bg-violet-50/40 dark:bg-violet-950/20',
    dotClass: 'bg-violet-500',
  },
  {
    id: 'Ready',
    label: 'Prontos',
    icon: <CheckCircle2 className="size-3.5" />,
    colorClass: 'text-emerald-600',
    headerClass: 'border-emerald-200/60 bg-emerald-50/40 dark:bg-emerald-950/20',
    dotClass: 'bg-emerald-500',
  },
];

const ARCHIVED_STATUSES: OrderStatus[] = [
  'Finalized',
  'Cancelled',
  'Failed',
  'Refunded',
  'Returned',
];

const PRIORITY_CONFIG: Record<
  Order['priority'],
  { label: string; className: string; icon?: React.ReactNode }
> = {
  High: {
    label: 'Alta',
    className: 'bg-red-100 text-red-700 border border-red-200 dark:bg-red-950/30 dark:text-red-400',
    icon: <Flame className="size-3" />,
  },
  Normal: {
    label: 'Normal',
    className: 'bg-blue-100 text-blue-700 border border-blue-200 dark:bg-blue-950/30 dark:text-blue-400',
  },
  Low: {
    label: 'Baixa',
    className: 'bg-muted text-muted-foreground border border-border',
  },
};

const FULFILLMENT_CONFIG: Record<
  Order['fulfillment'],
  { label: string; icon: React.ReactNode }
> = {
  Delivery: { label: 'Entrega', icon: <Truck className="size-3" /> },
  Pickup: { label: 'Retirada', icon: <Store className="size-3" /> },
  Unspecified: { label: 'Não definido', icon: null },
};

const ARCHIVED_STATUS_CONFIG: Partial<
  Record<OrderStatus, { label: string; className: string; icon: React.ReactNode }>
> = {
  Finalized: {
    label: 'Finalizado',
    className: 'bg-muted text-muted-foreground',
    icon: <PackageCheck className="size-3" />,
  },
  Cancelled: {
    label: 'Cancelado',
    className: 'bg-red-100 text-red-700 dark:bg-red-950/30 dark:text-red-400',
    icon: <XCircle className="size-3" />,
  },
  Failed: {
    label: 'Falhou',
    className: 'bg-red-100 text-red-700 dark:bg-red-950/30 dark:text-red-400',
    icon: <AlertTriangle className="size-3" />,
  },
  Refunded: {
    label: 'Reembolsado',
    className: 'bg-orange-100 text-orange-700 dark:bg-orange-950/30 dark:text-orange-400',
    icon: <CreditCard className="size-3" />,
  },
  Returned: {
    label: 'Devolvido',
    className: 'bg-orange-100 text-orange-700 dark:bg-orange-950/30 dark:text-orange-400',
    icon: <RotateCcw className="size-3" />,
  },
};

// ─── Helpers ─────────────────────────────────────────────────────────────────

function formatCurrency(value: number) {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(value);
}

function getOrderTotal(order: Order) {
  return order.items.reduce((sum, item) => sum + item.totalPrice, 0);
}

// ─── Sub-components ───────────────────────────────────────────────────────────

function StatCard({
  icon,
  label,
  value,
  iconClass,
}: {
  icon: React.ReactNode;
  label: string;
  value: number;
  iconClass?: string;
}) {
  return (
    <div className="rounded-xl border bg-card p-4 flex items-center gap-3 shadow-sm">
      <div className={cn('rounded-lg p-2.5 bg-muted/60', iconClass)}>{icon}</div>
      <div>
        <p className="text-xs text-muted-foreground font-medium uppercase tracking-wide">{label}</p>
        <p className="text-2xl font-bold leading-none mt-0.5">{value}</p>
      </div>
    </div>
  );
}

function OrderCard({ order, onClick }: { order: Order; onClick: () => void }) {
  const total = getOrderTotal(order);
  const previewItems = order.items.slice(0, 2);
  const extraCount = order.items.length - 2;
  const priority = PRIORITY_CONFIG[order.priority];
  const fulfillment = FULFILLMENT_CONFIG[order.fulfillment];

  return (
    <Card
      className={cn(
        'mx-2 mb-2 cursor-pointer transition-all duration-150 py-3 gap-0',
        'hover:shadow-md hover:-translate-y-0.5 active:translate-y-0 active:shadow-sm',
        order.priority === 'High' && 'border-l-2 border-l-red-500',
      )}
      onClick={onClick}
    >
      <CardContent className="px-3 space-y-2.5">
        <div className="flex items-start justify-between gap-2">
          <span className="font-mono text-sm font-bold leading-none">{order.code}</span>
          <span
            className={cn(
              'inline-flex items-center gap-1 rounded-full px-2 py-0.5 text-[11px] font-medium shrink-0',
              priority.className,
            )}
          >
            {priority.icon}
            {priority.label}
          </span>
        </div>

        <div className="flex items-center gap-1.5 text-xs text-muted-foreground">
          {fulfillment.icon}
          <span>{fulfillment.label}</span>
          {order.address && (
            <span className="truncate text-muted-foreground/60">· {order.address.neighborhood}</span>
          )}
        </div>

        <div className="space-y-1">
          {previewItems.map((item, i) => (
            <div key={i} className="flex items-center gap-1.5 text-xs">
              <span className="font-semibold text-muted-foreground/80 tabular-nums w-5 shrink-0">
                {item.quantity}x
              </span>
              <span className="truncate text-foreground/80">{item.title}</span>
            </div>
          ))}
          {extraCount > 0 && (
            <p className="text-xs text-muted-foreground/60 pl-6">+{extraCount} item{extraCount > 1 ? 'ns' : ''}</p>
          )}
        </div>

        <div className="flex items-center justify-between pt-1.5 border-t border-dashed">
          <div className="flex items-center gap-1 text-xs text-muted-foreground">
            <Utensils className="size-3" />
            <span>{order.items.length} {order.items.length === 1 ? 'item' : 'itens'}</span>
          </div>
          <span className="text-sm font-bold">{formatCurrency(total)}</span>
        </div>
      </CardContent>
    </Card>
  );
}

function OrderDetailSheet({
  order,
  onClose,
}: {
  order: Order | null;
  onClose: () => void;
}) {
  if (!order) return null;

  const total = getOrderTotal(order);
  const priority = PRIORITY_CONFIG[order.priority];
  const fulfillment = FULFILLMENT_CONFIG[order.fulfillment];

  return (
    <Sheet open={!!order} onOpenChange={onClose}>
      <SheetContent className="w-full sm:max-w-[440px] p-0 flex flex-col">
        <SheetHeader className="p-5 pb-4 border-b shrink-0">
          <div className="flex items-center gap-2.5 flex-wrap">
            <SheetTitle className="font-mono text-xl">{order.code}</SheetTitle>
            <span
              className={cn(
                'inline-flex items-center gap-1 rounded-full px-2.5 py-0.5 text-xs font-medium',
                priority.className,
              )}
            >
              {priority.icon}
              {priority.label}
            </span>
          </div>
          <SheetDescription className="flex items-center gap-1.5 text-sm">
            {fulfillment.icon}
            <span>{fulfillment.label}</span>
            {order.address && (
              <span className="text-muted-foreground/60">
                · {order.address.neighborhood}, {order.address.city}
              </span>
            )}
          </SheetDescription>
        </SheetHeader>

        <ScrollArea className="flex-1">
          <div className="p-5 space-y-6">
            <section>
              <h3 className="text-xs font-semibold text-muted-foreground uppercase tracking-wider mb-3 flex items-center gap-1.5">
                <Utensils className="size-3.5" />
                Itens do pedido
              </h3>

              <div className="space-y-3">
                {order.items.map((item, i) => (
                  <div key={i} className="flex justify-between gap-3">
                    <div className="flex gap-2.5 flex-1 min-w-0">
                      <span className="text-sm font-bold text-muted-foreground tabular-nums w-6 shrink-0">
                        {item.quantity}x
                      </span>
                      <div className="min-w-0">
                        <p className="text-sm font-medium leading-snug">{item.title}</p>
                        <p className="text-xs text-muted-foreground leading-snug mt-0.5">{item.description}</p>
                      </div>
                    </div>
                    <span className="text-sm font-semibold tabular-nums shrink-0">
                      {formatCurrency(item.totalPrice)}
                    </span>
                  </div>
                ))}
              </div>

              <Separator className="mt-4" />

              <div className="flex justify-between items-center pt-3">
                <span className="text-sm font-semibold">Total do pedido</span>
                <span className="text-lg font-bold">{formatCurrency(total)}</span>
              </div>
            </section>

            {order.address && (
              <section>
                <h3 className="text-xs font-semibold text-muted-foreground uppercase tracking-wider mb-3 flex items-center gap-1.5">
                  <MapPin className="size-3.5" />
                  Endereço de entrega
                </h3>
                <div className="rounded-lg border bg-muted/30 p-4 space-y-1.5">
                  <p className="text-sm font-medium">
                    {order.address.street}, {order.address.number}
                    {order.address.complement && (
                      <span className="text-muted-foreground"> — {order.address.complement}</span>
                    )}
                  </p>
                  <p className="text-sm text-muted-foreground">
                    {order.address.neighborhood} · {order.address.city}/{order.address.state}
                  </p>
                  <p className="text-xs text-muted-foreground/70">{order.address.zipCode}</p>
                  {order.address.reference && (
                    <p className="text-xs text-muted-foreground/60 italic mt-1">
                      Ref: {order.address.reference}
                    </p>
                  )}
                </div>
              </section>
            )}

            {order.fulfillment === 'Pickup' && (
              <section>
                <h3 className="text-xs font-semibold text-muted-foreground uppercase tracking-wider mb-3 flex items-center gap-1.5">
                  <Store className="size-3.5" />
                  Retirada no local
                </h3>
                <div className="rounded-lg border bg-muted/30 p-4">
                  <p className="text-sm text-muted-foreground">
                    O cliente irá retirar o pedido diretamente no estabelecimento.
                  </p>
                </div>
              </section>
            )}
          </div>
        </ScrollArea>
      </SheetContent>
    </Sheet>
  );
}

function ArchivedOrderCard({ order, onClick }: { order: Order; onClick: () => void }) {
  const total = getOrderTotal(order);
  const config = ARCHIVED_STATUS_CONFIG[order.status];
  const fulfillment = FULFILLMENT_CONFIG[order.fulfillment];

  return (
    <Card
      className="cursor-pointer hover:shadow-md transition-all duration-150 py-4 gap-0"
      onClick={onClick}
    >
      <CardContent className="px-4 space-y-2.5">
        <div className="flex items-center justify-between gap-2">
          <span className="font-mono text-sm font-bold">{order.code}</span>
          {config && (
            <span
              className={cn(
                'inline-flex items-center gap-1 rounded-full px-2 py-0.5 text-xs font-medium',
                config.className,
              )}
            >
              {config.icon}
              {config.label}
            </span>
          )}
        </div>

        <div className="flex items-center gap-1.5 text-xs text-muted-foreground">
          {fulfillment.icon}
          <span>{fulfillment.label}</span>
          {order.address && (
            <span className="text-muted-foreground/60">· {order.address.neighborhood}</span>
          )}
        </div>

        <div className="flex items-center justify-between pt-1 border-t border-dashed">
          <span className="text-xs text-muted-foreground">
            {order.items.length} {order.items.length === 1 ? 'item' : 'itens'}
          </span>
          <span className="text-sm font-bold">{formatCurrency(total)}</span>
        </div>
      </CardContent>
    </Card>
  );
}

// ─── Counter-Order Wizard ─────────────────────────────────────────────────────

function WizardStepIndicator({ step }: { step: WizardStep }) {
  return (
    <div className="flex items-center gap-1.5 mt-1">
      {([1, 2, 3] as WizardStep[]).map((s) => (
        <div
          key={s}
          className={cn(
            'h-1.5 flex-1 rounded-full transition-colors duration-200',
            s <= step ? 'bg-primary' : 'bg-muted',
          )}
        />
      ))}
      <span className="text-xs text-muted-foreground ml-1 shrink-0">
        Passo {step} de 3
      </span>
    </div>
  );
}

interface CounterOrderWizardProps {
  open: boolean;
  onClose: () => void;
  onRegister: (order: Order) => void;
}

function CounterOrderWizard({ open, onClose, onRegister }: CounterOrderWizardProps) {
  const [step, setStep] = useState<WizardStep>(1);
  const [orderType, setOrderType] = useState<OrderType>(null);
  const [tagNumber, setTagNumber] = useState('');
  const [cartItems, setCartItems] = useState<CartItem[]>([]);
  const [productSearch, setProductSearch] = useState('');
  const [paymentMethod, setPaymentMethod] = useState<'Pix' | 'Card' | 'Cash' | null>(null);
  const [paymentAmount, setPaymentAmount] = useState('');

  const cartTotal = cartItems.reduce((s, i) => s + i.product.price * i.quantity, 0);
  const cartCount = cartItems.reduce((s, i) => s + i.quantity, 0);

  const resetWizard = () => {
    setStep(1);
    setOrderType(null);
    setTagNumber('');
    setCartItems([]);
    setProductSearch('');
    setPaymentMethod(null);
    setPaymentAmount('');
  };

  const closeWizard = () => {
    onClose();
    setTimeout(resetWizard, 300);
  };

  // Sync payment amount when entering step 3
  useEffect(() => {
    if (step === 3) {
      setPaymentAmount(cartTotal.toFixed(2));
    }
  }, [step, cartTotal]);

  const canProceedStep1 =
    orderType !== null && (orderType === 'counter' || (orderType === 'tab' && tagNumber.trim() !== ''));

  const addToCart = (product: Product) => {
    setCartItems((prev) => {
      const existing = prev.find((i) => i.product.id === product.id);
      if (existing)
        return prev.map((i) =>
          i.product.id === product.id ? { ...i, quantity: i.quantity + 1 } : i,
        );
      return [...prev, { product, quantity: 1 }];
    });
  };

  const removeFromCart = (productId: string) => {
    setCartItems((prev) => {
      const existing = prev.find((i) => i.product.id === productId);
      if (!existing) return prev;
      if (existing.quantity === 1) return prev.filter((i) => i.product.id !== productId);
      return prev.map((i) =>
        i.product.id === productId ? { ...i, quantity: i.quantity - 1 } : i,
      );
    });
  };

  const getQuantity = (productId: string) =>
    cartItems.find((i) => i.product.id === productId)?.quantity ?? 0;

  const filteredProducts = MOCK_PRODUCTS.filter((p) => {
    const q = productSearch.toLowerCase();
    return p.title.toLowerCase().includes(q) || p.description.toLowerCase().includes(q);
  });

  const handleRegister = () => {
    const code =
      orderType === 'tab'
        ? `#TAG-${tagNumber.padStart(3, '0')}`
        : `#BAL-${String(Date.now()).slice(-4)}`;

    const newOrder: Order = {
      id: crypto.randomUUID(),
      code,
      status: 'Confirmed',
      priority: 'Normal',
      fulfillment: 'Pickup',
      items: cartItems.map((ci) => ({
        title: ci.product.title,
        description: ci.product.description,
        quantity: ci.quantity,
        unitPrice: ci.product.price,
        totalPrice: ci.product.price * ci.quantity,
      })),
      merchantId: 'merch-1',
      consumerId: 'counter-order',
    };

    onRegister(newOrder);
    closeWizard();
  };

  return (
    <Sheet open={open} onOpenChange={(o) => !o && closeWizard()}>
      <SheetContent className="w-full sm:max-w-[520px] p-0 flex flex-col">
        <SheetHeader className="p-5 pb-3 border-b shrink-0">
          <SheetTitle className="flex items-center gap-2">
            <ShoppingCart className="size-4" />
            Novo Pedido
          </SheetTitle>
          <SheetDescription asChild>
            <div>
              <WizardStepIndicator step={step} />
            </div>
          </SheetDescription>
        </SheetHeader>

        {/* ── Step 1: Order type ── */}
        {step === 1 && (
          <div className="flex flex-col flex-1 overflow-hidden">
            <div className="flex-1 overflow-y-auto p-5 space-y-4">
              <div>
                <p className="text-sm font-semibold mb-1">Tipo de Pedido</p>
                <p className="text-xs text-muted-foreground">Escolha como o pedido será identificado.</p>
              </div>

              <div className="space-y-3">
                {/* Counter option */}
                <button
                  type="button"
                  className={cn(
                    'w-full text-left rounded-xl border-2 p-4 transition-colors duration-150',
                    orderType === 'counter'
                      ? 'border-primary bg-primary/5'
                      : 'border-border hover:border-primary/40',
                  )}
                  onClick={() => setOrderType('counter')}
                >
                  <div className="flex items-center gap-3">
                    <div className="rounded-lg bg-muted p-2.5">
                      <ShoppingBag className="size-5 text-muted-foreground" />
                    </div>
                    <div>
                      <p className="font-semibold text-sm">Balcão</p>
                      <p className="text-xs text-muted-foreground mt-0.5">
                        Pedido simples no balcão, sem comanda
                      </p>
                    </div>
                  </div>
                </button>

                {/* Tab option */}
                <button
                  type="button"
                  className={cn(
                    'w-full text-left rounded-xl border-2 p-4 transition-colors duration-150',
                    orderType === 'tab'
                      ? 'border-primary bg-primary/5'
                      : 'border-border hover:border-primary/40',
                  )}
                  onClick={() => setOrderType('tab')}
                >
                  <div className="flex items-center gap-3">
                    <div className="rounded-lg bg-muted p-2.5">
                      <TagIcon className="size-5 text-muted-foreground" />
                    </div>
                    <div>
                      <p className="font-semibold text-sm">Comanda</p>
                      <p className="text-xs text-muted-foreground mt-0.5">
                        Associar pedido a uma comanda de mesa
                      </p>
                    </div>
                  </div>

                  {orderType === 'tab' && (
                    <div
                      className="mt-3 space-y-1.5"
                      onClick={(e) => e.stopPropagation()}
                    >
                      <Label htmlFor="tag-number-wizard" className="text-xs">
                        Número da comanda
                      </Label>
                      <Input
                        id="tag-number-wizard"
                        type="number"
                        min={1}
                        max={999}
                        placeholder="Ex: 12"
                        value={tagNumber}
                        onChange={(e) => setTagNumber(e.target.value)}
                        autoFocus
                      />
                    </div>
                  )}
                </button>
              </div>
            </div>

            <div className="border-t p-4 shrink-0">
              <Button
                className="w-full"
                disabled={!canProceedStep1}
                onClick={() => setStep(2)}
              >
                Continuar →
              </Button>
            </div>
          </div>
        )}

        {/* ── Step 2: Products ── */}
        {step === 2 && (
          <div className="flex flex-col flex-1 overflow-hidden">
            <div className="p-4 border-b shrink-0">
              <div className="relative">
                <Search className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground pointer-events-none" />
                <Input
                  className="pl-9"
                  placeholder="Buscar produtos..."
                  value={productSearch}
                  onChange={(e) => setProductSearch(e.target.value)}
                />
              </div>
            </div>

            <ScrollArea className="flex-1" style={{ height: 'calc(100vh - 380px)' }}>
              <div className="p-4 space-y-2">
                {filteredProducts.map((product) => {
                  const qty = getQuantity(product.id);
                  return (
                    <div
                      key={product.id}
                      className="flex items-center gap-3 rounded-lg border p-3"
                    >
                      {/* Thumbnail */}
                      <div className="size-8 rounded bg-muted shrink-0 overflow-hidden flex items-center justify-center">
                        {product.image ? (
                          <img
                            src={product.image}
                            alt={product.title}
                            className="w-full h-full object-cover"
                          />
                        ) : (
                          <span className="text-[10px] font-bold text-muted-foreground">
                            {product.title.slice(0, 2).toUpperCase()}
                          </span>
                        )}
                      </div>

                      {/* Info */}
                      <div className="flex-1 min-w-0">
                        <p className="text-sm font-medium leading-none truncate">{product.title}</p>
                        <p className="text-xs text-emerald-600 font-semibold mt-0.5">
                          {formatCurrency(product.price)}
                        </p>
                      </div>

                      {/* Cart controls */}
                      <div className="flex items-center gap-1 shrink-0">
                        {qty > 0 ? (
                          <>
                            <Button
                              size="icon"
                              variant="outline"
                              className="size-7"
                              onClick={() => removeFromCart(product.id)}
                            >
                              <Minus className="size-3" />
                            </Button>
                            <span className="text-sm font-bold tabular-nums w-5 text-center">{qty}</span>
                            <Button
                              size="icon"
                              variant="outline"
                              className="size-7"
                              onClick={() => addToCart(product)}
                            >
                              <Plus className="size-3" />
                            </Button>
                          </>
                        ) : (
                          <Button
                            size="icon"
                            variant="outline"
                            className="size-7"
                            onClick={() => addToCart(product)}
                          >
                            <Plus className="size-3" />
                          </Button>
                        )}
                      </div>
                    </div>
                  );
                })}
              </div>
            </ScrollArea>

            {/* Sticky cart summary */}
            <div className="border-t p-4 shrink-0 bg-card">
              <div className="flex items-center justify-between mb-3">
                <div className="flex items-center gap-2 text-sm text-muted-foreground">
                  <ShoppingCart className="size-4" />
                  <span>
                    {cartCount} {cartCount === 1 ? 'item' : 'itens'}
                  </span>
                </div>
                <span className="font-bold">{formatCurrency(cartTotal)}</span>
              </div>
              <div className="flex gap-2">
                <Button variant="outline" onClick={() => setStep(1)}>
                  ← Voltar
                </Button>
                <Button
                  className="flex-1"
                  disabled={cartItems.length === 0}
                  onClick={() => setStep(3)}
                >
                  Continuar →
                </Button>
              </div>
            </div>
          </div>
        )}

        {/* ── Step 3: Payment ── */}
        {step === 3 && (
          <div className="flex flex-col flex-1 overflow-hidden">
            <ScrollArea className="flex-1">
              <div className="p-5 space-y-5">
                {/* Order summary */}
                <div>
                  <p className="text-xs font-semibold text-muted-foreground uppercase tracking-wider mb-3">
                    Resumo do pedido
                  </p>
                  <div className="space-y-2">
                    {cartItems.map((ci) => (
                      <div key={ci.product.id} className="flex justify-between text-sm">
                        <span className="text-muted-foreground">
                          {ci.quantity}x {ci.product.title}
                        </span>
                        <span className="font-semibold tabular-nums">
                          {formatCurrency(ci.product.price * ci.quantity)}
                        </span>
                      </div>
                    ))}
                  </div>
                  <Separator className="my-3" />
                  <div className="flex justify-between items-center">
                    <span className="font-semibold">Total</span>
                    <span className="text-lg font-bold">{formatCurrency(cartTotal)}</span>
                  </div>
                </div>

                {/* Payment method */}
                <div>
                  <p className="text-xs font-semibold text-muted-foreground uppercase tracking-wider mb-3">
                    Forma de pagamento
                  </p>
                  <div className="grid grid-cols-3 gap-2">
                    {(
                      [
                        { key: 'Pix', label: 'Pix', icon: <Smartphone className="size-5" /> },
                        { key: 'Card', label: 'Cartão', icon: <CreditCardIcon className="size-5" /> },
                        { key: 'Cash', label: 'Dinheiro', icon: <Banknote className="size-5" /> },
                      ] as { key: 'Pix' | 'Card' | 'Cash'; label: string; icon: React.ReactNode }[]
                    ).map(({ key, label, icon }) => (
                      <button
                        key={key}
                        type="button"
                        className={cn(
                          'rounded-xl border-2 p-3 flex flex-col items-center gap-1.5 transition-colors duration-150 text-sm font-medium',
                          paymentMethod === key
                            ? 'border-primary bg-primary/5 text-primary'
                            : 'border-border hover:border-primary/40 text-muted-foreground',
                        )}
                        onClick={() => setPaymentMethod(key)}
                      >
                        {icon}
                        {label}
                      </button>
                    ))}
                  </div>
                </div>

                {/* Amount */}
                <div className="space-y-1.5">
                  <Label htmlFor="payment-amount">Valor recebido (R$)</Label>
                  <Input
                    id="payment-amount"
                    type="number"
                    min={0}
                    step={0.01}
                    value={paymentAmount}
                    onChange={(e) => setPaymentAmount(e.target.value)}
                  />
                </div>
              </div>
            </ScrollArea>

            <div className="border-t p-4 shrink-0 space-y-2">
              <Button
                className="w-full bg-emerald-600 hover:bg-emerald-700 text-white"
                disabled={!paymentMethod}
                onClick={handleRegister}
              >
                Registrar Pedido
              </Button>
              <Button variant="outline" className="w-full" onClick={() => setStep(2)}>
                ← Voltar
              </Button>
            </div>
          </div>
        )}
      </SheetContent>
    </Sheet>
  );
}

// ─── Main Page ────────────────────────────────────────────────────────────────

export function OrdersPage() {
  const [orders, setOrders] = useState<Order[]>(MOCK_ORDERS);
  const [selectedOrder, setSelectedOrder] = useState<Order | null>(null);
  const [wizardOpen, setWizardOpen] = useState(false);

  const activeOrders = orders.filter((o) => !ARCHIVED_STATUSES.includes(o.status));
  const archivedOrders = orders.filter((o) => ARCHIVED_STATUSES.includes(o.status));

  const pendingCount = orders.filter((o) => o.status === 'Pending').length;
  const inPreparationCount = orders.filter((o) => o.status === 'InPreparation').length;
  const readyCount = orders.filter((o) => o.status === 'Ready').length;

  const handleDrop = (orderId: string, newStatus: string) => {
    setOrders((prev) =>
      prev.map((o) => (o.id === orderId ? { ...o, status: newStatus as OrderStatus } : o)),
    );
  };

  const handleRegisterOrder = (newOrder: Order) => {
    setOrders((prev) => [newOrder, ...prev]);
  };

  return (
    <>
      <div className="flex flex-col gap-5 p-4 lg:p-6">
        <div className="flex items-start justify-between">
          <div>
            <h1 className="text-2xl font-bold tracking-tight">Pedidos</h1>
            <p className="text-sm text-muted-foreground mt-0.5">
              Acompanhe e gerencie os pedidos em tempo real
            </p>
          </div>
          <Button onClick={() => setWizardOpen(true)}>
            <Plus className="size-4" />
            Novo Pedido
          </Button>
        </div>

        <div className="grid grid-cols-2 lg:grid-cols-4 gap-3">
          <StatCard
            icon={<ShoppingBag className="size-4 text-foreground/70" />}
            label="Total hoje"
            value={orders.length}
          />
          <StatCard
            icon={<Clock className="size-4 text-amber-600" />}
            label="Pendentes"
            value={pendingCount}
            iconClass="text-amber-600"
          />
          <StatCard
            icon={<ChefHat className="size-4 text-violet-600" />}
            label="Em preparo"
            value={inPreparationCount}
            iconClass="text-violet-600"
          />
          <StatCard
            icon={<CheckCircle2 className="size-4 text-emerald-600" />}
            label="Prontos"
            value={readyCount}
            iconClass="text-emerald-600"
          />
        </div>

        <Tabs defaultValue="active">
          <TabsList className="w-fit">
            <TabsTrigger value="active" className="gap-1.5">
              Em andamento
              {activeOrders.length > 0 && (
                <Badge
                  variant="secondary"
                  className="h-4 px-1.5 text-[10px] font-bold rounded-full"
                >
                  {activeOrders.length}
                </Badge>
              )}
            </TabsTrigger>
            <TabsTrigger value="archived" className="gap-1.5">
              Arquivados
              {archivedOrders.length > 0 && (
                <Badge
                  variant="outline"
                  className="h-4 px-1.5 text-[10px] font-bold rounded-full"
                >
                  {archivedOrders.length}
                </Badge>
              )}
            </TabsTrigger>
          </TabsList>

          <TabsContent value="active" className="mt-4">
            <Board>
              {KANBAN_COLUMNS.map((col) => {
                const colOrders = activeOrders.filter((o) => o.status === col.id);

                return (
                  <BoardColumn key={col.id} id={col.id} onDrop={handleDrop}>
                    <BoardColumnHeader className={col.headerClass}>
                      <div className={cn('flex items-center justify-between w-full', col.colorClass)}>
                        <div className="flex items-center gap-1.5 font-semibold text-sm">
                          <span
                            className={cn('size-1.5 rounded-full shrink-0', col.dotClass)}
                          />
                          {col.icon}
                          {col.label}
                        </div>
                        <span className="text-xs font-bold tabular-nums bg-background/70 rounded-full px-2 py-0.5 border min-w-6 text-center">
                          {colOrders.length}
                        </span>
                      </div>
                    </BoardColumnHeader>

                    {colOrders.length === 0 ? (
                      <BoardEmpty
                        message="Nenhum pedido aqui"
                        className="py-10 text-xs"
                      />
                    ) : (
                      <div className="flex flex-col pt-2 pb-1">
                        {colOrders.map((order) => (
                          <BoardItem key={order.id} id={order.id} draggable>
                            <OrderCard
                              order={order}
                              onClick={() => setSelectedOrder(order)}
                            />
                          </BoardItem>
                        ))}
                      </div>
                    )}
                  </BoardColumn>
                );
              })}
            </Board>
          </TabsContent>

          <TabsContent value="archived" className="mt-4">
            {archivedOrders.length === 0 ? (
              <div className="flex flex-col items-center justify-center py-16 text-muted-foreground gap-2">
                <PackageCheck className="size-8 opacity-30" />
                <p className="text-sm">Nenhum pedido arquivado.</p>
              </div>
            ) : (
              <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
                {archivedOrders.map((order) => (
                  <ArchivedOrderCard
                    key={order.id}
                    order={order}
                    onClick={() => setSelectedOrder(order)}
                  />
                ))}
              </div>
            )}
          </TabsContent>
        </Tabs>
      </div>

      <OrderDetailSheet order={selectedOrder} onClose={() => setSelectedOrder(null)} />

      <CounterOrderWizard
        open={wizardOpen}
        onClose={() => setWizardOpen(false)}
        onRegister={handleRegisterOrder}
      />
    </>
  );
}
