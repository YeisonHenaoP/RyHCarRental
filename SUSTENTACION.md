# Guía de sustentación — RyH Car Rental

Documento de estudio para la defensa oral del proyecto. Lenguaje simple.

---

## Discurso de apertura (1 minuto)

> Somos el grupo RyH Car Rental. Desarrollamos una API de alquiler de vehículos en .NET 8.
>
> Un cliente puede rentar uno o varios carros por un rango de fechas. El sistema valida que los carros estén libres, calcula el precio por días y guarda todo en SQL Server.
>
> Seguimos la misma arquitectura de 3 capas que vimos en SportsLeague: API, Domain y DataAccess.

---

## Preguntas frecuentes del profesor

### 1. ¿Qué es una API REST?

Es un servicio web que responde con JSON. Cada URL es un recurso (`/api/Vehicles`, `/api/Rentals`) y los verbos HTTP indican la acción: GET consultar, POST crear, PUT actualizar, DELETE borrar.

### 2. ¿Por qué 3 capas y no todo en el Controller?

Para separar responsabilidades. Si mañana cambiamos SQL Server por PostgreSQL, solo tocamos DataAccess. Si cambiamos el frontend, solo tocamos API. La lógica de negocio en Domain no se rompe.

### 3. ¿Qué hace Domain?

Define las entidades (Customer, Vehicle, Rental…), los enums de estado, las interfaces (contratos) y los Services con las reglas de negocio. **No conoce HTTP ni Entity Framework.**

### 4. ¿Qué hace DataAccess?

Implementa los repositorios con Entity Framework Core. Guarda y lee de SQL Server. También tiene las migraciones y el DataSeeder.

### 5. ¿Qué hace la API?

Recibe peticiones HTTP, usa DTOs (no expone entidades crudas), llama a los Services y devuelve JSON. AutoMapper convierte Entity ↔ DTO.

### 6. ¿Qué es el patrón Repository?

Es una capa entre el Service y la base de datos. El Service dice "tráeme el vehículo 1" y el Repository ejecuta la consulta. `GenericRepository` tiene operaciones comunes; repositorios específicos tienen consultas especiales (por placa, por disponibilidad).

### 7. ¿Qué es un DTO?

Data Transfer Object. Es lo que viaja por la red. Separar DTO de Entity evita exponer campos internos o relaciones circulares en el JSON.

### 8. ¿Qué hace AutoMapper?

Evita mapear campo por campo a mano. Configuramos perfiles en `MappingProfile.cs` y AutoMapper convierte automáticamente.

### 9. ¿Qué es Entity Framework Core?

Es un ORM: mapea clases C# a tablas SQL. Usamos **Code First**: escribimos las clases y EF genera las tablas con migraciones.

### 10. ¿Qué es una migración?

Un archivo que describe cambios en la BD (nueva tabla, índice, FK). `dotnet ef migrations add` la crea; `MigrateAsync()` o `database update` la aplica.

### 11. ¿Qué hace el DataSeeder?

Al arrancar la API, si no hay datos, inserta tipos de vehículo, sucursales, vehículos y clientes de prueba. Así no hay que insertar manualmente en SQL.

### 12. ¿Cuáles son las relaciones 1:N?

- Un cliente tiene muchos alquileres.
- Un tipo de vehículo tiene muchos vehículos.
- Una sucursal tiene muchos vehículos.

### 13. ¿Cuál es la relación N:M?

Un alquiler puede incluir varios vehículos y un vehículo puede aparecer en varios alquileres (en fechas distintas). La tabla `RentalDetail` es el puente: guarda RentalId, VehicleId y SubTotal.

### 14. ¿Para qué sirven los enums?

Para estados controlados. `VehicleStatus`: Available, Rented, InMaintenance. `RentalStatus`: Active, Completed, Cancelled. Evita textos inválidos en la BD.

### 15. ¿Cómo se calcula el costo de un alquiler?

```
días = EndDate - StartDate (mínimo 1)
subtotal por vehículo = DailyRate × días
TotalCost = suma de todos los subtotales
```

### 16. ¿Cómo se valida disponibilidad?

`RentalRepository.IsVehicleAvailableAsync` verifica que el vehículo no esté en un alquiler **Active** cuyas fechas se crucen con las solicitadas.

### 17. ¿Qué pasa al completar o cancelar un alquiler?

El rental pasa a Completed o Cancelled y cada vehículo del detalle vuelve a `Available`.

### 18. ¿Qué validaciones tiene VehicleService?

Placa única, tarifa > 0, año válido, tipo y sucursal existen. No se edita ni elimina un vehículo en estado `Rented`.

### 19. ¿Qué es la inyección de dependencias?

En `Program.cs` registramos `IVehicleService → VehicleService`. .NET crea las instancias automáticamente y las pasa a los Controllers. Facilita pruebas y desacoplamiento.

### 20. ¿Qué códigos HTTP usan?

- 200 OK — consulta exitosa
- 201 Created — recurso creado
- 204 No Content — actualización/borrado OK
- 400 Bad Request — datos inválidos
- 404 Not Found — no existe
- 409 Conflict — regla de negocio (placa duplicada, vehículo alquilado)

---

## Demo sugerida en Swagger (5 minutos)

1. **GET** `/api/VehicleTypes` — mostrar datos del seeder.
2. **GET** `/api/Vehicles` — vehículos con tipo y sucursal.
3. **POST** `/api/Rentals` — crear alquiler con `customerId: 1`, `vehicleIds: [1]`.
4. **GET** `/api/Rentals/1` — ver costo calculado y detalle.
5. **PUT** `/api/Rentals/1/complete` — completar y liberar vehículo.
6. **GET** `/api/Vehicles/1` — confirmar estado `Available`.

---

## Si te preguntan por un archivo concreto

| Archivo | Respuesta corta |
|---------|-----------------|
| `Program.cs` | Registra servicios, DbContext, Swagger y ejecuta migraciones + seeder |
| `ApplicationDbContext.cs` | Define tablas, relaciones, índices únicos |
| `GenericRepository.cs` | CRUD genérico para cualquier entidad |
| `RentalService.cs` | Lógica más compleja: fechas, disponibilidad, costos, estados |
| `MappingProfile.cs` | Configuración de AutoMapper |
| `DataSeeder.cs` | Datos iniciales automáticos |

---

## Lo que aún falta (honestidad en sustentación)

- Frontend con mínimo 3 vistas consumiendo la API.
- Completar nombres de integrantes en README.
- Opcional: middleware global de errores, autenticación JWT.
