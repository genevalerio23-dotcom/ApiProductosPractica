# ApiProductosPractica

API REST desarrollada en C# con .NET 8, arquitectura en capas y acceso a datos mediante ADO.NET puro.

El proyecto implementa la gestión de productos y categorías utilizando SQL Server, procedimientos almacenados, DTOs, inyección de dependencias, programación asíncrona, manejo global de excepciones, Swagger, logging y AutoMapper.

## Objetivo

Construir una API REST aplicando una arquitectura separada por responsabilidades:

- Presentación
- Lógica de negocio
- Acceso a datos
- Entidades
- DTOs

El acceso a SQL Server se realiza con ADO.NET utilizando `Microsoft.Data.SqlClient`, sin Entity Framework.

## Tecnologías utilizadas

- C#
- .NET 8
- ASP.NET Core Web API
- SQL Server
- ADO.NET
- Microsoft.Data.SqlClient
- Swagger / OpenAPI
- AutoMapper
- ILogger
- Visual Studio 2022
- SQL Server Management Studio

## Arquitectura del proyecto

La solución está dividida en cinco proyectos:

```text
ApiProductos.sln
│
├── ApiProductos.Api
│   ├── Controllers
│   ├── Middlewares
│   ├── Program.cs
│   └── appsettings.json
│
├── ApiProductos.Negocio
│   ├── Excepciones
│   ├── ProductoServicio.cs
│   ├── CategoriaServicio.cs
│   ├── IProductoServicio.cs
│   ├── ICategoriaServicio.cs
│   └── MapeoProfile.cs
│
├── ApiProductos.Datos
│   ├── Conexion
│   └── Repositorios
│
├── ApiProductos.Entidades
│   ├── Producto.cs
│   └── Categoria.cs
│
├── ApiProductos.Dto
│   ├── DTOs de Productos
│   ├── DTOs de Categorías
│   └── RespuestaApi.cs
│
└── Database
    ├── 01_CrearBaseDatos.sql
    ├── 02_ProcedimientosProductos.sql
    ├── 03_ProcedimientosCategorias.sql
    └── 04_ProductosExtras.sql
```

## Flujo de una petición

El flujo principal de la aplicación es:

```text
Cliente / Swagger
        ↓
Controller
        ↓
Servicio de Negocio
        ↓
Repositorio
        ↓
ADO.NET
        ↓
SQL Server
```

La capa API no accede directamente a la base de datos.

## Base de datos

La aplicación utiliza una base de datos SQL Server llamada:

```text
TiendaDB
```

Contiene las tablas:

```text
Categorias
Productos
```

La relación principal es:

```text
Categorias.IdCategoria
          ↓
Productos.IdCategoria
```

## Scripts SQL

Los scripts necesarios para reconstruir la base de datos se encuentran en la carpeta:

```text
Database/
```

Se recomienda ejecutarlos en este orden:

```text
01_CrearBaseDatos.sql
02_ProcedimientosProductos.sql
03_ProcedimientosCategorias.sql
04_ProductosExtras.sql
```

El primer script crea:

- Base de datos `TiendaDB`
- Tabla `Categorias`
- Tabla `Productos`
- Relación entre ambas tablas
- Datos iniciales de prueba

## Procedimientos almacenados

### Productos

```text
sp_Productos_ListarTodos
sp_Productos_ObtenerPorId
sp_Productos_Insertar
sp_Productos_Actualizar
sp_Productos_Eliminar
sp_Productos_ListarPaginado
sp_Productos_ListarPorCategoria
```

### Categorías

```text
sp_Categorias_ListarTodos
sp_Categorias_ObtenerPorId
sp_Categorias_Insertar
sp_Categorias_Actualizar
sp_Categorias_Eliminar
```

La eliminación de productos y categorías se implementa mediante baja lógica utilizando el campo `Activo`.

## Configuración de la conexión

La cadena de conexión se encuentra en:

```text
ApiProductos.Api/appsettings.json
```

Ejemplo para SQL Server Express local:

```json
{
  "ConnectionStrings": {
    "TiendaDB": "Server=.\\SQLEXPRESS;Database=TiendaDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Si la instancia de SQL Server tiene otro nombre, debe modificarse el valor de `Server`.

## Ejecutar el proyecto

Desde la carpeta raíz:

```bash
dotnet restore
dotnet build
```

Luego:

```bash
cd ApiProductos.Api
dotnet run
```

También puede ejecutarse directamente desde Visual Studio utilizando `F5`.

Al iniciar la aplicación, Swagger estará disponible en una dirección similar a:

```text
https://localhost:xxxx/swagger
```

El puerto puede variar según la configuración local.

## Endpoints de Productos

### Listar productos con paginación

```http
GET /api/Productos?pagina=1&tamanioPagina=10
```

La paginación se implementa en SQL Server utilizando:

```sql
OFFSET
FETCH NEXT
```

### Obtener producto por ID

```http
GET /api/Productos/{id}
```

### Crear producto

```http
POST /api/Productos
```

Ejemplo:

```json
{
  "nombre": "Teclado mecánico",
  "descripcion": "RGB",
  "precio": 35.5,
  "stock": 10,
  "idCategoria": 1
}
```

### Actualizar producto

```http
PUT /api/Productos/{id}
```

Ejemplo:

```json
{
  "idProducto": 1,
  "nombre": "Mouse Pro",
  "descripcion": "Actualizado",
  "precio": 19.99,
  "stock": 40,
  "idCategoria": 1
}
```

### Eliminar producto

```http
DELETE /api/Productos/{id}
```

La eliminación es lógica, estableciendo:

```text
Activo = 0
```

### Productos por categoría

```http
GET /api/Productos/categoria/{idCategoria}
```

Permite obtener únicamente los productos pertenecientes a una categoría determinada.

### Contador de productos activos

```http
GET /api/Productos/contador
```

Este endpoint utiliza `ExecuteScalarAsync()` para ejecutar:

```sql
SELECT COUNT(*)
FROM Productos
WHERE Activo = 1;
```

## Endpoints de Categorías

### Listar categorías

```http
GET /api/Categorias
```

### Obtener categoría por ID

```http
GET /api/Categorias/{id}
```

### Crear categoría

```http
POST /api/Categorias
```

Ejemplo:

```json
{
  "nombre": "Deportes"
}
```

### Actualizar categoría

```http
PUT /api/Categorias/{id}
```

Ejemplo:

```json
{
  "idCategoria": 4,
  "nombre": "Deportes y Fitness"
}
```

### Eliminar categoría

```http
DELETE /api/Categorias/{id}
```

## DTOs y validaciones

La API utiliza DTOs diferentes para entrada, actualización y salida de información.

Entre las validaciones implementadas se encuentran:

- `[Required]`
- `[StringLength]`
- `[Range]`

Esto permite validar los datos antes de enviarlos a la capa de negocio y evita exponer directamente las entidades del dominio.

## Manejo de excepciones

Se implementó un middleware global:

```text
ManejadorErroresMiddleware
```

El middleware maneja excepciones como:

```text
ProductoNoEncontradoException
CategoriaNoEncontradaException
ReglaDeNegocioException
```

De esta manera los Controllers no necesitan repetir bloques `try/catch`.

## Programación asíncrona

Las operaciones de acceso a datos utilizan `async/await`.

Entre los métodos utilizados se encuentran:

```text
OpenAsync()
ExecuteReaderAsync()
ExecuteNonQueryAsync()
ExecuteScalarAsync()
ReadAsync()
```

## Logging

El repositorio de Productos utiliza `ILogger` para registrar las operaciones de escritura:

- Creación de productos
- Actualización de productos
- Eliminación lógica de productos

Ejemplo:

```text
Producto creado con Id ...
Producto actualizado con Id ...
Producto eliminado lógicamente con Id ...
```

## AutoMapper

Se incorporó AutoMapper para reducir el código repetitivo al transformar entidades y DTOs.

Las reglas de mapeo se encuentran centralizadas en:

```text
MapeoProfile.cs
```

Entre los mapeos configurados se encuentran:

```text
Producto → ProductoDto
ProductoCrearDto → Producto
ProductoActualizarDto → Producto

Categoria → CategoriaDto
CategoriaCrearDto → Categoria
CategoriaActualizarDto → Categoria
```

Antes de utilizar AutoMapper era necesario asignar manualmente cada propiedad durante las conversiones.

Con AutoMapper los servicios contienen menos código repetitivo y pueden concentrarse principalmente en la lógica de negocio.

En proyectos pequeños el mapeo manual puede resultar más explícito, mientras que AutoMapper facilita el mantenimiento cuando aumenta la cantidad de entidades y DTOs.

## Funcionalidades implementadas

La práctica incluye:

- CRUD completo de Productos
- CRUD completo de Categorías
- Arquitectura física en capas
- ADO.NET puro
- Procedimientos almacenados
- SQL parametrizado
- DTOs
- Validaciones con DataAnnotations
- Inyección de dependencias
- Métodos asíncronos
- Manejo de valores NULL
- Manejo global de excepciones
- Swagger
- Baja lógica
- Paginación con `OFFSET/FETCH`
- Filtro de productos por categoría
- Contador mediante `ExecuteScalarAsync`
- Logging mediante `ILogger`
- Mapeo mediante AutoMapper

## Pruebas

Los endpoints fueron probados mediante Swagger.

Se verificaron respuestas HTTP como:

```text
200 OK
201 Created
400 Bad Request
404 Not Found
```

También se comprobó la conexión entre la API y SQL Server, así como las operaciones de lectura, creación, actualización y eliminación lógica.

## Repositorio

Proyecto desarrollado como práctica académica de implementación de una API REST en C# utilizando arquitectura en capas y ADO.NET.
