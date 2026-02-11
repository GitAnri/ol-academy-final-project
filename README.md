Individuals Management API - ASP.NET Core Web API:

This is an ASP.NET Core Web API for managing individuals and their related data. It started as a CRUD system for Individuals, Cities, Phone Numbers, and Relations using Entity Framework Core and SQL Server, with validation, DTO mapping, and business-logic services layered over repository-pattern data access.

Later, I extended the project with a secure authentication system using JWT tokens, salted SHA-256 password hashing, user roles (Admin/User), and custom middleware that verifies the user and role on every authenticated request, preventing forged tokens or access from deleted users. Swagger is configured with JWT support to allow secure testing of protected endpoints.

What I learned:

- ASP.NET Core Web API + Entity Framework Core + SQL Server

- Repository + Service architecture

- CRUD for Individuals, Cities, Phone Numbers, Relations

- DTO-based API contracts

- Validation & business rules

- JWT authentication & role-based authorization

- Salted SHA-256 password hashing

- Middleware verifying token user & role still exist

- Admin-only policy support

- Swagger UI with JWT support
