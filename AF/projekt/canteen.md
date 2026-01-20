
# MinuteMeal – Project Structure & Dependencies

MinuteMeal is the UTB system for made-to-order meals

## 1️⃣ Solution Structure

```text
Canteen.Ordering.sln
│
├── UTB.MinuteMeal.Domain
├── UTB.MinuteMeal.Persistence
├── UTB.MinuteMeal.Infrastructure
├── UTB.MinuteMeal.Contracts
├── UTB.MinuteMeal.Seeder
├── UTB.MinuteMeal.Api          ← Minimal APIs
└── UTB.MinuteMeal.BlazorClient ← Blazor WASM
```
---

## 2️⃣ Project Responsibilities

🧠 Domain

Purpose: Business logic
Contains:
- Entities (EF-mapped, no EF Core references)
- Value Objects
- Domain services
- Repository interfaces
- Business rules & invariants
References: None

🗄️ Persistence

Purpose: Database layer & EF Core
Contains:
- DbContext
- EF Core configurations
- Migrations
- Repository implementations
References: Domain

🔌 Infrastructure

Purpose: External services and integrations
Contains:
- Email services
- Payments
- File storage
- Message bus
- Background jobs
References: Domain

📦 Contracts

Purpose: Shared API contracts
Contains:
- DTOs
- Request / Response models
References: None

🌱 Seeder

Purpose: Seed database (Console app)
Contains: Initial data logic
References: Domain, Persistence

🌐 API (Minimal API)

Purpose: Application orchestration layer
Contains:
- Minimal API endpoints
- Authentication (JWT)
- Validation
- Dependency injection setup
References: Domain, Persistence, Infrastructure, Contracts

🖥️ Blazor Client

Purpose: User interface (WASM)
Contains:
- Razor components
- API client services
- UI state management
References: Contracts

---

## 3️⃣ Dependency Rules

Project          | References
---------------- | -----------------------------------------------
Domain           | (none)
Persistence      | Domain
Infrastructure   | Domain
Contracts        | (none)
Seeder           | Domain, Persistence
API              | Domain, Persistence, Infrastructure, Contracts
Blazor Client    | Contracts

Forbidden Dependencies:
- Domain → EF / API / Infrastructure
- Persistence → API / Blazor Client
- Infrastructure → API / Blazor Client
- Seeder → API / Blazor Client
- Blazor Client → Persistence / Domain

---

## 4️⃣ Visual Dependency Diagram

```text
┌──────────────────────────────────────┐
│   UTB.MinuteMeal.BlazorClient        │
│   (Blazor WASM)                      │
└──────────────▲───────────────────────┘
               │ HTTP
┌──────────────┴───────────────────────┐
│        UTB.MinuteMeal.Api            │
│        (Minimal APIs)                │
└───────▲──────────────▲───────────────┘
        │              │
┌───────┴───────┐ ┌────┴────────────────┐
│ Persistence   │ │ Infrastructure      │
│ EF Core       │ │ External Services   │
└───────▲───────┘ └──────────▲──────────┘
        │                     │
        └──────────────┬──────┘
                       │
             ┌─────────┴─────────┐
             │      Domain        │
             └───────────────────┘

       ┌──────────────────────────┐
       │         Seeder           │
       │ (Domain + Persistence)   │
       └──────────────────────────┘

       ┌──────────────────────────┐
       │        Contracts         │
       │   Shared DTOs            │
       └────▲──────────▲──────────┘
            │          │
     BlazorClient      Api
```
---

## 5️⃣ Summary

- Domain = Pure business logic, EF entities allowed but no EF dependency
- Persistence = Database layer with EF Core
- Infrastructure = External integrations (Email, Payments, etc.)
- Contracts = Shared DTOs for API and Blazor communication
- API = Minimal API orchestration layer
- Blazor Client = UI, communicates with API only
- Seeder = Initializes database, uses Domain + Persistence

> This architecture is clear, scalable, and ensures EF and external systems do not leak into Domain or UI.
