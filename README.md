# EcommerceApi

## Descripción
EcommerceApi es una API RESTful desarrollada en .NET que implementa una arquitectura limpia (Clean Architecture) para gestionar operaciones de comercio electrónico. El proyecto está estructurado en capas siguiendo los principios SOLID y las mejores prácticas de desarrollo de software.

## Arquitectura
El proyecto está organizado en las siguientes capas:

- **EcommerceApi.Api**: Capa de presentación que contiene los controladores, middleware y configuración de la API.
- **EcommerceApi.Application**: Capa de aplicación que contiene los casos de uso, DTOs y servicios de aplicación.
- **EcommerceApi.Core**: Capa de dominio que contiene las entidades, interfaces y lógica de negocio central.
- **EcommerceApi.Infrastructure**: Capa de infraestructura que implementa las interfaces definidas en Core y maneja la persistencia de datos.

## Requisitos Previos
- .NET 7.0 o superior
- Visual Studio 2022 o Visual Studio Code
- SQL Server (o la base de datos que se esté utilizando)
- Git

## Configuración del Proyecto

### 1. Clonar el Repositorio
```bash
git clone [URL_DEL_REPOSITORIO]
cd EcommerceApi
```

### 2. Configurar la Base de Datos
1. Asegúrate de tener SQL Server instalado y en ejecución
2. Actualiza la cadena de conexión en `appsettings.json`
3. Ejecuta las migraciones de Entity Framework Core

### 3. Restaurar Dependencias
```bash
dotnet restore
```

### 4. Ejecutar el Proyecto
```bash
dotnet run --project EcommerceApi.Api
```

## Estructura del Proyecto
```
EcommerceApi/
├── EcommerceApi.Api/           # Capa de presentación
│   ├── Controllers/            # Controladores de la API
│   └── Program.cs              # Punto de entrada de la aplicación
│
├── EcommerceApi.Application/   # Capa de aplicación
│   ├── DTOs/                   # Objetos de Transferencia de Datos
│   ├── Services/               # Servicios de aplicación
│
├── EcommerceApi.Core/          # Capa de dominio
│   ├── Entities/               # Entidades del dominio
│
└── EcommerceApi.Infrastructure/# Capa de infraestructura
    ├── Data/                   # Configuración de la base de datos
```

## Patrones y Prácticas Implementadas
- Clean Architecture
- Repository Pattern
- Unit of Work
- Dependency Injection
- CQRS (Command Query Responsibility Segregation)
- Mediator Pattern
- SOLID Principles
- RESTful API Design



## Licencia
Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## Contacto
Para cualquier consulta o sugerencia, por favor contacta con el equipo de desarrollo. 
