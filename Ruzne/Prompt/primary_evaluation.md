You are evaluating a university semester project in Software Engineering.

# 📘 PROJECT CONTEXT

## Canteen Ordering System (University Project)

This is a semester project for Software Engineering.

The system is a **canteen meal ordering platform**.

---

## 👥 Roles in the system

### Students

* browse daily menu
* place meal orders
* see order status in real time (SSE)

### Kitchen staff

* view active orders
* update order states (preparing, ready, cancelled, completed)

### Administration (canteen management)

* manage meals (create, update, deactivate)
* manage menu (schedule meals per day)

---

## 🍽️ Core business concept

The system manages **meal ordering with limited portions**:

* Each menu item has a limited number of available portions
* Ordering a meal reduces available portions
* Cancelled orders do NOT restore portions
* System must handle concurrency (last portion problem)

---

## 📡 Real-time behavior (SSE)

* Order state changes must be broadcast to clients
* Students and kitchen staff receive updates instantly
* No authentication required for SSE broadcast channel

---

## 🧠 Key domain rules

* Meals are never deleted, only deactivated
* Orders follow strict state transitions:

  * Preparing → Ready → Completed
  * Preparing → Cancelled
* Invalid state transitions must be prevented
* System must ensure consistency of available portions

---

## ⚙️ Technical stack

* .NET 10
* ASP.NET Core Minimal Web API
* Entity Framework Core
* .NET Aspire (infrastructure orchestration)
* PostgreSQL
* Keycloak authentication
* Blazor clients (NOT evaluated in this task)

---

## System purpose

Students order meals from a canteen system.
Kitchen staff manage orders.
Students receive real-time updates via SSE.

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

* UI clients (Blazor apps)
* UX design
* frontend behavior
* SSE
* Authentication/authorization (Keycloak)

---

# 🎯 TASK

Evaluate strictly according to rubric.

Do NOT guess missing functionality.
Use ONLY evidence from code/tests.

---

# 📊 SCORING RULE

Use POINT-BASED evaluation ONLY.

For each rubric item:

* Max points
* Suggested points
* Evidence (files/classes/tests)
* Reason for deductions
* Confidence (High/Medium/Low)

---

# 🔍 STRICT RULES

Detect and penalize:

* DTO leakage outside Contracts project
* Missing or weak integration tests
* Missing Aspire service discovery usage
* Hardcoded URLs or ports
* Direct EF entities returned from API
* Incorrect project structure
* Build/runtime issues
* Missing required features

---

# 🧪 TEST REQUIREMENT

Integration tests must:

* verify real API behavior
* use SQL database (not InMemory)
* validate state changes, not only endpoints

---

# 📦 RUBRIC (SUMMARY)

## Fail conditions (whole project 0 points)

* does not build
* does not run
* not .NET 10
* not English source code

## Structure (3)

* required projects exist
* correct references

## Data & DTO (5)

* EF model correct
* DbContext correct
* enum for state
* DTOs only in Contracts
* no entity exposure

## API + tests (6)

* Meals CRUD + tests
* Menu CRUD + tests
* Orders + state transitions + tests

## Aspire (4)

* DB provisioning
* reset/seed command
* seed works
* service discovery

## Documentation (2)

* README quality

## Penalties

* -1 warning/bug
* -2 architecture/non-functional violation

---

# 📄 OUTPUT FORMAT

## Per rubric item:

(max points, suggested points, evidence, reasoning, confidence)

---

## FINAL SUMMARY

* Total score (0–20)
* Top 5 issues
* Manual review risks
* Warnings/errors
* Project quality (1–5)

---

# 📊 FINAL EXCEL OUTPUT (MANDATORY)

ONE LINE ONLY:

Project;TotalPoints;MaxPoints;Percentage;MainIssues;AIConfidence;ManualAdjustmentNeeded

Rules:

* MaxPoints = 20
* Percentage = TotalPoints / 20 * 100
* MainIssues max 5 keywords
* AIConfidence = High/Medium/Low
* ManualAdjustmentNeeded = Yes/No

Example:
Team01;17.5;20;87.5;DTO violation, weak tests;High;Yes
