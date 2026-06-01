# RyH Car Rental API

Sistema de gestión de alquiler de vehículos desarrollado como proyecto final grupal del curso **Programación Web (ITM)**.  
Tema: **2.6 Sistema de Alquiler de Vehículos**.

Repositorio: [https://github.com/YeisonHenaoP/RyHCarRental](https://github.com/YeisonHenaoP/RyHCarRental)

---

## Integrantes

| Nombre | GitHub | Rol principal |
|--------|--------|---------------|
| Yeison David Henao Pareja | [YeisonHenaoP](https://github.com/YeisonHenaoP) | Backend / API |
| Alexander Ramirez | [aramirez22itm](https://github.com/aramirez22itm) | Backend / Domain |

> Grupo de 2 integrantes. Frontend pendiente de implementación.

---

## Descripción

API REST en **.NET 8** para administrar:

- Clientes (`Customer`)
- Vehículos (`Vehicle`), tipos (`VehicleType`) y sucursales (`Branch`)
- Alquileres (`Rental`) con varios vehículos por contrato (`RentalDetail`)

El sistema valida disponibilidad por fechas, calcula el costo total por días y actualiza el estado de los vehículos (`Available`, `Rented`, `InMaintenance`).

---

## Tecnologías

| Capa | Tecnología |
|------|------------|
| Backend | .NET 8, ASP.NET Core Web API |
| ORM | Entity Framework Core 8 (Code First) |
| Base de datos | SQL Server / LocalDB |
| Mapeo | AutoMapper |
| Documentación | Swagger (Swashbuckle) |
| Frontend | Pendiente (Angular, React, etc.) |

---

## Arquitectura (3 capas)

```
RyHCarRental/              → API (Controllers, DTOs, Mappings, Program.cs)
RyHCarRental.Domain/       → Negocio (Entities, Enums, Services, Interfaces)
RyHCarRental.DataAccess/   → Datos (DbContext, Repositories, Migrations, Seeders)
```

**Flujo de una petición:**

```
HTTP Request → Controller (DTO) → Service (validaciones) → Repository → SQL Server
```

---

## Modelo de datos

### Entidades

| Entidad | Descripción |
|---------|-------------|
| `Customer` | Cliente que realiza alquileres |
| `Vehicle` | Vehículo de la flota |
| `VehicleType` | Categoría (Sedán, SUV, Pickup) |
| `Branch` | Sucursal donde está el vehículo |
| `Rental` | Contrato de alquiler (fechas, costo, estado) |
| `RentalDetail` | Tabla puente Rental ↔ Vehicle (N:M) |

### Relaciones

- **1:N** — `Customer` → `Rental`
- **1:N** — `VehicleType` → `Vehicle`
- **1:N** — `Branch` → `Vehicle`
- **N:M** — `Rental` ↔ `Vehicle` mediante `RentalDetail`

### Enums

- `VehicleStatus`: Available, Rented, InMaintenance
- `RentalStatus`: Active, Completed, Cancelled

---

## Endpoints principales

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET/POST | `/api/Customers` | Clientes |
| GET/POST/PUT/DELETE | `/api/VehicleTypes` | Tipos de vehículo |
| GET/POST/PUT/DELETE | `/api/Branches` | Sucursales |
| GET/POST/PUT/DELETE | `/api/Vehicles` | Vehículos |
| GET/POST | `/api/Rentals` | Alquileres |
| PUT | `/api/Rentals/{id}/complete` | Completar alquiler |
| PUT | `/api/Rentals/{id}/cancel` | Cancelar alquiler |

---

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server Express, Developer Edition o **LocalDB** (incluido con Visual Studio)
- Visual Studio 2022, VS Code o Rider
- Git

---

## Cómo ejecutar el proyecto

### 1. Clonar el repositorio

```bash
git clone https://github.com/YeisonHenaoP/RyHCarRental.git
cd RyHCarRental
```

### 2. Configurar la cadena de conexión

Edita `RyHCarRental/appsettings.json` si usas otra instancia de SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RyHCarRentalDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 3. Restaurar, compilar y ejecutar

```bash
dotnet restore
dotnet build RyHCarRental/RyHCarRental.API.csproj
dotnet run --project RyHCarRental/RyHCarRental.API.csproj
```

### 4. Abrir Swagger

- HTTP: [http://localhost:5293/swagger](http://localhost:5293/swagger)
- HTTPS: [https://localhost:7048/swagger](https://localhost:7048/swagger)

Al iniciar, la API aplica migraciones automáticamente (`MigrateAsync`) y ejecuta el **DataSeeder** si la base de datos está vacía.

---

## Migraciones (manual, opcional)

```bash
dotnet ef database update --project RyHCarRental.DataAccess --startup-project RyHCarRental
```

Crear una nueva migración:

```bash
dotnet ef migrations add NombreMigracion --project RyHCarRental.DataAccess --startup-project RyHCarRental
```

---

## Datos iniciales (DataSeeder)

Si la BD está vacía, se crean automáticamente:

- 3 tipos de vehículo (Sedán, SUV, Pickup)
- 2 sucursales (Medellín, Bogotá)
- 3 vehículos de ejemplo
- 2 clientes de ejemplo

---

## Ejemplo: crear un alquiler

**POST** `/api/Rentals`

```json
{
  "startDate": "2026-06-10",
  "endDate": "2026-06-15",
  "customerId": 1,
  "vehicleIds": [1, 2]
}
```

El sistema valida fechas, disponibilidad, calcula `TotalCost` y marca los vehículos como `Rented`.

---

## Estructura del repositorio

```
RyHCarRental/
├── RyHCarRental/                    # API
│   ├── Controllers/
│   ├── DTOs/
│   │   ├── Request/                 # Datos que envía el cliente (POST/PUT)
│   │   └── Response/                # Datos que devuelve la API (GET)
│   ├── Mappings/
│   └── Program.cs
├── RyHCarRental.Domain/             # Negocio
│   ├── Entities/
│   ├── Enums/
│   ├── Interfaces/
│   │   ├── Repositories/
│   │   └── Services/
│   └── Services/
├── RyHCarRental.DataAccess/         # Datos
│   ├── Context/
│   ├── Repositories/
│   ├── Migrations/
│   └── Seeders/
└── README.md
```

---

## Flujo de trabajo Git (commits en inglés)

```bash
git checkout -b feature/nombre-tarea
# ... hacer cambios ...
git add .
git commit -m "add VehiclesController with CRUD endpoints"
git push -u origin feature/nombre-tarea
```

Abrir Pull Request hacia `master` en GitHub.

**Verbos recomendados:** `add`, `update`, `fix`, `remove`, `add migration`

---

## Frontend (pendiente)

El proyecto final exige **Backend + Frontend**. El backend ya expone la API; el frontend debe consumirla.

### Requisitos mínimos (ITM)

| Requisito | Detalle |
|-----------|---------|
| Framework | Angular, React, Vue, Blazor u otro moderno |
| Consumo de API | HTTP hacia `http://localhost:5293/api/...` |
| 3+ vistas | Listado, formulario, detalle o listado secundario |
| Routing | Navegación entre pantallas |
| UI presentable | Bootstrap, Angular Material, Tailwind, etc. |

### Vistas sugeridas

1. **Vehículos** — listado (`GET /api/Vehicles`)
2. **Nuevo alquiler** — formulario (`POST /api/Rentals`)
3. **Alquileres** — listado y acciones complete/cancel (`GET /api/Rentals`, `PUT /api/Rentals/{id}/complete`)

### Estructura recomendada en el repo

```
RyHCarRental/
├── RyHCarRental/              # Backend API
├── RyHCarRental.Domain/
├── RyHCarRental.DataAccess/
└── ryh-car-rental-web/        # Crear: proyecto frontend
```

### Configuración necesaria

1. **CORS en el backend** — permitir el origen del frontend (ej. `http://localhost:4200`).
2. **URL base de la API** — variable de entorno en el frontend apuntando a `http://localhost:5293/api`.
3. **Models TypeScript** — alineados con los DTOs **Response** del backend (`VehicleResponseDto`, `RentalRequestDto`, etc.).

### Ejecutar full-stack (cuando exista el frontend)

**Terminal 1 — Backend:**

```bash
dotnet run --project RyHCarRental/RyHCarRental.API.csproj
```

**Terminal 2 — Frontend** (ejemplo Angular):

```bash
cd ryh-car-rental-web
npm install
ng serve
```

Documentar aquí los comandos exactos cuando el framework esté elegido.

### Commits del frontend

Usar la misma convención en inglés y la **cuenta GitHub de quien desarrolle** esa parte:

```bash
git checkout -b feature/frontend-initial-setup
git commit -m "add Angular project with vehicle list page"
git push -u origin feature/frontend-initial-setup
```

---

## Estado del proyecto

| Requisito | Estado |
|-----------|--------|
| Backend .NET 8 | ✅ |
| 5+ entidades y relaciones | ✅ |
| Repository + Services | ✅ |
| DTOs + AutoMapper | ✅ |
| Migraciones + DataSeeder | ✅ |
| Swagger | ✅ |
| Frontend | ⏳ Pendiente |
| README | ✅ |

---

## Licencia

Proyecto académico — Instituto Tecnológico Metropolitano (ITM), 2026.
