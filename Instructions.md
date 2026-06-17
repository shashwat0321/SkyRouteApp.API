# SkyRoute - Clean Architecture Setup Instructions

## Overview
Create a Clean Architecture solution with 4 projects. Only create project structure, folders, references, and install NuGet packages. Do NOT create any code files or modify any configuration files.

---

## 1. Create Solution and Projects

Create a new solution named **SkyRoute** with these 4 projects:

### Projects to Create:
1. **SkyRoute.API** - ASP.NET Core Web API (.NET 8.0)
2. **SkyRoute.Application** - Class Library (.NET 8.0)
3. **SkyRoute.Domain** - Class Library (.NET 8.0)
4. **SkyRoute.Infrastructure** - Class Library (.NET 8.0)

---

## 2. Configure Project References

Set up these dependencies (project references):

- **SkyRoute.API** → references → **SkyRoute.Application**
- **SkyRoute.Application** → references → **SkyRoute.Domain**
- **SkyRoute.Infrastructure** → references → **SkyRoute.Application**, **SkyRoute.Domain**
- **SkyRoute.Domain** → **NO references** (pure domain layer)

---

## 3. Create Empty Folder Structure

Create ONLY these folders (leave them completely empty):

### SkyRoute.API/

- Controllers/
-Middleware/
-Extensions/

### SkyRoute.Application/

-Services/
-Services/Interfaces/
-DTOs/
-DTOs/Request/
-DTOs/Response/
-Mappers/
-Validators/
-Common/
-Common/Exceptions/

### SkyRoute.Domain/

Entities/
Enums/
Common/

### SkyRoute.Infrastructure/

Data/
Data/Configurations/
Repositories/
Repositories/Interfaces/
Services/

---

## 4. Install NuGet Packages

Install these packages in their respective projects:

### SkyRoute.API
- Microsoft.EntityFrameworkCore.Design

### SkyRoute.Application
- AutoMapper.Extensions.Microsoft.DependencyInjection
- FluentValidation.DependencyInjectionExtensions

### SkyRoute.Infrastructure
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.AspNetCore.Identity.EntityFrameworkCore

### SkyRoute.Domain
- No packages needed (pure domain)

---

## 5. CRITICAL RULES - WHAT NOT TO DO

### ❌ DO NOT:
- Create ANY .cs files (no classes, interfaces, enums - nothing)
- Create ANY configuration files
- Modify appsettings.json (leave it as default)
- Modify Program.cs (leave it as default)
- Write ANY code
- Create empty class files
- Add ANY namespaces or using statements
- Create BaseEntity.cs, User.cs, Flight.cs, or any other files

### ✅ ONLY DO:
- Create the 4 projects
- Set up project references
- Create the empty folder structure
- Install NuGet packages
- Leave everything else untouched

---

## Expected Final Structure
SkyRoute/

├── SkyRoute.sln

│

├── SkyRoute.API/

│   ├── Controllers/          [empty]

│   ├── Middleware/           [empty]

│   ├── Extensions/           [empty]

│   ├── appsettings.json      [default, untouched]

│   └── Program.cs            [default, untouched]

│

├── SkyRoute.Application/

│   ├── Services/

│   │   └── Interfaces/       [empty]

│   ├── DTOs/

│   │   ├── Request/          [empty]

│   │   └── Response/         [empty]

│   ├── Mappers/              [empty]

│   ├── Validators/           [empty]

│   └── Common/

│       └── Exceptions/       [empty]

│

├── SkyRoute.Domain/

│   ├── Entities/             [empty]

│   ├── Enums/                [empty]

│   └── Common/               [empty]

│

└── SkyRoute.Infrastructure/

├── Data/

│   └── Configurations/   [empty]

├── Repositories/

│   └── Interfaces/       [empty]

└── Services/             [empty]


---

## Verification Checklist

After completion, verify:
- [ ] 4 projects created with correct names
- [ ] Project references configured correctly
- [ ] All folders created and empty
- [ ] NuGet packages installed in correct projects
- [ ] No code files created
- [ ] appsettings.json unchanged (default template)
- [ ] Program.cs unchanged (default template)

---

## Summary

This setup creates the skeleton Clean Architecture structure.
All actual implementation (entities, services, controllers, configurations) will be manually coded as part of the learning process.