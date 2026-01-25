# School Canteen Ordering System – Student Project

## Project Overview

The goal of this project is to design and implement a **web-based school canteen ordering system** with clear separation of responsibilities, multiple user roles, and real-world constraints.

---

## System Architecture

```text
                    ┌─────────────────────────┐
                    │   Meal & Menu Management │
                    │        Website (Admin)   │
                    └─────────────┬───────────┘
                                  │
                                  ▼
                         ┌─────────────────┐
                         │    Menu API     │
                         │ (internal API)  │
                         └────────┬────────┘
                                  │
                                  ▼
                         ┌─────────────────┐
                         │    Menu DB      │
                         │ (Meals, Menus)  │
                         └─────────────────┘


┌─────────────────────────┐        fetch menus / meals
│    Ordering Website     │ ─────────────────────────▶ ┌─────────────────┐
│     (Students)          │                              │    Menu API     │
└─────────────┬───────────┘                              └─────────────────┘
              │
              │  create orders
              ▼
┌──────────────────────────────────────────┐
│               Order API                  │
│        (internal + public endpoints)     │
└───────────────┬──────────────────────────┘
                │
                ▼
         ┌─────────────────┐
         │    Order DB     │
         │ (Orders, Status │
         │  OrderItems)    │
         └─────────────────┘


┌─────────────────────────┐        update order status
│  Order Processing Site  │ ─────────────────────────▶ ┌─────────────────┐
│      (Staff)            │                              │    Order API    │
└─────────────────────────┘                              └─────────────────┘


┌─────────────────────────┐        read order status
│  Public Order Tracking  │ ─────────────────────────▶ ┌─────────────────┐
│        API (Public)     │                              │    Order API    │
└─────────────────────────┘                              └─────────────────┘

```

The system consists of multiple web clients communicating with backend APIs.
All services may share a single database.

```text
[ Admin Client ] ──▶ Menu API ──┐
                               │
[ Student Client ] ─▶ Order API ├─▶ Database
                               │
[ Staff Client ] ───▶ Order API ┤
                               │
[ Public Client ] ──▶ Public API┘
```

---

## User Roles

| Role    | Description |
|--------|-------------|
| Admin  | Manages meals and menus |
| Student| Places food orders |
| Staff  | Processes and updates order status |
| Public | Tracks order status only |

---

## Subsystems

### 1. Meal & Menu Management System (Admin)

**Users:** Canteen management  
**Purpose:** Define what can be sold

**Responsibilities**
- Create, edit, and delete meals
- Set prices and allergen information
- Create daily or weekly menus
- Enable or disable meals

**Restrictions**
- Cannot create or modify orders
- Cannot change order status

---

### 2. Ordering System (Students)

**Users:** Students / teachers  
**Purpose:** Place food orders

**Responsibilities**
- View current menu
- Create orders
- View personal order history
- Enforce ordering cutoff time

**Restrictions**
- Cannot edit meals or menus
- Cannot change order status

---

### 3. Order Processing System (Canteen Staff)

**Users:** Kitchen and serving staff  
**Purpose:** Handle order workflow

**Responsibilities**
- View incoming orders
- Change order status
- View order contents (meals and quantities)

**Restrictions**
- Cannot edit meals or menus
- Cannot see sensitive user data (only name and order number)

---

### 4. Public Order Tracking System

**Users:** Anyone  
**Purpose:** Track order status

**Responsibilities**
- Track order by order number
- Display current order status only

**Restrictions**
- No authentication
- No personal data
- Read-only access

---

## Order Lifecycle

Orders must follow a defined state transition model:

```text
NEW → PREPARING → READY → COMPLETED
           ↘
         CANCELLED
```

**Rules**
- Only staff may change order status
- Invalid transitions must be rejected
- Public users can only view the current status

---

## Core Data Entities

### User
- id
- name
- role (ADMIN, STUDENT, STAFF)

### Meal
- id
- name
- description
- price
- allergens
- active

### Menu
- date
- list of meals

### Order
- id
- user_id
- status
- created_at

### OrderItem
- order_id
- meal_id
- quantity

---

## Technical Requirements

- RESTful API design
- Clear separation of responsibilities
- Role-based access control
- Server-side validation
- Persistent storage (SQL recommended)

---

## Grading Rubric (100 points)

### 1. Architecture & Design (20 points)

| Criteria | Points |
|--------|--------|
| Clear separation of subsystems | 8 |
| Correct responsibility boundaries | 6 |
| Logical data model (ER design) | 6 |

---

### 2. Backend Implementation (25 points)

| Criteria | Points |
|--------|--------|
| Correct REST API structure | 10 |
| Business rules enforced | 8 |
| Error handling and validation | 7 |

---

### 3. Role-Based Access Control (15 points)

| Criteria | Points |
|--------|--------|
| Proper authentication | 5 |
| Authorization rules enforced | 10 |

---

### 4. Order Workflow Logic (15 points)

| Criteria | Points |
|--------|--------|
| Valid status transitions | 10 |
| Protection against invalid updates | 5 |

---

### 5. Frontend Functionality (15 points)

| Criteria | Points |
|--------|--------|
| Admin interface usability | 5 |
| Student ordering interface | 5 |
| Staff order processing interface | 5 |

---

### 6. Code Quality and Documentation (10 points)

| Criteria | Points |
|--------|--------|
| Clean, readable code | 5 |
| README and documentation | 5 |

---

## Bonus Features (Optional)

- Daily order limits
- Meal availability counters
- Live order queue for staff
- Unit tests
- Dockerized deployment

---

## Submission Requirements

- Source code repository
- README with setup instructions
- Database schema
- API documentation (OpenAPI or Markdown)

---

## Learning Outcomes

By completing this project, students will demonstrate:
- Real-world system design skills
- REST API development
- Role-based security implementation
- Workflow and state management
- Full-stack development principles



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
