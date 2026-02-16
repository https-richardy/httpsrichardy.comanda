#### THE IDEA BEHIND COMANDA
Comanda is a SaaS platform designed to manage online orders and digital payments for fast-food establishments. It enables businesses to receive, process, and track customer orders in real time, providing a scalable foundation for modern, digital-first operations.

#### THE ARCHITECTURE BEHIND COMANDA

Comanda is architected around explicit bounded contexts, each one representing an independent system with its own lifecycle and deployment pipeline. Every boundary is deployable on its own, evolving autonomously without leaking concerns across domains.

Rather than a traditional layered monolith, Comanda is structured as a distributed domain model organized within a modular monorepo. Each bounded context fully owns its business rules, infrastructure, tests, and operational workflows, ensuring strict domain isolation and long-term architectural integrity.


---

#### REPOSITORY STRUCTURE

The repository is intentionally structured to reflect Comanda’s architectural boundaries and operational model.

```text
.
├── .github/                # ci/cd workflows (pipelines)
├── Artifacts/              # internal nuget packages (shared kernel/contracts)
├── Boundaries/             # independent bounded contexts
│   ├── Comanda.Stores/
│   └── Comanda.Subscriptions/
├── .editorconfig           # global code style rules
└── nuget.config            # centralized nuget configuration
```

All bounded contexts live in `Boundaries/` directory (e.g., Comanda.Stores, Comanda.Subscriptions).
Each directory represents an independent system with its own solution, source code, tests, Docker configuration, and deployment pipeline. Every boundary builds, evolves, and deploys autonomously.

---

<h4 align="center">THE TECHNOLOGY STACK BEHIND COMANDA</h4>

<div align="center">
  <!-- Stacks -->
  <img src="https://www.vectorlogo.zone/logos/dotnet/dotnet-tile.svg" width="25" alt=".NET" />
  <img src="https://www.vectorlogo.zone/logos/mongodb/mongodb-icon.svg" width="25" alt="MongoDB" />
  <img src="https://www.vectorlogo.zone/logos/docker/docker-icon.svg" width="25" alt="Docker" />

  <!-- DevOps -->
  <img src="https://www.vectorlogo.zone/logos/sentryio/sentryio-ar21.svg" width="60" alt="Sentry" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/githubactions/githubactions-original.svg" width="25" alt="GitHub Actions" />
</div>


<div align="center">

| Layer | Technology | Purpose |
|:-------------:|:------------:|:--------------------------------------------------:|
| Backend | .NET 9 | High-performance APIs and long-term maintainability |
| Persistence | MongoDB | Aggregate-oriented, boundary-aligned persistence |
| Resilience | Polly | Retries, circuit breakers, and explicit QoS policies |
| Observability | Sentry · Seq | Real-time errors, tracing, and structured diagnostics |
| Containerization | Docker | Isolation and reproducible deployments |
| CI/CD | GitHub Actions | Autonomous pipelines per bounded context |

</div>

---

<h4 align="center">EXTERNAL SERVICES BEHIND COMANDA</h4>

#### PIX Transactions

> Comanda integrates with **AbacatePay** to process PIX-based payments, enabling fast and localized transactions for Brazilian merchants.  
This powers the real-time checkout experience used by stores when processing customer orders.

- Website: https://www.abacatepay.com/
- Documentation: https://docs.abacatepay.com/

#### Subscription & Billing Infrastructure

> As a SaaS platform, Comanda relies on **Stripe** to manage the full subscription lifecycle.  
Recurring billing, subscription status, payment retries, and invoicing are orchestrated through Stripe’s infrastructure, ensuring platform monetization remains robust and operationally independent from merchant revenue flows.

- Website: https://stripe.com/
- Documentation: https://docs.stripe.com/
