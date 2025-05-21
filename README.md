# 🛡️ SecureApp - API Segura OWASP 2021

## 📋 Descripción
API REST segura que implementa los 10 principios de seguridad OWASP 2021, utilizando .NET 8, arquitectura limpia y patrones de diseño seguros.

## 🚀 Características
- ✅ Autenticación JWT
- ✅ Autorización basada en roles
- ✅ Validación robusta de datos
- ✅ Protección contra ataques comunes
- ✅ Logging y monitoreo con Application Insights
- ✅ Documentación con Swagger
- ✅ Headers de seguridad configurados
- ✅ CORS configurado
- ✅ Manejo de errores global
- ✅ Validación de contraseñas segura

## 🛠️ Tecnologías
- .NET 8
- C# 12
- Arquitectura Limpia
- JWT Authentication
- Swagger/OpenAPI
- Application Insights
- BCrypt para hashing
- MediatR para CQRS
- FluentValidation

## 📦 Requisitos
- .NET 8 SDK
- Visual Studio 2022 o VS Code
- Git

## 🚀 Instalación

1. Clonar el repositorio:
```bash
git clone https://github.com/tu-usuario/SecureApp.git
cd SecureApp
```

2. Restaurar dependencias:
```bash
dotnet restore
```

3. Configurar variables de entorno:
```bash
# Crear archivo appsettings.Development.json
{
  "JwtSettings": {
    "SecretKey": "tu_clave_secreta_muy_larga_y_segura_min_32_caracteres",
    "ExpirationInMinutes": 60,
    "Issuer": "https://localhost:7001",
    "Audience": "https://localhost:7001"
  }
}
```

4. Ejecutar la aplicación:
```bash
dotnet run --project SecureApp.WebApi
```

## 📚 Ejemplos de Uso

### 1. Registro de Usuario
```http
POST /api/usuarios/registro
Content-Type: application/json

{
  "nombre": "Usuario Ejemplo",
  "email": "usuario@ejemplo.com",
  "password": "P@ssw0rd123!",
  "rol": "Usuario"
}
```

### 2. Login
```http
POST /api/usuarios/login
Content-Type: application/json

{
  "email": "usuario@ejemplo.com",
  "password": "P@ssw0rd123!"
}
```

### 3. Crear Vulnerabilidad
```http
POST /api/vulnerabilidades
Authorization: Bearer {token}
Content-Type: application/json

{
  "titulo": "Vulnerabilidad XSS",
  "descripcion": "Cross-Site Scripting en formulario de contacto",
  "severidad": "Alta",
  "estado": "Abierta"
}
```

## 🔒 Seguridad

### Headers de Seguridad Implementados
- ✅ Strict-Transport-Security (HSTS)
- ✅ X-Content-Type-Options
- ✅ X-Frame-Options
- ✅ X-XSS-Protection
- ✅ Content-Security-Policy
- ✅ Referrer-Policy
- ✅ Permissions-Policy
- ✅ Cache-Control

### Validaciones de Seguridad
- ✅ Contraseñas seguras (mínimo 8 caracteres, mayúsculas, minúsculas, números y símbolos)
- ✅ Protección contra fuerza bruta
- ✅ Tokens JWT con expiración
- ✅ Sanitización de datos
- ✅ Validación de tipos
- ✅ CORS configurado
- ✅ HTTPS forzado

## 📊 Monitoreo

### Application Insights
La aplicación está configurada con Application Insights para:
- 📈 Monitoreo de rendimiento
- 🔍 Trazabilidad de requests
- ⚠️ Alertas de errores
- 📊 Métricas de uso
- 🔐 Eventos de seguridad

## 🏗️ Estructura del Proyecto
```
SecureApp/
├── SecureApp.Domain/           # Entidades y reglas de negocio
├── SecureApp.Application/      # Casos de uso y lógica de aplicación
├── SecureApp.Infrastructure/   # Implementaciones técnicas
└── SecureApp.WebApi/          # API y configuración
```

## 🤝 Contribuir
1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## 📝 Licencia
Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

## 👥 Autores
- **David Sanclemente** - *Desarrollo inicial* - [@dsanclemente](https://github.com/dsanclemente)

## 🙏 Agradecimientos
- OWASP por sus guías de seguridad
- La comunidad de .NET
- Todos los contribuidores 