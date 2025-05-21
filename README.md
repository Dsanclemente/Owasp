# SecureApp - Aplicación Segura con OWASP

Este proyecto es una aplicación web segura que implementa las mejores prácticas de seguridad de OWASP. Está diseñada para gestionar vulnerabilidades y usuarios de manera segura.

## 🛡️ Características de Seguridad

### Autenticación y Autorización
- Autenticación JWT con expiración configurable
- Autorización basada en roles (Admin, Usuario)
- Protección contra ataques de fuerza bruta
- Validación robusta de contraseñas
- Tokens JWT seguros con claims personalizados

### Gestión de Usuarios
- Registro seguro de usuarios
- Validación de datos de entrada
- Prevención de enumeración de usuarios
- Protección contra inyección SQL
- Roles de usuario (Admin, Usuario)

### Gestión de Vulnerabilidades
- Registro de vulnerabilidades
- Seguimiento de estado
- Asignación de severidad
- Documentación de soluciones
- Historial de cambios

## 🏗️ Arquitectura

El proyecto sigue una arquitectura limpia (Clean Architecture) con las siguientes capas:

- **Domain**: Entidades y reglas de negocio
- **Application**: Casos de uso y lógica de aplicación
- **Infrastructure**: Implementaciones concretas (EF Core, SQL Server)
- **WebApi**: API REST con Swagger

## 🚀 Tecnologías

- .NET 9.0
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger/OpenAPI
- MediatR
- FluentValidation

## 📋 Requisitos

- .NET 9.0 SDK
- SQL Server
- Visual Studio 2022 o VS Code

## 🔧 Configuración

1. Clonar el repositorio
2. Configurar la cadena de conexión en `appsettings.json`
3. Ejecutar las migraciones:
   ```bash
   dotnet ef database update
   ```
4. Ejecutar la aplicación:
   ```bash
   dotnet run
   ```

## 📚 Documentación API

La documentación de la API está disponible en Swagger UI:
- URL: `https://localhost:5001/swagger`
- Autenticación: JWT Bearer Token

### Endpoints Principales

#### Autenticación
- `POST /api/auth/login`: Inicio de sesión

#### Usuarios
- `GET /api/usuarios/{id}`: Obtener usuario
- `POST /api/usuarios`: Crear usuario (Admin)
- `PUT /api/usuarios/{id}`: Actualizar usuario (Admin)

#### Vulnerabilidades
- `GET /api/vulnerabilidades`: Listar vulnerabilidades
- `POST /api/vulnerabilidades`: Crear vulnerabilidad
- `PUT /api/vulnerabilidades/{id}`: Actualizar vulnerabilidad

## 🔐 Roles de Usuario

- **Admin**: Acceso total al sistema
- **Usuario**: Acceso limitado a funcionalidades básicas

## 🛠️ Desarrollo

### Estructura de Carpetas
```
SecureApp/
├── SecureApp.Domain/         # Entidades y reglas de negocio
├── SecureApp.Application/    # Casos de uso y lógica de aplicación
├── SecureApp.Infrastructure/ # Implementaciones concretas
└── SecureApp.WebApi/        # API REST
```

### Patrones de Diseño
- CQRS (Command Query Responsibility Segregation)
- Repository Pattern
- Unit of Work
- Value Objects
- Domain Events

## 📝 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 🤝 Contribución

1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## 📧 Contacto

David Sanclemente - [@dsanclemente](https://github.com/dsanclemente)

## 🙏 Agradecimientos

- OWASP por sus guías de seguridad
- La comunidad de .NET
- Todos los contribuidores 