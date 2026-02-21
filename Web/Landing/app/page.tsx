import { Navbar } from "@/components/landing/navbar"
import { Hero } from "@/components/landing/hero"
import { StatsBar } from "@/components/landing/stats-bar"
import { Features } from "@/components/landing/features"
import { Pricing } from "@/components/landing/pricing"
import { Testimonials } from "@/components/landing/testimonials"
import { FAQ } from "@/components/landing/faq"
import { FinalCTA } from "@/components/landing/final-cta"
import { Footer } from "@/components/landing/footer"

export default function LandingPage() {
  return (
    <main>
      <Navbar />
      <Hero />
      <StatsBar />
      <Features />
      <Pricing />
      <Testimonials />
      <FAQ />
      <FinalCTA />
      <Footer />
    </main>
  )
}
