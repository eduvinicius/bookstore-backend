using System.Text;
using AutoMapper;
using Bookstore.Api.Mapping;
using Bookstore.App.Services;
using Bookstore.App.Services.External;
using Bookstore.App.Services.Interfaces;
using Bookstore.Infrastructure.Data;
using Bookstore.Infrastructure.Repositories;
using Bookstore.Infrastructure.Repositories.Interfaces;
using Bookstore.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Bookstore.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// LOGGING
// ============================================
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
builder.Logging.AddFilter("Bookstore", LogLevel.Information);

// ============================================
// CORS CONFIGURATION
// ============================================
var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
    ?? new[] { "http://localhost:3000", "http://localhost:4200" };

builder.Services.AddCors(options =>
{
    // Política para Desenvolvimento (permissiva)
    options.AddPolicy("DevelopmentPolicy", policy =>
    {
        policy.WithOrigins(corsOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials() // Permite cookies e autenticação
              .WithExposedHeaders("Content-Disposition"); // Para download de arquivos
    });

    // Política para Produção (restritiva)
    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration["Cors:ProductionOrigin"] ?? "https://seu-frontend.com")
              .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
              .WithHeaders("Authorization", "Content-Type", "Accept")
              .AllowCredentials()
              .SetIsOriginAllowedToAllowWildcardSubdomains(); // Permite subdomínios
    });

    // Política ABERTA (APENAS PARA TESTES - NÃO USE EM PRODUÇÃO!)
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ============================================
// AUTHENTICATION & AUTHORIZATION
// ============================================
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };

        // Configuração para permitir JWT via query string (útil para SignalR/WebSockets)
        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// ============================================
// CONTROLLERS & JSON
// ============================================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddAutoMapper(cfg => {
    cfg.AddProfile<AutoMapperProfile>();
});

// ============================================
// DATABASE
// ============================================
builder.Services.AddDbContext<BookstoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookstoreDB")));

// ============================================
// SWAGGER/OPENAPI
// ============================================
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Bookstore API",
        Version = "v1",
        Description = "API for managing books and bookcases",
        Contact = new OpenApiContact
        {
            Name = "Bookstore Team",
            Email = "contact@bookstore.com"
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
});

// ============================================
// SERVICES
// ============================================
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IGoogleBooksService, GoogleBooksService>();
builder.Services.AddOpenApi();

builder.Services.AddScoped<JwtTokenGenerator>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookcaseRepository, BookcaseRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookcasesService, BookcasesService>();
builder.Services.AddScoped<IBookImportService, BookImportService>();

var app = builder.Build();

// ============================================
// MIDDLEWARE PIPELINE (ORDEM IMPORTA!)
// ============================================

// 1. Exception Handler (SEMPRE PRIMEIRO)
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// 2. CORS (ANTES de Authentication/Authorization)
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevelopmentPolicy");
}
else
{
    app.UseCors("ProductionPolicy");
}

// 3. Development Tools
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bookstore API v1");
        options.RoutePrefix = "swagger";
    });
    app.MapOpenApi();
}

// 4. HTTPS Redirection
app.UseHttpsRedirection();

// 5. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 6. Controllers
app.MapControllers();

// ============================================
// STARTUP LOG
// ============================================
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Bookstore API started successfully at {Time}", DateTime.UtcNow);
logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
logger.LogInformation("CORS Policy: {Policy}", app.Environment.IsDevelopment() ? "DevelopmentPolicy" : "ProductionPolicy");

app.Run();
