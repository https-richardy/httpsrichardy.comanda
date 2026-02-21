import { MonitorSmartphone, ShoppingBag, Zap, Globe, Shield } from "lucide-react"

const features: Record<string, any>[] = [
    {
        icon: MonitorSmartphone,
        title: "Pedidos online e presenciais",
        description: "Receba pedidos do seu cardápio digital e do balcão em um único painel. Tudo sincronizado em tempo real.",
    },
    {
        icon: ShoppingBag,
        title: "Cardápio digital personalizado",
        description: "Crie seu cardápio online com fotos, descrições e preços. Seus clientes pedem direto pelo celular.",
    },
    {
        icon: Zap,
        title: "Gestão de cozinha",
        description: "Painel dedicado para a cozinha visualizar pedidos em fila, preparo e prontos para entrega.",
    },
    {
        icon: Globe,
        title: "Sem taxas de marketplace",
        description: "Diferente de iFood e similares, você não paga comissão por pedido. Preço fixo mensal e pronto.",
    },
    {
        icon: Shield,
        title: "Seguro e confiável",
        description: "Seus dados ficam protegidos com criptografia. Servidor dedicado com 99,9% de disponibilidade.",
    },
]

export function Features() {
    return (
        <section id="funcionalidades" className="py-24 lg:py-32">
            <div className="mx-auto max-w-7xl px-6">
                <div className="mx-auto max-w-2xl text-center">
                    <span className="text-sm font-medium text-[#ed214d]">Funcionalidades</span>
                    <h2 className="mt-3 font-mono text-3xl font-bold tracking-tight text-foreground sm:text-4xl text-balance">
                        Tudo que seu fast food precisa em um so lugar
                    </h2>
                    <p className="mt-4 text-lg leading-relaxed text-muted-foreground">
                        Chega de malabarismos entre WhatsApp, caderninho e marketplace.
                        O Comanda centraliza tudo pra voce.
                    </p>
                </div>

                <div className="mt-16 grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
                    {features.map((feature) => {
                        const Icon = feature.icon
                        return (
                            <div key={feature.title} className="group rounded-xl border border-border bg-card p-6 transition-all hover:border-[#ed214d]/30 hover:bg-[#ed214d]/5">
                                <div className="mb-4 flex h-10 w-10 items-center justify-center rounded-lg bg-[#ed214d]/10">
                                    <Icon className="h-5 w-5 text-[#ed214d]" />
                                </div>
                                <h3 className="font-mono text-lg font-semibold text-foreground">{feature.title}</h3>
                                <p className="mt-2 text-sm leading-relaxed text-muted-foreground">{feature.description}</p>
                            </div>
                        )
                    })}
                </div>
            </div>
        </section>
    )
}
