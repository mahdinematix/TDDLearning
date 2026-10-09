[TDDLearning_README.md](https://github.com/user-attachments/files/33242500/TDDLearning_README.md)
# TDDLearning

A learning-focused ASP.NET Core Web API that I built in 2025 to improve my practical understanding of **Test-Driven Development (TDD)** and automated testing in .NET. I kept the business domain deliberately small so I could spend more time exploring how to test different layers of an application, choose the right test boundary, and get useful feedback from tests while developing.

The sample application is an **Academy Course Management API**. It supports creating, listing, retrieving, editing, and deleting courses. A course contains information such as its name, instructor, tuition, online/offline format, and sections. The domain model also protects a couple of basic invariants, including a non-empty course name and positive tuition, while the application layer handles use-case rules such as duplicate course names and attempts to edit courses that do not exist.

## What I wanted to learn

- Practicing a test-first workflow and the **Red–Green–Refactor** cycle.
- Writing focused unit tests for domain rules, application behavior, and controller behavior.
- Using integration tests to verify persistence and HTTP/API behavior across real application boundaries.
- Exploring BDD-style acceptance testing for higher-level scenarios.
- Using test doubles to isolate a unit under test, while recognizing when a test needs real infrastructure.
- Keeping the test suite organized by architectural layer and test purpose.

## Solution structure

The solution separates the application into four main projects:

```text
src/
├── Academy.Domain/          # Course and Section models, domain rules, and domain exceptions
├── Academy.Application/     # Course use cases, application contract, and request models
├── Academy.Infrastructure/  # EF Core persistence, SQL Server context, and repository implementation
└── Academy.Presentation/    # ASP.NET Core Web API controllers and Swagger/OpenAPI

tests/
├── Academy.Domain.Tests/                         # Domain unit tests
├── Academy.Application.Tests.Unit/               # Application-layer unit tests
├── Academy.Domain.Infrastructure.Tests.Unit/     # Repository-focused tests
├── Academy.Infrastructure.Tests.Integration/    # Database-backed integration tests
├── Academy.Presentation.Tests.Unit/              # Controller unit tests
├── Academy.Presentation.Tests.Integration/       # HTTP/API integration tests
└── Academy.Acceptance.Tests/                     # Higher-level acceptance scenarios
```

### Application overview

- **Domain:** `Course` and `Section` represent the core model. The `Course` constructor validates its name and tuition, and domain-specific exceptions make invalid states explicit.
- **Application:** `CourseApplication` coordinates course-management use cases through the `ICourseRepository` abstraction. It includes checks for duplicate course names and missing courses during editing.
- **Infrastructure:** `AcademyContext` and `CourseRepository` implement persistence using Entity Framework Core and SQL Server.
- **Presentation:** `CourseController` exposes the course operations through HTTP endpoints. Swagger/OpenAPI is configured to make the API easier to explore during development.

## Testing strategy

## API endpoints

The `CourseController` exposes the following basic course-management endpoints under `api/Course`:

| Method | Route | Purpose |
|---|---|---|
| `GET` | `/api/Course` | Retrieve all courses |
| `GET` | `/api/Course/{id}` | Retrieve a course by ID |
| `POST` | `/api/Course` | Create a course |
| `PUT` | `/api/Course` | Edit a course |
| `DELETE` | `/api/Course/{id}` | Delete a course by ID |

The repository contains separate test projects for several layers and levels of testing:

| Test project | Focus | Main tools / approach |
|---|---|---|
| `Academy.Domain.Tests` | Domain behavior and validation rules | xUnit, FluentAssertions |
| `Academy.Application.Tests.Unit` | Use-case behavior, repository interactions, duplicate-name and missing-course cases | xUnit, NSubstitute, FluentAssertions |
| `Academy.Presentation.Tests.Unit` | Controller behavior and delegation to the application layer | xUnit, NSubstitute, FluentAssertions |
| `Academy.Domain.Infrastructure.Tests.Unit` | Repository behavior | xUnit and a database-backed fixture in the current implementation |
| `Academy.Infrastructure.Tests.Integration` | Persistence behavior against SQL Server | xUnit, EF Core, FluentAssertions |
| `Academy.Presentation.Tests.Integration` | HTTP requests against an in-process API host | `WebApplicationFactory`, RESTFulSense, xUnit |
| `Academy.Acceptance.Tests` | Higher-level acceptance scenarios with a BDD-style approach | TestStack.BDDfy, RestSharp, xUnit, FluentAssertions |

For isolated unit tests, **NSubstitute** helps replace dependencies such as `ICourseRepository`, while **FluentAssertions** keeps assertions readable. The integration tests use `WebApplicationFactory` and RESTFulSense to exercise the API through HTTP, rather than calling controller methods directly. TestStack.BDDfy and RestSharp are included in the acceptance-test project to support higher-level, BDD-style scenarios.

The repository also includes `coverlet.collector` in its test projects for test-coverage collection.

> **Testing note:** Although the repository-focused project is named `Academy.Infrastructure.Tests.Unit`, its current repository tests use `RealDatabaseFixture` and a real SQL Server connection. Those tests are therefore database-backed rather than fully isolated unit tests. This is an important distinction when deciding what each test should prove and what infrastructure it needs.

## Tech stack

- **Language & framework:** C#, .NET 10, ASP.NET Core Web API
- **Persistence:** Entity Framework Core, SQL Server
- **API documentation:** Swagger / OpenAPI
- **Testing:** xUnit, FluentAssertions, NSubstitute, TestStack.BDDfy, RESTFulSense, RestSharp, `Microsoft.AspNetCore.Mvc.Testing`, Coverlet Collector

## Getting started

### Prerequisites

- .NET 10 SDK
- SQL Server for database-backed tests and local API execution
- A configured `TddAcademy` database with the tables expected by the application and test fixture

### Run the API

From the repository root:

```bash
dotnet restore TDDLearning.slnx
dotnet run --project src/Academy.Presentation/Academy.Presentation.csproj
```

In the Development environment, open the Swagger UI at `/swagger` on the local URL printed by the application.

### Run the tests

```bash
dotnet test TDDLearning.slnx
```

The database-backed test projects require a working local SQL Server configuration and the expected test database/schema. They will not run successfully on a machine where those prerequisites are missing.

## Configuration and security

Before running the project, replace the hard-coded database connection strings currently present in [`Program.cs`](src/Academy.Presentation/Program.cs) and [`RealDatabaseFixture.cs`](tests/Academy.Infrastructure.Tests.Integration/ClassFixtures/RealDatabaseFixture.cs) with environment-specific configuration or user secrets. **Do not commit database passwords or other credentials to source control.**

## Scope

This is a learning project, not a production-ready academy platform. Its purpose is to practice test design and understand the differences between unit, integration, and acceptance tests while working with a small, understandable domain.
