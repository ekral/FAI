# Canteen Ordering System

Semester project for the course **Application Frameworks**.

The goal of the project is to design and implement an ordering system for a canteen
using the tools and frameworks **.NET Aspire, Minimal WebAPI, Entity Framework Core and Blazor**.

---

## 🧠 Project Assignment

The canteen ordering system allows ordering made-to-order meals.
The student orders a meal in a web application running on a touch panel.
The kitchen staff then prepare the meal and change the order status in their web application.
The student is informed about the order status in real time.

---

## Functional Requirements

### Canteen Management

#### Meals
- Displays a list of meals (description, price).
- Creates new meals.
- Edits meals. A meal is not deleted, it is only marked as inactive.

#### Menu
- Displays menu items (date, meal, number of available portions) for all days.
- Creates new menu items.
- Edits menu items.
- Deletes menu items.

### Kitchen Staff

#### Orders
- Displays a list of orders that are not completed.
- Changes the order status to:
  - ready,
  - cancelled,
  - completed (handed to the student or the student informed about the cancellation).

### Student

#### Orders
- Displays the menu for the current day (sold-out meals are visually distinguished).
- Orders a meal from the current menu (the number of available portions is reduced).

### Order Statuses
- Preparing (the number of portions is reduced)
- Ready (prepared for pickup)
- Cancelled (a cancelled order does not return the portion)
- Completed

---

## Non-Functional Requirements

Thanks to the use of [Aspire](https://aspire.dev/get-started/what-is-aspire/)
the teacher must be able to run the entire project locally, including the database and Keycloak. The teacher will have Visual Studio 2026, .NET 10, and Docker installed and running on their computer.

### Solution Requirements

- .NET 10
- The language used in the source code will be **English**. The application language may be different.
- The project uses [**Aspire**](https://aspire.dev/get-started/what-is-aspire/):
  - It creates a database (e.g. [**PostgreSQL**](https://aspire.dev/integrations/databases/efcore/postgres/postgresql-get-started/)).
  - It uses the Identity tool [**Keycloak**](https://aspire.dev/integrations/security/keycloak/) to secure the application.
  - It uses **Service Discovery**, without hardcoded IP addresses.
  - It contains an **Http Command** for database reset (deletion, creation, seed of test data).
- The project uses **Entity Framework for working with the database**.
- The project uses **Minimal Web API** with TypedResults.
- The project uses **DTO (Data Transfer Objects)** independent of entities.
- The code is not duplicated (DTOs are defined in only one place).
- The project uses **Server-Sent Events (SSE)** for server-initiated notifications
  about changes in student orders and for the kitchen staff. SSE notifications about order changes are broadcast to everyone without security.
- Client applications call the Minimal Web API using the HTTP protocol and do not access the database and entities directly.
- Tests will use a "production" database, for example a PostgreSQL server, and not EF InMemory. Tests must run automatically without manual intervention (using a database started through Aspire).

---

## 📂 Solution Structure

The solution will contain the following projects:

- `UTB.Minute.AppHost` - Aspire Integration.
- `UTB.Minute.Db` – entities and `DbContext`.
- `UTB.Minute.DbManager` – WebAPI for Http Command, database reset and seed (reference to `UTB.Minute.Db`).
- `UTB.Minute.Contracts` – DTO (Data Transfer Objects).
- `UTB.Minute.WebApi` – shared WebAPI for all clients including Server-Sent Events (SSE) notifications (reference to `UTB.Minute.Db` and `UTB.Minute.Contracts`).
- `UTB.Minute.WebApi.Tests` - WebAPI test project using the selected database, for example SQL Server (reference to `UTB.Minute.WebApi`).     

---

# 📝 Checklist and Evaluation

This checklist serves:
- for **students** as a checklist before submission
- for **teachers** as unified evaluation criteria

> [!WARNING]
> **Important Rule** 
> If the submitted project **cannot be built or run**,
> the source code **is not in English** or **is not created in .NET 10**  
> it will be graded with **0 points**
> (regardless of the amount of implemented functionality).

---

## 📤 Midterm Submission (20 points)

Students submit only the **backend and WebAPI**  
*(without client applications, without Keycloak integration, and without SSE)*

---

### Projects and Solution Structure (0–3 points)
- [ ] All required projects exist and are correctly named (2 points)  
  (`UTB.Minute.Db`, `DbManager`, `Contracts`, `WebApi`, `WebApi.Tests`)
- [ ] Correct references between projects (1 point)

---

### Data Model and DTO (0–5 points)
- [ ] Entities and their relationships match the assignment (1 point)
- [ ] Properly designed `DbContext` (1 point)
- [ ] Order status handled by an enum (1 point)
- [ ] DTOs are defined only in `UTB.Minute.Contracts` (1 point)
- [ ] WebAPI does not return entities directly (1 point)

---

### WebAPI Functionality and Its Tests (0–6 points)

#### Meals (0–2 points)
- [ ] Creation and reading of meals and their tests (1 point)
- [ ] Meal editing + deactivation and their tests (1 point)

#### Menu (0–2 points)
- [ ] Creation and reading of menu items and their tests (1 point)
- [ ] Editing and deletion of menu items and their tests (1 point)

#### Orders (0–2 points)
- [ ] Creation and reading of orders and their tests (1 point)
- [ ] Changing order status and its test (1 point)

---

### Aspire Integration (0–4 points)
- [ ] Database created and configured through Aspire (1 point)
- [ ] Http Command for database reset (1 point)
- [ ] Test data seed works (1 point)
- [ ] Service Discovery without hardcoded addresses (1 point)

---

### Documentation (0 (not present) or 2 points (brief README.md))
- [ ] Brief project documentation (README.md) (2 points)

---

### Penalty Points (negative)
- [ ] Bugs, warnings (-1 point for each). Ignore nuget related package warnings. Same issue in multiple issues is counted only once.
- [ ] Failure to follow non-functional requirements, naming conventions (-2 points for each).

---

✅ **Total: 20 points**

---

# 🧪 TEST REQUIREMENTS

Integration tests must:

* verify real API behavior
* use real SQL database
* NOT use EF InMemory provider
* cover implemented functionality only; they are not required to test behavior that is not implemented
* validate business behavior (cover edge cases and error conditions for implemented features), not just "happy path" 
---