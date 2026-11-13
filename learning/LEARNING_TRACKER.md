---

### File 2: `learning/LEARNING_TRACKER.md`

Adapted from your tracker structure[cite: 1] and pre-loaded with your exact `UserManagementApi` milestones, quiz results, and session history:

```markdown
# UserManagementApi — Learning & System Architecture Mastery Tracker

This document tracks progress, covered topics, proficiency levels, and the structured 6-phase roadmap for mastering **`UserManagementApi`** (.NET + PostgreSQL) and progressing toward becoming a **Backend Developer** and **System Architect**.

---

## Part 1: Foundation & Vertical Slice Baseline (Completed)

**Goal:** Build a working end-to-end vertical slice connecting an ASP.NET Core Minimal API to a local PostgreSQL database, refactor into clean architectural layers, and establish version control and secret management.

| Block | Topic | Status | What Was Accomplished |
| :--- | :--- | :--- | :--- |
| **Block 1** | **PostgreSQL & Minimal API Setup** | ✅ Completed | Created `app_users` table in PostgreSQL (`dev_db`), configured `Npgsql`, and built initial `POST /api/users` and `GET /api/users/{id}` endpoints. |
| **Block 2** | **Clean Architecture & Connection Pooling** | ✅ Completed | Refactored monolithic `Program.cs` into `Models/`, `DTOs/`, `Repositories/`, and `Services/`; fixed connection pool exhaustion by injecting `NpgsqlDataSource` as a `Singleton`. |
| **Block 3** | **Full Read/Delete & SQL Debugging** | ✅ Completed | Implemented `GET /api/users` and `DELETE /api/users/{id:guid}`; diagnosed and fixed `Npgsql.PostgresException (42703)` column typo (`create_at` -> `created_at`). |
| **Block 4** | **Secret Management & Git/GitHub Workflow** | ✅ Completed | Secured database credentials using `dotnet user-secrets`, added `.gitignore`, initialized local Git repository, and pushed baseline to GitHub (`main`). |
| **Block 5** | **Baseline Recall Assessment** | ✅ Completed | Scored 8/10 on comprehensive recall quiz covering connection pooling, DI lifetimes, `Result<T>`, data readers, user secrets, and Git. |

---

## Part 2: Long-Term Backend & System Architect Roadmap

- **Phase 1 — C#, SQL & Minimal API Vertical Slice:** ✅ **Completed** — Minimal API routing, parameterized SQL, `NpgsqlDataReader`, `POST` & `GET by ID`.
- **Phase 2 — Clean Architecture, DI Lifetimes & Full CRUD:** 🔄 **In Progress** — Layered structure, `NpgsqlDataSource` pooling, `Result<T>` pattern, `GET all` & `DELETE` completed. **Next:** `UserResponse` DTOs, `ErrorType` mapping, and `PUT /api/users/{id:guid}`.
- **Phase 3 — Production API Hardening & Data Integrity:** ⏳ **Pending** — Database `UNIQUE` constraints & concurrency race conditions, global exception handling (`ProblemDetails`), pagination/filtering, and PostgreSQL indexing (`EXPLAIN ANALYZE`).
- **Phase 4 — Security, Authentication & Authorization:** ⏳ **Pending** — Password hashing (BCrypt/Argon2), login endpoint, JWT authentication, role-based authorization, and `CancellationToken` propagation.
- **Phase 5 — Automated Testing, Logging & Docker:** ⏳ **Pending** — Structured logging (`ILogger`), unit tests (`xUnit`), integration tests against real Postgres (`Testcontainers`), and Docker Compose setup.
- **Phase 6 — System Architecture & Distributed Systems:** ⏳ **Pending** — Redis caching, background workers & message queues, Outbox pattern, read replicas & PgBouncer, rate limiting, and system design trade-offs.

---

## Part 3: Dual Progress Tracking

### Proficiency Standards (per `command.md`):
- **Level 1 — Introduced:** Concept explained and demonstrated in code.
- **Level 2 — Understood:** Can explain the concept accurately, reason about trade-offs, and trace execution.
- **Level 3 — Independently Applied (Mastered):** Can implement, debug, or apply the concept independently without step-by-step guidance.

---

### Track A: General Backend Engineering & System Architecture

| Concept / Skill | Level | Notes / Evidence |
| :--- | :--- | :--- |
| RESTful HTTP Methods & Status Codes (`200`, `201`, `204`, `400`, `404`) | Level 2 (Understood) | Correctly maps resource creation (`201 Created`) vs. deletion (`204 No Content`). |
| Parameterized SQL Queries (`@param`) | Level 2 (Understood) | Uses parameterized commands across all repository methods to prevent SQL injection. |
| ADO.NET / Npgsql Execution (`ExecuteScalarAsync`, `ExecuteNonQueryAsync`, `ExecuteReaderAsync`) | Level 2 (Understood) | Clarified distinction between scalar counts, row-modifying commands, and forward-only row streams. |
| Database Connection Pooling (`NpgsqlDataSource`) | Level 2 (Understood) | Understands why creating a DataSource per request exhausts pools and why `Singleton` registration solves it. |
| Interfaces & Dependency Inversion | Level 2 (Understood) | Understands how `UserService` consumes `IUserRepository` without instantiating `PostgresUserRepository`. |
| ASP.NET Core DI Lifetimes (`Transient`, `Scoped`, `Singleton`) & Captive Dependencies | Level 2 (Understood) | Identified captive dependency bug when a `Singleton` holds a `Scoped` repository. |
| Result Pattern (`Result<T>`) vs. HTTP Response Factory (`Results`) | Level 2 (Understood) | Distinguishes domain/service outcome wrappers from ASP.NET Core HTTP response builders. |
| Route Constraints (`{id:guid}`) | Level 2 (Understood) | Reviewed how non-GUID segments fail route matching and return `404 Not Found` before handler execution. |
| Secret Management (`dotnet user-secrets`) & Git/GitHub | Level 3 (Independently Applied) | Removed plaintext password from `appsettings.json`, configured user secrets, initialized Git, and pushed to GitHub. |
| Request & Response DTO Separation | Level 1 (Introduced) | `RegisterUserRequest` implemented; `UserResponse` and `UpdateUserRequest` ready for implementation. |

---

### Track B: `UserManagementApi` Project Mastery

| Component / File / Flow | Level | Notes / Evidence |
| :--- | :--- | :--- |
| PostgreSQL `app_users` Table & Schema | Level 2 (Understood) | Verified columns (`id`, `username`, `email`, `age`, `created_at`) and diagnosed SQL column mismatch (`42703`). |
| Project Folder Architecture (`Models/`, `DTOs/`, `Repositories/`, `Services/`, `Program.cs`) | Level 2 (Understood) | Successfully refactored single-file prototype into separated architectural layers. |
| `Models/User.cs` & `Models/Result.cs` | Level 2 (Understood) | Domain entity and generic result wrapper in place; next step adds `User.Update()` and `ErrorType`. |
| `Repositories/IUserRepository.cs` & `PostgresUserRepository.cs` | Level 2 (Understood) | Implements `AddAsync`, `EmailExistsAsync`, `GetByIdAsync`, `GetAllAsync`, and `DeleteAsync`. |
| `Services/UserService.cs` | Level 2 (Understood) | Encapsulates input validation and business rules decoupled from HTTP and SQL. |
| `POST /api/users`, `GET /api/users`, `GET /api/users/{id:guid}`, `DELETE /api/users/{id:guid}` | Level 3 (Independently Applied) | Built, debugged, and verified end-to-end via Swagger UI and pgAdmin 4. |
| `DTOs/UserResponse.cs` & `PUT /api/users/{id:guid}` | Level 1 (Introduced) | Architecture and code walkthrough provided; awaiting implementation and testing. |

---

## Part 4: Session Activity Log

* **Session 1 (Initial Setup & Vertical Slice):**
  - Created `UserManagementApi` Web API project and PostgreSQL `app_users` table.
  - Built and tested `POST /api/users` (Create) and `GET /api/users/{id:guid}` (Read by ID) using `Npgsql`.
* **Session 2 (Architectural Refactoring & Connection Pooling):**
  - Separated codebase into `Models/`, `DTOs/`, `Repositories/`, `Services/`, and `Program.cs`.
  - Registered `NpgsqlDataSource` as a `Singleton` and `IUserRepository`/`UserService` as `Scoped`.
  - Clarified interface consumption vs. implementation via Constructor Injection.
* **Session 3 (Full Read/Delete, Debugging & Git/GitHub):**
  - Implemented `GET /api/users` (`while` loop over `NpgsqlDataReader`) and `DELETE /api/users/{id:guid}` (`204 No Content`).
  - Fixed `Result<bool>` syntax/return-type compiler errors and resolved PostgreSQL `42703` (`create_at` typo) in `GetByIdAsync`.
  - Moved connection string to `dotnet user-secrets`, generated `.gitignore`, initialized Git, and pushed `main` branch to GitHub.
* **Session 4 (2026-11-13 — Recall Quiz & Mentorship Setup):**
  - Completed 10-question architecture & Postgres recall quiz (8/10).
  - Created customized `learning/command.md` and `learning/LEARNING_TRACKER.md` for VS Code Copilot mentorship.
  - **Active Next Step:** Implement `DTOs/UserResponse.cs`, `DTOs/UpdateUserRequest.cs`, `ErrorType` in `Result<T>`, and `PUT /api/users/{id:guid}`.