import Image from "next/image"

export function Footer()
{
    return (
        <footer className="border-t border-border py-12">
            <div className="mx-auto max-w-7xl px-6">
                <div className="grid gap-10 sm:grid-cols-2 lg:grid-cols-4">
                    <div className="flex flex-col gap-4 lg:col-span-1">
                        <div className="flex items-center gap-2">
                            <Image src="/images/comanda-logo.png" alt="Comanda Logo" width={28} height={28} />
                            <span className="font-mono text-lg font-bold text-foreground">COMANDA</span>
                        </div>
                        <p className="text-sm leading-relaxed text-muted-foreground">
                            O sistema de gestão de pedidos feito para fast foods que querem crescer sem pagar taxas absurdas.
                        </p>
                    </div>

                    <div>
                        <h4 className="text-sm font-semibold text-foreground">Produto</h4>
                        <ul className="mt-4 flex flex-col gap-2">
                            <li>
                                <a href="#funcionalidades" className="text-sm text-muted-foreground hover:text-foreground transition-colors">
                                    Funcionalidades
                                </a>
                            </li>
                            <li>
                                <a href="#preco" className="text-sm text-muted-foreground hover:text-foreground transition-colors">
                                    Preco
                                </a>
                            </li>
                            <li>
                                <a href="#depoimentos" className="text-sm text-muted-foreground hover:text-foreground transition-colors">
                                    Depoimentos
                                </a>
                            </li>
                            <li>
                                <a href="#faq" className="text-sm text-muted-foreground hover:text-foreground transition-colors">
                                    FAQ
                                </a>
                            </li>
                        </ul>
                    </div>

                    <div>
                        <h4 className="text-sm font-semibold text-foreground">Legal</h4>
                        <ul className="mt-4 flex flex-col gap-2">
                            <li>
                                <a href="#" className="text-sm text-muted-foreground hover:text-foreground transition-colors">
                                    Termos de uso
                                </a>
                            </li>
                            <li>
                                <a href="#" className="text-sm text-muted-foreground hover:text-foreground transition-colors">
                                    Politica de privacidade
                                </a>
                            </li>
                        </ul>
                    </div>

                    <div>
                        <h4 className="text-sm font-semibold text-foreground">Contato</h4>
                        <ul className="mt-4 flex flex-col gap-2">
                            <li>
                                <a href="#" className="text-sm text-muted-foreground hover:text-foreground transition-colors">
                                    Suporte
                                </a>
                            </li>
                            <li>
                                <a href="#" className="text-sm text-muted-foreground hover:text-foreground transition-colors">
                                    WhatsApp
                                </a>
                            </li>
                        </ul>
                    </div>
                </div>

                <div className="mt-12 flex flex-col items-center justify-between gap-4 border-t border-border pt-8 sm:flex-row">
                    <p className="text-xs text-muted-foreground">
                        Comanda. Todos os direitos reservados.
                    </p>
                    <p className="text-xs text-muted-foreground">
                        Feito com dedicação para donos de fast food do Brasil.
                    </p>
                </div>
            </div>
        </footer>
    )
}
