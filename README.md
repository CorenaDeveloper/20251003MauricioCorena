# Sistema de Gestión de Productos

Aplicación web desarrollada en ASP.NET Core 9.0 con Razor Pages para administrar productos de una tienda en línea.

## Descripción

Este proyecto permite gestionar un catálogo de productos, incluyendo operaciones de crear, editar, eliminar y actualizar precios. 

## Tecnologías

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
2. Abrir la solución en Visual Studio
3. Ejecutar las migraciones de base de datos:

```
Update-Database
```


## Configuracion de Base de Datos

La aplicación usa Code First con Entity Framework. La cadena de conexión está en `appsettings.json` y apunta a LocalDB por defecto.

Se incluyen dos migraciones:
- Creación de la tabla Productos
- Creación del procedimiento almacenado sp_ObtenerProductos

## Estructura del Proyecto

```
Models/              - Entidades del dominio
Data/                - Contexto de base de datos
Repositories/        - Acceso a datos
Services/            - Lógica de negocio
Pages/Productos/     - Páginas para el CRUD
Migrations/          - Migraciones de EF Core
wwwroot/images/      - Almacenamiento de imágenes
```

## Funcionalidades Implementadas

### HU-001: Gestión de Productos

Permite agregar, editar y eliminar productos del catálogo. Cada producto tiene:
- Nombre
- Descripción
- Precio base
- Precio con descuento (opcional)
- Imagen

La lista de productos se obtiene mediante un procedimiento almacenado (sp_ObtenerProductos) para optimizar las consultas.

### HU-002: Configuración de Precios

Permite actualizar los precios de los productos con las siguientes validaciones:
- El precio base debe ser mayor que cero
- El precio con descuento (si existe) debe ser mayor que cero
- El precio con descuento debe ser menor al precio base

### Carga de Imágenes

Las imágenes se almacenan en la carpeta wwwroot/images/productos/. Se valida que sean formatos JPG, PNG o GIF y que no excedan 5MB.

## Arquitectura

El proyecto utiliza una arquitectura en capas:

**Capa de Datos (Repository Pattern)**
- IProductoRepository: Define las operaciones de acceso a datos
- ProductoRepository: Implementa las operaciones usando Entity Framework

**Capa de Negocio (Service Layer)**
- IProductoService: Define la lógica de negocio
- ProductoService: Implementa las validaciones y reglas de negocio

**Capa de Presentación (Razor Pages)**
- Index: Lista de productos
- Create: Crear producto
- Edit: Editar producto
- EditPrice: Actualizar precios
- Delete: Eliminar producto

## Principios SOLID

El proyecto aplica los principios SOLID:

**Single Responsibility**: Cada clase tiene una única responsabilidad (ej: ProductoRepository solo maneja acceso a datos)

**Open/Closed**: Se pueden agregar nuevas funcionalidades sin modificar el código existente, usando interfaces

**Liskov Substitution**: Las implementaciones de las interfaces pueden intercambiarse sin problemas

**Interface Segregation**: Las interfaces son específicas (IProductoRepository, IProductoService)

**Dependency Inversion**: Las clases dependen de abstracciones (interfaces) en lugar de implementaciones concretas

La inyección de dependencias se configura en Program.cs:

```csharp
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
```

## Consultas Eficientes

Se utilizan las siguientes técnicas para optimizar las consultas:

- AsNoTracking() en consultas de solo lectura
- Procedimiento almacenado para obtener la lista de productos
- Consultas asíncronas (async/await)

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