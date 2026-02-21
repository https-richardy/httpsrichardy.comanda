import { Star } from "lucide-react"

const testimonials = [
    {
        name: "Carlos Eduardo",
        business: "Burger Point - SP",
        text: "Eu pagava quase R$3.000 por mes em comissoes pro iFood. Com o Comanda, pago R$100 e recebo os mesmos pedidos direto. Melhor decisao que tomei pro meu negocio.",
    },
    {
        name: "Ana Paula",
        business: "Hot Dog da Ana - RJ",
        text: "Configurei em menos de 10 minutos. Meus clientes adoraram o cardapio digital e eu finalmente tenho controle de todos os pedidos em um so lugar.",
    },
    {
        name: "Marcos Vinicius",
        business: "Acai do Marcos - BH",
        text: "O painel da cozinha mudou minha vida. Antes era papel pra todo lado, agora ta tudo no tablet. Menos erro, mais velocidade. E sem pagar comissao nenhuma.",
    },
    {
        name: "Fernanda Lima",
        business: "Pizza Express - POA",
        text: "O suporte e incrivel. Tive uma duvida as 23h e me responderam em minutos. O sistema e simples, direto ao ponto. Recomendo demais.",
    },
]

export function Testimonials() {
    return (
        <section id="depoimentos" className="border-y border-border bg-muted/50 py-24 lg:py-32">
            <div className="mx-auto max-w-7xl px-6">
                <div className="mx-auto max-w-2xl text-center">
                    <span className="text-sm font-medium text-[#ed214d]">Depoimentos</span>
                    <h2 className="mt-3 font-mono text-3xl font-bold tracking-tight text-foreground sm:text-4xl text-balance">
                        Donos de fast food que ja transformaram seus negocios
                    </h2>
                </div>

                <div className="mt-16 grid gap-6 sm:grid-cols-2">
                    {testimonials.map((t) => (
                        <div key={t.name} className="rounded-xl border border-border bg-card p-6 transition-all hover:border-[#ed214d]/20">
                            <div className="flex gap-1">
                                {Array.from({ length: 5 }).map((_, i) => (
                                    <Star key={i} className="h-4 w-4 fill-[#ed214d] text-[#ed214d]"/>
                                ))}
                            </div>
                            <p className="mt-4 text-sm leading-relaxed text-muted-foreground">
                                {`"${t.text}"`}
                            </p>
                            <div className="mt-4 border-t border-border pt-4">
                                <p className="text-sm font-semibold text-foreground">{t.name}</p>
                                <p className="text-xs text-muted-foreground">{t.business}</p>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </section>
    )
}
