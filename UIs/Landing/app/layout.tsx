import type { Metadata, Viewport } from 'next'
import { Inter, Space_Grotesk } from 'next/font/google'
import { Analytics } from '@vercel/analytics/next'
import './globals.css'

const _inter = Inter({ subsets: ["latin"], variable: "--font-inter" });
const _spaceGrotesk = Space_Grotesk({ subsets: ["latin"], variable: "--font-space-grotesk" });

export const metadata: Metadata = {
    title: 'Comanda - Gerencie seus pedidos sem taxas e sem complicação',
    description: 'O sistema de gestão de pedidos feito para fast foods. Gerencie pedidos online e presenciais sem taxas de marketplace. Assine por apenas R$100/mes.',
    icons: {
        icon: '/images/comanda-logo.png',
    },
}

export const viewport: Viewport = {
    themeColor: '#ed214d',
}

export default function RootLayout({ children }: Readonly<{ children: React.ReactNode }>) {
    return (
        <html lang="pt-BR">
            <body className={`${_inter.variable} ${_spaceGrotesk.variable} font-sans antialiased`} suppressHydrationWarning>
                {children}
                <Analytics />
            </body>
        </html>
    )
}
