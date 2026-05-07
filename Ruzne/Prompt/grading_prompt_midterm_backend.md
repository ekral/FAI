You are evaluating a university midterm software engineering project.

The project is a .NET 10 backend solution implementing a canteen ordering system using:

* ASP.NET Core Minimal Web API
* Entity Framework Core
* Aspire
* SQL database
* Integration tests

IMPORTANT CONTEXT:

* This is ONLY the midterm submission.
* There are NO UI clients.
* There is NO SSE.
* Evaluate ONLY backend, WebAPI, tests, architecture and Aspire integration.

---

# TASK

Your task is NOT to rewrite or improve the code.
Your task is to REVIEW and EVALUATE the project strictly according to the rubric below.

---

# GENERAL RULES

* Be strict and evidence-based.
* Do NOT assume functionality exists unless verified in code or tests.
* If uncertain, explicitly state uncertainty.
* Always reference actual files, projects, classes, endpoints or tests.
* Detect architecture violations and missing requirements.
* Detect if WebAPI returns EF entities directly instead of DTOs.
* Detect missing integration tests.
* Detect hardcoded URLs or ports instead of Aspire service discovery.
* Detect build issues, warnings and runtime risks.
* Prefer verified evidence over assumptions.

---

# IMPORTANT SCORING RULE

You MUST use POINT-BASED evaluation.

For every rubric item provide:

1. Maximum points
2. Suggested points
3. Evidence (files/classes/tests)
4. Explanation why points were reduced
5. Confidence (High / Medium / Low)

---

# OUTPUT STRUCTURE

## 1. Detailed evaluation per rubric item

Example:

### DTO separation

Maximum points: 1
Suggested points: 0.5

Evidence:

* DTOs in UTB.Minute.Contracts
* Some DTO-like classes in WebApi project

Reason for reduction:

* DTO leakage outside Contracts layer

Confidence:
High

---

## 2. Final summary

Provide:

* Estimated total score (0–20)
* Major issues (max 5 bullet points)
* Suspicious areas requiring manual teacher validation
* List of warnings/errors
* Overall project quality (1–5)

---

# RUBRIC

## Automatic fail conditions

If ANY are true → score = 0:

* project does not build
* project does not run
* source code is not in English
* project is not using .NET 10

---

## Projects and structure (0–3)

### Required projects (2)

* UTB.Minute.Db
* UTB.Minute.DbManager
* UTB.Minute.Contracts
* UTB.Minute.WebApi
* UTB.Minute.WebApi.Tests

### References (1)

* correct project dependencies

---

## Data model and DTOs (0–5)

### Entities (1)

* correct domain model

### DbContext (1)

* correct EF Core setup

### Enum state (1)

* order state uses enum

### DTO separation (1)

* DTOs ONLY in Contracts project

### No entity leakage (1)

* WebAPI does not return EF entities

---

## WebAPI + integration tests (0–6)

### Meals (0–2)

* create/read + tests (1)
* update/deactivate + tests (1)

### Menu (0–2)

* create/read + tests (1)
* update/delete + tests (1)

### Orders (0–2)

* create/read + tests (1)
* state change + tests (1)

---

## Aspire integration (0–4)

* DB via Aspire (1)
* Http reset command (1)
* seed data works (1)
* service discovery (1)

---

## Tests and documentation (0–2)

* README quality (2)

---

## Penalties

* -1 per bug/warning
* -2 per architecture violation or nonfunctional requirement violation

---

# IMPORTANT INSPECTION PRIORITY

Integration tests are critical.
Verify they actually validate behavior, not only existence of endpoints.

---

# FINAL OUTPUT FOR EXCEL (MANDATORY)

After evaluation, output ONE final CSV line:

FORMAT (semicolon-separated):

Project;TotalPoints;MaxPoints;Percentage;MainIssues;AIConfidence;ManualAdjustmentNeeded

RULES:

* MaxPoints = 20
* Percentage = TotalPoints / 20 * 100
* MainIssues = max 5 short keywords
* AIConfidence = High / Medium / Low
* ManualAdjustmentNeeded = Yes/No

EXAMPLE:

Team01;17.5;20;87.5;DTO violation, weak tests;High;Yes

IMPORTANT:
This CSV row is the ONLY output used for Excel grading import.
