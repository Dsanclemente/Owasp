using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SecureApp.Domain.Users;
using SecureApp.Domain.Vulnerabilities;
using SecureApp.Infrastructure.Repositories;
using SecureApp.Infrastructure.Services;
using SecureApp.WebApi.Configuration;
using SecureApp.WebApi.Middleware;
using SecureApp.WebApi.Services;
using SecureApp.Domain.Users.Services;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.Filters;
using SecureApp.Application.Usuarios;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Configurar HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443;
});

// Configurar CORS
var corsSettings = builder.Configuration.GetSection("Cors").Get<CorsSettings>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("SecureAppPolicy", policy =>
    {
        policy.WithOrigins(corsSettings.AllowedOrigins.ToArray())
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = actionContext =>
        {
            var errors = actionContext.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .Select(e => new
                {
                    Campo = e.Key,
                    Errores = e.Value?.Errors.Select(e => e.ErrorMessage)
                });

            return new BadRequestObjectResult(new
            {
                Titulo = "Error de validación",
                Status = 400,
                Errores = errors
            });
        };
    });

// Agregar después de builder.Services.AddControllers();
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
    logging.AddEventSourceLogger();
});

// Agregar después de builder.Services.AddControllers();
builder.Services.AddHttpClient("SecureClient", client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "SecureApp");
    client.Timeout = TimeSpan.FromSeconds(30);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    AllowAutoRedirect = false,
    MaxAutomaticRedirections = 0,
    CheckCertificateRevocationList = true
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "SecureApp API", 
        Version = "v1",
        Description = @"
# SecureApp - API Segura OWASP

Esta API implementa los 10 principios de seguridad OWASP 2021:

## 1. Broken Access Control (A1:2021)
- Autenticación JWT implementada
- Autorización basada en roles
- Validación de permisos en endpoints
- Protección contra acceso no autorizado

## 2. Cryptographic Failures (A2:2021)
- Almacenamiento seguro de contraseñas (BCrypt)
- Tokens JWT firmados
- HTTPS habilitado
- Datos sensibles encriptados

## 3. Injection (A3:2021)
- Validación de entrada de datos
- Sanitización de datos
- Prevención de SQL Injection
- Validación de tipos

## 4. Insecure Design (A4:2021)
- Arquitectura limpia
- Separación de responsabilidades
- Patrones de diseño seguros
- Validación en múltiples capas

## 5. Security Misconfiguration (A5:2021)
- Configuración segura por defecto
- Headers de seguridad
- CORS configurado
- Configuración por ambiente

## 6. Vulnerable Components (A6:2021)
- Dependencias actualizadas
- Análisis de vulnerabilidades
- Componentes mínimos
- Monitoreo de dependencias

## 7. Authentication Failures (A7:2021)
- Autenticación multifactor
- Protección contra fuerza bruta
- Gestión segura de sesiones
- Validación robusta

## 8. Software Integrity (A8:2021)
- Validación de integridad
- Firmas digitales
- Protección contra manipulación
- Validación en tránsito

## 9. Logging Failures (A9:2021)
- Logging detallado
- Monitoreo de actividades
- Registro de intentos
- Alertas de seguridad

## 10. SSRF (A10:2021)
- Validación de URLs
- Listas blancas
- Protección SSRF
- Validación de recursos

Para más información sobre OWASP, visita: https://owasp.org/
",
        Contact = new OpenApiContact
        {
            Name = "Equipo de Desarrollo",
            Email = "dev@secureapp.com"
        }
    });

    // Configuración de seguridad JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Agregar documentación XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Configurar ejemplos para los endpoints
    c.ExampleFilters();
});

// Agregar soporte para ejemplos
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

// Configure JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

// Register services
builder.Services.AddScoped<IUsuarioRepository, InMemoryUsuarioRepository>();
builder.Services.AddScoped<IVulnerabilidadRepository, InMemoryVulnerabilidadRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordValidationService, PasswordValidationService>();
builder.Services.AddScoped<ILoginAttemptService, LoginAttemptService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<CreateUsuarioHandler>();
builder.Services.AddScoped<GetUsuarioHandler>();
builder.Services.AddScoped<UpdateUsuarioHandler>();

var app = builder.Build();

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseCors("SecureAppPolicy");

// Add global error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
    context.Response.Headers.Add("Permissions-Policy", "geolocation=(), microphone=(), camera=()");
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
