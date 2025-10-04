# Sistema de Gesti�n de Productos

Aplicaci�n web desarrollada en ASP.NET Core 9.0 con Razor Pages para administrar productos de una tienda en l�nea.

## Descripci�n

Este proyecto permite gestionar un cat�logo de productos, incluyendo operaciones de crear, editar, eliminar y actualizar precios. 

## Tecnolog�as

- .NET 9.0
- ASP.NET Core Razor Pages
- Entity Framework Core 9.0
- SQL Server LocalDB
- Bootstrap 5
- Nuget Packages:
  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore.SqlServer
  - Microsoft.EntityFrameworkCore.Tools
  - Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
  - Moq
  - xunit


## Requisitos

- Visual Studio 2022
- .NET 9.0 
- SQL Server LocalDB

## Instalacion

1. Clonar el repositorio
2. Abrir la soluci�n en Visual Studio
3. Ejecutar las migraciones de base de datos:

```
Update-Database
```


## Configuracion de Base de Datos

La aplicaci�n usa Code First con Entity Framework. La cadena de conexi�n est� en `appsettings.json` y apunta a LocalDB por defecto.

Se incluyen dos migraciones:
- Creaci�n de la tabla Productos
- Creaci�n del procedimiento almacenado sp_ObtenerProductos

## Estructura del Proyecto

```
Models/              - Entidades del dominio
Data/                - Contexto de base de datos
Repositories/        - Acceso a datos
Services/            - L�gica de negocio
Pages/Productos/     - P�ginas para el CRUD
Migrations/          - Migraciones de EF Core
wwwroot/images/      - Almacenamiento de im�genes
```

## Funcionalidades Implementadas

### HU-001: Gesti�n de Productos

Permite agregar, editar y eliminar productos del cat�logo. Cada producto tiene:
- Nombre
- Descripci�n
- Precio base
- Precio con descuento (opcional)
- Imagen

La lista de productos se obtiene mediante un procedimiento almacenado (sp_ObtenerProductos) para optimizar las consultas.

### HU-002: Configuraci�n de Precios

Permite actualizar los precios de los productos con las siguientes validaciones:
- El precio base debe ser mayor que cero
- El precio con descuento (si existe) debe ser mayor que cero
- El precio con descuento debe ser menor al precio base

### Carga de Im�genes

Las im�genes se almacenan en la carpeta wwwroot/images/productos/. Se valida que sean formatos JPG, PNG o GIF y que no excedan 5MB.

## Arquitectura

El proyecto utiliza una arquitectura en capas:

**Capa de Datos (Repository Pattern)**
- IProductoRepository: Define las operaciones de acceso a datos
- ProductoRepository: Implementa las operaciones usando Entity Framework

**Capa de Negocio (Service Layer)**
- IProductoService: Define la l�gica de negocio
- ProductoService: Implementa las validaciones y reglas de negocio

**Capa de Presentaci�n (Razor Pages)**
- Index: Lista de productos
- Create: Crear producto
- Edit: Editar producto
- EditPrice: Actualizar precios
- Delete: Eliminar producto

## Principios SOLID

El proyecto aplica los principios SOLID:

**Single Responsibility**: Cada clase tiene una �nica responsabilidad (ej: ProductoRepository solo maneja acceso a datos)

**Open/Closed**: Se pueden agregar nuevas funcionalidades sin modificar el c�digo existente, usando interfaces

**Liskov Substitution**: Las implementaciones de las interfaces pueden intercambiarse sin problemas

**Interface Segregation**: Las interfaces son espec�ficas (IProductoRepository, IProductoService)

**Dependency Inversion**: Las clases dependen de abstracciones (interfaces) en lugar de implementaciones concretas

La inyecci�n de dependencias se configura en Program.cs:

```csharp
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
```

## Consultas Eficientes

Se utilizan las siguientes t�cnicas para optimizar las consultas:

- AsNoTracking() en consultas de solo lectura
- Procedimiento almacenado para obtener la lista de productos
- Consultas as�ncronas (async/await)

Ejemplo del procedimiento almacenado:

```sql
CREATE PROCEDURE sp_ObtenerProductos
AS
BEGIN
    SELECT Id, Nombre, Descripcion, PrecioBase, PrecioConDescuento,
           ISNULL(PrecioConDescuento, PrecioBase) AS PrecioActual,
           Imagen, FechaCreacion, FechaModificacion
    FROM Productos
    ORDER BY FechaCreacion DESC;
END
```

## Base de Datos

Tabla Productos:
- Id (int, PK, Identity)
- Nombre (nvarchar(200))
- Descripcion (nvarchar(1000))
- PrecioBase (decimal(18,2))
- PrecioConDescuento (decimal(18,2), nullable)
- Imagen (nvarchar(500))
- FechaCreacion (datetime2)
- FechaModificacion (datetime2)

## Autor

Mauricio Corena

## Fecha

Octubre 2025