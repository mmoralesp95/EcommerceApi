# EcommerceApi

## Descripción
EcommerceApi es una API RESTful desarrollada en .NET que implementa una arquitectura limpia (Clean Architecture) para gestionar operaciones de comercio electrónico. El proyecto está estructurado en capas siguiendo los principios SOLID y las mejores prácticas de desarrollo de software. Siguiendo https://roadmap.sh/projects/ecommerce-api

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

### 2. Configuración de Variables de Entorno
1. Copia el archivo `appsettings.example.json` a `appsettings.json`:
   ```bash
   cp EcommerceApi.Api/appsettings.example.json EcommerceApi.Api/appsettings.json
   ```
2. Actualiza los siguientes valores en `appsettings.json`:
   - `ConnectionStrings.DefaultConnection`: Cadena de conexión a tu base de datos
   - `Jwt.Key`: Clave secreta para la generación de tokens JWT
   - `Jwt.Issuer`: Emisor del token JWT
   - `Jwt.Audience`: Audiencia del token JWT
   - `Stripe.SecretKey`: Clave secreta de Stripe
   - `Stripe.WebhookSecret`: Secreto para webhooks de Stripe
   - `Stripe.PublishableKey`: Clave pública de Stripe

### 3. Configurar la Base de Datos
1. Asegúrate de tener SQL Server instalado y en ejecución
2. Ejecuta las migraciones de Entity Framework Core:
   ```bash
   dotnet ef database update --project EcommerceApi.Infrastructure
   ```

### 4. Restaurar Dependencias
```bash
dotnet restore
```

### 5. Ejecutar el Proyecto
```bash
dotnet run --project EcommerceApi.Api
```

## Seguridad
Este proyecto implementa varias medidas de seguridad:

- Autenticación JWT con expiración configurable
- Hash de contraseñas usando algoritmos seguros
- Protección contra CSRF
- Validación de datos de entrada
- Rate limiting para prevenir ataques de fuerza bruta
- Integración segura con Stripe para pagos

### Consideraciones de Seguridad
- Nunca compartas tus archivos `appsettings.json` o `appsettings.Development.json`
- Mantén tus claves JWT y de Stripe seguras
- Usa HTTPS en producción
- Implementa CORS adecuadamente según tu entorno
- Mantén las dependencias actualizadas

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
