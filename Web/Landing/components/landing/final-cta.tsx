import { ArrowRight } from "lucide-react"
import { Button } from "@/components/ui/button"

export function FinalCTA() {
    return (
        <section className="border-t border-border bg-muted/50 py-24 lg:py-32">
            <div className="mx-auto max-w-7xl px-6">
                <div className="relative overflow-hidden rounded-2xl border border-[#ed214d]/30 bg-background p-10 sm:p-16 lg:p-20">
                    <div className="pointer-events-none absolute -top-40 left-1/2 -translate-x-1/2 h-80 w-[500px] rounded-full bg-[#ed214d]/10 blur-[100px]" />

                    <div className="relative mx-auto max-w-2xl text-center">
                        <h2 className="font-mono text-3xl font-bold tracking-tight text-foreground sm:text-4xl lg:text-5xl text-balance">
                            Pare de pagar comissões. Comece a lucrar de verdade.
                        </h2>

                        <p className="mt-6 text-lg leading-relaxed text-muted-foreground">
                            Junte-se a mais de 2.000 donos de fast food que já escolheram o Comanda.
                            Crie sua conta agora e comece a receber pedidos em minutos.
                        </p>

                        <div className="mt-10 flex flex-col items-center gap-4 sm:flex-row sm:justify-center">
                            <Button size="lg" className="bg-[#ed214d] px-10 text-base font-semibold text-white hover:bg-[#d91b43]">
                                Criar minha conta agora
                                <ArrowRight className="ml-2 h-4 w-4" />
                            </Button>
                        </div>

                        <p className="mt-4 text-sm text-muted-foreground">
                            R$100/mês fixos. Sem taxa por pedido. Sem fidelidade. Cancele quando quiser.
                        </p>
                    </div>
                </div>
            </div>
        </section>
    )
}
