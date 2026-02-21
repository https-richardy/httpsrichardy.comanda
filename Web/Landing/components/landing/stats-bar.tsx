export function StatsBar() {
    const stats = [
        { value: "2.000+", label: "Estabelecimentos ativos" },
        { value: "500k+", label: "Pedidos por mes" },
        { value: "R$0", label: "em taxas por pedido" },
        { value: "99.9%", label: "de uptime garantido" },
    ]

    return (
        <section className="border-y border-border bg-muted/50">
            <div className="mx-auto grid max-w-7xl grid-cols-2 gap-6 px-6 py-12 lg:grid-cols-4 lg:gap-0 lg:divide-x lg:divide-border">
                {stats.map((stat) => (
                    <div key={stat.label} className="flex flex-col items-center gap-1 text-center">
                        <span className="font-mono text-3xl font-bold text-[#ed214d]">{stat.value}</span>
                        <span className="text-sm text-muted-foreground">{stat.label}</span>
                    </div>
                ))}
            </div>
        </section>
    )
}
