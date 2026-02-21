import Image from "next/image"

import { ArrowRight } from "lucide-react"
import { Button } from "@/components/ui/button"

export function Hero()
{
    return (
        <section className="relative overflow-hidden pt-32 pb-20 lg:pt-44 lg:pb-32">
            <div className="pointer-events-none absolute top-0 left-1/2 -translate-x-1/2 h-[600px] w-[800px] rounded-full bg-[#ed214d]/8 blur-[120px]" />

            <div className="relative mx-auto max-w-7xl px-6">
                <div className="grid items-center gap-12 lg:grid-cols-2 lg:gap-20">
                    <div className="flex flex-col gap-8">
                        <h1 className="font-mono text-4xl font-bold leading-tight tracking-tight text-foreground sm:text-5xl lg:text-6xl text-balance">
                            GERENCIE PEDIDOS.{" "}
                            <span className="text-[#ed214d]">SEM COMPLICAÇÃO.</span>{" "}
                            SEM TAXAS.
                        </h1>

                        <p className="max-w-lg text-lg leading-relaxed text-muted-foreground">
                            O Comanda é o sistema completo para seu fast food receber pedidos online e presenciais,
                            organizar a cozinha e crescer sem pagar comissões absurdas para marketplaces.
                        </p>

                        <div className="flex flex-col gap-3 sm:flex-row sm:items-center">
                            <Button size="lg" className="bg-[#ed214d] px-8 text-base font-semibold text-white hover:bg-[#d91b43] transition-all">
                                 Comece agora por R$100/mês
                                <ArrowRight className="ml-2 h-4 w-4" />
                            </Button>
                            <span className="text-sm text-muted-foreground">
                                Cancele quando quiser. Sem fidelidade.
                            </span>
                        </div>

                        <div className="flex items-center gap-6 pt-2">
                            <div className="flex flex-col">
                                <span className="font-mono text-2xl font-bold text-foreground">0%</span>
                                <span className="text-xs text-muted-foreground">de taxa por pedido</span>
                            </div>
                            <div className="h-8 w-px bg-border" />
                            <div className="flex flex-col">
                                <span className="font-mono text-2xl font-bold text-foreground">5min</span>
                                <span className="text-xs text-muted-foreground">para configurar</span>
                            </div>
                            <div className="h-8 w-px bg-border" />
                            <div className="flex flex-col">
                                <span className="font-mono text-2xl font-bold text-foreground">24/7</span>
                                <span className="text-xs text-muted-foreground">suporte dedicado</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    )
}
