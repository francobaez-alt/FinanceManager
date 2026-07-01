# FinanceManager
Personal finance management REST API built with ASP.NET Core, Clean Architecture, Entity Framework Core, SQL Server, JWT authentication, and RESTful best practices.
ework Core
- SQL Server
- JWT Authentication
- AutoMapper
- FluentValidation
- Clean Architecture
- Repository Pattern
- Dependency Injection
- Global Exception Middleware
- Swagger / OpenAPI

---

# 📊 Modelo de Base de Datos

<p align="center">
    <img src="Diagrama%20Base%20de%20datos.png" alt="Database Diagram" width="100%">
</p>

El sistema está compuesto por 10 entidades principales que permiten administrar usuarios, wallets, categorías, presupuestos, transacciones e historial de cambios.

## Entidades

- Users
- Roles
- Permissions
- RolePermissions
- Wallets
- Categories
- Transactions
- ScheduledTransactions
- TransactionHistory
- Budgets

---

# 🏛 Arquitectura

El proyecto sigue una implementación de **Clean Architecture**.

```
FinanceManager
│
├── Api
├── Application
├── Domain
├── Infrastructure
└── Shared
```

### Domain

Contiene:

- Entidades
- Interfaces
- Enums
- Constantes

No depende de ningún otro proyecto.

---

### Application

Contiene toda la lógica de negocio.

- Services
- DTOs
- Validators
- Mapping Profiles
- Interfaces
- Exceptions

---

### Infrastructure

Implementación de acceso a datos.

- Entity Framework Core
- Repositories
- Configurations
- DbContext
- JWT
- Password Hasher

---

### Api

Punto de entrada de la aplicación.

- Controllers
- Middleware
- Dependency Injection
- Swagger
- Authentication

---

# 🔑 Funcionalidades

- Registro de usuarios
- Login con JWT
- Gestión de Roles y Permisos
- CRUD de Wallets
- CRUD de Categorías
- Registro de ingresos y gastos
- Historial de modificaciones
- Presupuestos por categoría
- Transacciones programadas
- Soft Delete
- Validaciones
- Manejo global de excepciones

---

# 📁 Estructura del proyecto

```
FinanceManager.sln

src
│
├── Api
├── Application
├── Domain
├── Infrastructure

tests
│
└── FinanceManager.Tests
```

---

# 🔒 Seguridad

- JWT Bearer Authentication
- Password Hashing
- Authorization basada en Roles
- Validaciones mediante FluentValidation
- Middleware global de manejo de errores

---

