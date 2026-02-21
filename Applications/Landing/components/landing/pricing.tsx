import { Check, ArrowRight, Sparkles, Zap, ShieldCheck } from "lucide-react"
import { Button } from "@/components/ui/button"

const included = [
    "Cardapio digital ilimitado",
    "Pedidos online e presenciais",
    "Painel de gestao da cozinha",
    "Relatorios de vendas completos",
    "Suporte via chat 24/7",
    "Sem taxa por pedido",
    "Atualizacoes gratuitas",
    "Dominio personalizado",
]

const guarantees = [
    { icon: Zap, text: "Ativo em 5 minutos" },
    { icon: ShieldCheck, text: "7 dias gratis" },
    { icon: Sparkles, text: "Sem fidelidade" },
]

export function Pricing() {
    return (
        <section id="preco" className="py-24 lg:py-32">
            <div className="mx-auto max-w-7xl px-6">
                <div className="mx-auto max-w-2xl text-center">
                    <span className="inline-flex items-center gap-1.5 rounded-full bg-[#ed214d]/10 px-4 py-1.5 text-sm font-medium text-[#ed214d]">
                        <Sparkles className="h-3.5 w-3.5" />
                        Preco simples
                    </span>
                    <h2 className="mt-5 font-mono text-3xl font-bold tracking-tight text-foreground sm:text-4xl text-balance">
                        Um unico plano. Tudo incluso. Sem surpresas.
                    </h2>
                    <p className="mt-4 text-lg leading-relaxed text-muted-foreground">
                        Enquanto marketplaces cobram ate 27% por pedido, o Comanda cobra um valor fixo mensal.
                        Quanto mais voce vende, mais voce economiza.
                    </p>
                </div>

                <div className="mx-auto mt-16 max-w-lg">
                    <div className="group relative">
                        <div className="absolute -inset-[2px] rounded-3xl bg-gradient-to-b from-[#ed214d] via-[#ed214d]/40 to-[#ed214d]/10 opacity-100 blur-[1px]" />
                        <div className="relative overflow-hidden rounded-3xl bg-background">
                            <div className="h-1.5 w-full bg-gradient-to-r from-[#ed214d]/60 via-[#ed214d] to-[#ed214d]/60" />
                            <div className="pointer-events-none absolute -top-24 left-1/2 -translate-x-1/2 h-48 w-96 rounded-full bg-[#ed214d]/6 blur-[80px]" />
                            <div className="relative px-8 pt-8 pb-10 sm:px-10 sm:pt-10 sm:pb-12">
                                <div className="flex items-start justify-between">
                                    <div>
                                        <h3 className="font-mono text-xl font-bold text-foreground">
                                            Plano Completo
                                        </h3>
                                        <p className="mt-1 text-sm text-muted-foreground">
                                            Tudo que voce precisa para gerenciar seu negocio
                                        </p>
                                    </div>
                                    <div className="flex items-center gap-1.5 rounded-full bg-[#ed214d] px-3 py-1 text-xs font-bold text-white shadow-lg shadow-[#ed214d]/25">
                                        <Sparkles className="h-3 w-3" />
                                        Popular
                                    </div>
                                </div>

                                <div className="mt-8 flex items-baseline gap-2">
                                    <span className="text-sm text-muted-foreground line-through">R$200</span>
                                </div>
                                <div className="flex items-baseline gap-1">
                                    <span className="font-mono text-6xl font-bold tracking-tight text-foreground">R$100</span>
                                    <span className="text-lg text-muted-foreground">/mes</span>
                                </div>

                                <p className="mt-2 text-sm text-muted-foreground">
                                    Cancele quando quiser. Sem fidelidade nem multa.
                                </p>

                                <Button
                                    size="lg"
                                    className="mt-8 w-full bg-[#ed214d] text-base font-semibold text-white shadow-lg shadow-[#ed214d]/25 hover:bg-[#d91b43] hover:shadow-[#ed214d]/40 transition-all duration-300 h-14 rounded-xl"
                                >
                                    Comecar agora - 7 dias gratis
                                    <ArrowRight className="ml-2 h-4 w-4" />
                                </Button>

                                <p className="mt-3 text-center text-xs text-muted-foreground">
                                    Sem cartao de credito nos primeiros 7 dias
                                </p>

                                <div className="mt-6 flex items-center justify-center gap-4">
                                    {guarantees.map(({ icon: Icon, text }) => (
                                        <div key={text} className="flex items-center gap-1.5 text-xs text-muted-foreground">
                                            <Icon className="h-3.5 w-3.5 text-[#ed214d]" />
                                            <span>{text}</span>
                                        </div>
                                    ))}
                                </div>

                                <div className="relative mt-8">
                                    <div className="absolute inset-0 flex items-center">
                                        <div className="w-full border-t border-border" />
                                    </div>
                                    <div className="relative flex justify-center">
                                        <span className="bg-background px-4 text-xs font-semibold uppercase tracking-wider text-muted-foreground">
                                            Tudo incluso
                                        </span>
                                    </div>
                                </div>

                                <ul className="mt-8 grid grid-cols-1 gap-3 sm:grid-cols-2">
                                    {included.map((item) => (
                                        <li key={item} className="flex items-center gap-3 text-sm text-foreground/80">
                                            <div className="flex h-5 w-5 shrink-0 items-center justify-center rounded-full bg-[#ed214d]/10">
                                                <Check className="h-3 w-3 text-[#ed214d]" />
                                            </div>
                                            {item}
                                        </li>
                                    ))}
                                </ul>
                            </div>
                        </div>
                    </div>

                    <div className="mt-8 rounded-2xl border border-border bg-muted/50 p-6 sm:p-8">
                        <div className="flex items-center gap-2">
                            <div className="flex h-8 w-8 items-center justify-center rounded-lg bg-[#ed214d]/10">
                                <Zap className="h-4 w-4 text-[#ed214d]" />
                            </div>
                            <p className="text-sm font-bold text-foreground">
                                Faça as contas
                            </p>
                        </div>
                        <p className="mt-3 text-sm leading-relaxed text-muted-foreground">
                            Se você vende <span className="font-semibold text-foreground">R$10.000/mês</span> em pedidos,
                            um marketplace cobraria até{" "}
                            <span className="font-semibold text-[#ed214d]">R$2.700 em comissões</span>.
                            Com o Comanda, você paga apenas{" "}
                            <span className="font-semibold text-foreground">R$100 fixos</span>.{" "}
                            Isso é uma economia de{" "}
                            <span className="inline-flex items-baseline gap-1 font-mono font-bold text-[#ed214d]">R$2.600/mês</span>.
                        </p>
                    </div>
                </div>
            </div>
        </section>
    )
}
