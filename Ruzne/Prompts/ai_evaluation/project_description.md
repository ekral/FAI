# 📘 PROJECT CONTEXT

## Canteen Ordering System (University Project)

This is a semester project for Software Engineering.

The system is a canteen meal ordering platform.

---

## 👥 Roles in the system

### Students

* browse daily menu
* place meal orders
* see order status in real time (SSE)

### Kitchen staff

* view active orders
* update order states:

  * preparing
  * ready
  * cancelled
  * completed

### Administration (canteen management)

* manage meals (create, update, deactivate)
* manage menu (schedule meals per day)

---

## 🍽️ Core business concept

The system manages meal ordering with limited portions.

Rules:

* Each menu item has limited available portions
* Ordering reduces available portions
* Cancelled orders do NOT restore portions
* System must correctly handle concurrency issues

---

## 📡 Real-time behavior (NOT evaluated in midterm)

* SSE broadcasts order updates
* Students and kitchen staff receive updates instantly

---

## 🧠 Key domain rules

* Meals are never deleted, only deactivated
* Valid order transitions:

  * Preparing → Ready → Completed
  * Preparing → Cancelled
* Invalid transitions must be prevented
* Available portions must remain consistent

---

## ⚙️ Technical stack

* .NET 10
* ASP.NET Core Minimal Web API
* Entity Framework Core
* .NET Aspire
* PostgreSQL or other SQL database
* Keycloak authentication
* Blazor clients (NOT evaluated here)

---

# ⚠️ IMPORTANT SCOPE

This is MIDTERM evaluation.

ONLY evaluate:

* Backend (.NET solution)
* WebAPI
* Database layer
* Integration tests
* Aspire configuration

IGNORE:

* UI clients
* frontend UX
* SSE implementation
* authentication/authorization

---

# 🔍 STRICT RULES

Detect and penalize:

* DTO leakage outside Contracts project
* Missing or weak integration tests
* Missing Aspire service discovery
* Hardcoded URLs or ports
* Direct EF entity exposure from API
* Incorrect project structure
* Build/runtime issues
* Missing required features

---

# 🧪 TEST REQUIREMENTS

Integration tests must:

* verify real API behavior
* use real SQL database
* NOT use EF InMemory provider
* validate business behavior and state transitions

---

# 📦 RUBRIC

## Fail conditions (whole project = 0 points)

* project does not build
* project does not run
* source code not in English
* not using .NET 10

## Structure (0–3)

* required projects exist
* correct project references

## Data & DTO (0–5)

* correct EF model
* correct DbContext
* enum for order state
* DTOs only in Contracts
* no entity exposure from API

## API + tests (0–6)

* Meals CRUD + tests
* Menu CRUD + tests
* Orders + state transitions + tests

## Aspire (0–4)

* database provisioning
* reset/seed command
* seed works
* service discovery

## Documentation (0–2)

* README quality

## Penalties

* -1 warning/bug
* -2 architecture/non-functional violation
