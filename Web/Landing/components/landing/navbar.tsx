"use client"

import Image from "next/image"

import { useState } from "react"
import { Menu, X } from "lucide-react"
import { Button } from "@/components/ui/button"

const navLinks: Record<string, string>[] = [
    { label: "Funcionalidades", href: "#funcionalidades" },
    { label: "Preco", href: "#preco" },
    { label: "Depoimentos", href: "#depoimentos" },
    { label: "FAQ", href: "#faq" },
]

export function Navbar() {
    const [mobileOpen, setMobileOpen] = useState(false)

    return (
        <header className="fixed top-0 left-0 right-0 z-50 border-b border-border/50 bg-background/80 backdrop-blur-xl">
            <nav className="mx-auto flex max-w-7xl items-center justify-between px-6 py-4">
                <a href="#" className="flex items-center gap-2">
                    <Image src="/images/comanda-logo.png" alt="Comanda Logo" width={55} height={55} />
                    <span className="font-mono text-xl font-bold tracking-tight text-foreground">
                        COMANDA
                    </span>
                </a>

                <ul className="hidden items-center gap-8 md:flex">
                    {navLinks.map((link) =>
                    (
                        <li key={link.href}>
                            <a href={link.href} className="text-sm text-muted-foreground transition-colors hover:text-foreground">
                                {link.label}
                            </a>
                        </li>
                    ))}
                </ul>

                <div className="hidden items-center gap-3 md:flex">
                    <Button variant="ghost" className="text-sm text-foreground/70 hover:text-foreground hover:bg-secondary">
                        Entrar
                    </Button>
                    <Button className="bg-[#ed214d] text-sm font-semibold text-white hover:bg-[#d91b43]">
                        Criar Conta Gratis
                    </Button>
                </div>

                <button className="text-foreground md:hidden" onClick={() => setMobileOpen(!mobileOpen)} aria-label={mobileOpen ? "Fechar menu" : "Abrir menu"}>
                    {mobileOpen ? <X size={24} /> : <Menu size={24} />}
                </button>
            </nav>

            {mobileOpen && (
                <div className="border-t border-border bg-background px-6 pb-6 md:hidden">
                    <ul className="flex flex-col gap-4 pt-4">
                        {navLinks.map((link) => (
                            <li key={link.href}>
                                <a href={link.href} className="text-sm text-muted-foreground transition-colors hover:text-foreground" onClick={() => setMobileOpen(false)}>
                                    {link.label}
                                </a>
                            </li>
                        ))}
                    </ul>
                    <div className="mt-4 flex flex-col gap-3">
                        <Button variant="ghost" className="w-full justify-center text-sm text-muted-foreground">
                            Entrar
                        </Button>
                        <Button className="w-full bg-[#ed214d] text-sm font-semibold text-white hover:bg-[#d91b43]">
                            Criar Conta Gratis
                        </Button>
                    </div>
                </div>
            )}
        </header>
    )
}
