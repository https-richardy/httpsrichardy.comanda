"use client"

import {Accordion, AccordionContent, AccordionItem, AccordionTrigger } from "@/components/ui/accordion"

const faqs: Record<string, string>[] = [
    {
        question: "Preciso de algum equipamento especial?",
        answer: "Não! Você só precisa de um computador, tablet ou celular com acesso a internet. O Comanda funciona 100% no navegador, sem necessidade de instalar nada.",
    },
    {
        question: "É realmente sem taxas por pedido?",
        answer: "Sim! Você paga apenas R$100 fixos por mês. Não importa se você recebe 10 ou 10.000 pedidos, o valor é sempre o mesmo. Zero comissão por venda.",
    },
    {
        question: "Como meus clientes fazem pedidos online?",
        answer: "Você recebe um link exclusivo do seu cardápio digital. Seus clientes acessam pelo celular, escolhem os itens e fazem o pedido. Você recebe tudo no painel em tempo real.",
    },
    {
        question: "Posso cancelar a qualquer momento?",
        answer: "Sim, sem nenhum problema. Não temos fidelidade, multa ou período mínimo. Se não gostar, cancele quando quiser.",
    },
    {
        question: "É difícil de configurar?",
        answer: "Não! Em menos de 5 minutos você cria sua conta, cadastra seus produtos e já pode começar a receber pedidos. Temos tutoriais e suporte para te ajudar.",
    },
    {
        question: "Funciona para delivery e retirada?",
        answer: "Sim! O Comanda suporta pedidos para delivery, retirada no balcão e consumo no local. Você configura como quiser.",
    }
]

export function FAQ() {
    return (
        <section id="faq" className="py-24 lg:py-32">
            <div className="mx-auto max-w-3xl px-6">
                <div className="text-center">
                    <span className="text-sm font-medium text-[#ed214d]">Duvidas frequentes</span>
                    <h2 className="mt-3 font-mono text-3xl font-bold tracking-tight text-foreground sm:text-4xl text-balance">
                        Perguntas que todo dono de fast food faz
                    </h2>
                </div>

                <Accordion type="single" collapsible className="mt-12">
                    {faqs.map((faq, index) => (
                        <AccordionItem key={index} value={`item-${index}`} className="border-border">
                            <AccordionTrigger className="text-left text-sm font-medium text-foreground hover:text-[#ed214d] hover:no-underline">
                                {faq.question}
                            </AccordionTrigger>
                            <AccordionContent className="text-sm leading-relaxed text-muted-foreground">
                                {faq.answer}
                            </AccordionContent>
                        </AccordionItem>
                    ))}
                </Accordion>
            </div>
        </section>
    )
}
