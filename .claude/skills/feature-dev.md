# Feature Development Skill

Use this skill when implementing any new feature or ticket.

## 1. Clean Architecture

Separate concerns across layers — never mix business logic with presentation or infrastructure.

**Backend (ASP.NET Core):**
```
Domain/          → entities, value objects, domain interfaces (no dependencies)
Application/     → use cases, commands/queries (CQRS), DTOs, service interfaces
Infrastructure/  → EF Core, external APIs, implementation of service interfaces
API/Controllers/ → thin HTTP adapters; delegate immediately to Application layer
```
- Controllers must not contain business logic — only map HTTP input to commands/queries and return results.
- Domain layer must not reference any other layer or framework.
- Application layer depends only on Domain; never on Infrastructure directly (use interfaces).

**Frontend (Angular):**
```
feature/
  components/    → presentational only; emit events, no direct API calls
  services/      → all HTTP calls and state management live here
  models/        → interfaces/types for the feature
  store/         → state if needed (signals or NgRx)
```
- Components receive data via `@Input()` and communicate up via `@Output()`.
- Services are the only place that call `HttpClient`.

## 2. Tests

**Write tests for every feature.** Decide the type based on what is being tested:

| What | Test type | Tool |
|---|---|---|
| Domain logic, use cases, pure functions | Unit | Vitest (FE) / xUnit (BE) |
| Angular components | Unit (TestBed) | Vitest + Angular Testing |
| API endpoints end-to-end | Integration | xUnit + WebApplicationFactory |
| DB queries / EF Core mappings | Integration | xUnit + in-memory or real DB |

**Rules:**
- Unit tests: mock all dependencies (interfaces, services).
- Integration tests: use real infrastructure (no mocks for the layer under test).
- Every public method / use case must have at least one happy-path and one failure-path test.
- Place test files next to the code they test (`*.spec.ts` for FE, `*.Tests` project for BE).

Run before pushing:
```bash
# Frontend
cd elshop.client && npm test

# Backend (once test project exists)
cd ElShop.Server && dotnet test
```

## 3. Commit & PR format

Always include the ticket/task ID in commits and the PR title.

**Commit message format:**
```
[TICKET-123] Short imperative description

- Bullet with key implementation detail if non-obvious
- Another bullet if needed
```

**Branch naming:**
```
TICKET-123-short-description
```

**PR title:**
```
[TICKET-123] Short description of what this PR does
```

**PR body must include:**
- What the ticket required
- Architectural decisions made (if any)
- How to test it manually
