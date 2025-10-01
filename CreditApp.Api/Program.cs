using System.Text.Json;
using System.Text.Json.Serialization;
using CreditApp.Api.Data;
using CreditApp.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// === Connection string (Render: usar env var ConnectionStrings__Default)
var connectionString =
    builder.Configuration.GetConnectionString("Default") ??
    Environment.GetEnvironmentVariable("ConnectionStrings__Default") ??
    "Host=localhost;Port=5432;Database=creditapp;Username=postgres;Password=postgres";

// === DbContext (PostgreSQL)
builder.Services.AddDbContext<CreditAppDbContext>(opt =>
    opt.UseNpgsql(connectionString));

// === Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// === CORS (origen del front)
var corsOrigin =
    builder.Configuration["Cors:Origin"] ??
    Environment.GetEnvironmentVariable("CORS_ORIGIN") ??
    "http://localhost:5173";

builder.Services.AddCors(o =>
{
    o.AddPolicy("app", p => p
        .WithOrigins(corsOrigin)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

// === JSON (camelCase + enums como string)
builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    o.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// ================== BUILD ==================
var app = builder.Build();

// Ejecutar migraciones al arrancar (si no hay, crea esquema)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CreditAppDbContext>();
    try { db.Database.Migrate(); }
    catch { db.Database.EnsureCreated(); }
}

app.UseCors("app");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { ok = true, time = DateTime.UtcNow }));

// ================== API ==================
var api = app.MapGroup("/api");

// ---- Clients ----
var clients = api.MapGroup("/clients");
clients.MapGet("/", async (CreditAppDbContext db) => await db.Clients.AsNoTracking().ToListAsync());
clients.MapGet("/{id:guid}", async (Guid id, CreditAppDbContext db)
        => await db.Clients.FindAsync(id) is { } c ? Results.Ok(c) : Results.NotFound());
clients.MapPost("/", async (Client c, CreditAppDbContext db) =>
{
    db.Clients.Add(c);
    await db.SaveChangesAsync();
    return Results.Created($"/api/clients/{c.Id}", c);
});
clients.MapPut("/{id:guid}", async (Guid id, Client dto, CreditAppDbContext db) =>
{
    var c = await db.Clients.FindAsync(id);
    if (c is null) return Results.NotFound();
    db.Entry(c).CurrentValues.SetValues(dto);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
clients.MapDelete("/{id:guid}", async (Guid id, CreditAppDbContext db) =>
{
    var c = await db.Clients.FindAsync(id);
    if (c is null) return Results.NotFound();
    db.Remove(c);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ---- Units ----
var units = api.MapGroup("/units");
units.MapGet("/", async (CreditAppDbContext db) => await db.Units.AsNoTracking().ToListAsync());
units.MapGet("/{id:guid}", async (Guid id, CreditAppDbContext db)
        => await db.Units.FindAsync(id) is { } u ? Results.Ok(u) : Results.NotFound());
units.MapPost("/", async (Unit u, CreditAppDbContext db) =>
{
    db.Units.Add(u);
    await db.SaveChangesAsync();
    return Results.Created($"/api/units/{u.Id}", u);
});
units.MapPut("/{id:guid}", async (Guid id, Unit dto, CreditAppDbContext db) =>
{
    var u = await db.Units.FindAsync(id);
    if (u is null) return Results.NotFound();
    db.Entry(u).CurrentValues.SetValues(dto);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
units.MapDelete("/{id:guid}", async (Guid id, CreditAppDbContext db) =>
{
    var u = await db.Units.FindAsync(id);
    if (u is null) return Results.NotFound();
    db.Remove(u);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ---- Configs ----
var configs = api.MapGroup("/configs");
configs.MapGet("/", async (CreditAppDbContext db) => await db.Configs.AsNoTracking().ToListAsync());
configs.MapPost("/", async (Config c, CreditAppDbContext db) =>
{
    db.Configs.Add(c);
    await db.SaveChangesAsync();
    return Results.Created($"/api/configs/{c.Id}", c);
});
configs.MapPut("/{id:guid}", async (Guid id, Config dto, CreditAppDbContext db) =>
{
    var c = await db.Configs.FindAsync(id);
    if (c is null) return Results.NotFound();
    db.Entry(c).CurrentValues.SetValues(dto);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
configs.MapDelete("/{id:guid}", async (Guid id, CreditAppDbContext db) =>
{
    var c = await db.Configs.FindAsync(id);
    if (c is null) return Results.NotFound();
    db.Remove(c);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ---- Simulations ----
var sims = api.MapGroup("/simulations");
sims.MapGet("/", async (CreditAppDbContext db) => await db.Simulations.AsNoTracking().ToListAsync());
sims.MapPost("/", async (Simulation s, CreditAppDbContext db) =>
{
    db.Simulations.Add(s);
    await db.SaveChangesAsync();
    return Results.Created($"/api/simulations/{s.Id}", s);
});
sims.MapDelete("/{id:guid}", async (Guid id, CreditAppDbContext db) =>
{
    var s = await db.Simulations.FindAsync(id);
    if (s is null) return Results.NotFound();
    db.Remove(s);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ---- Audit ----
var audit = api.MapGroup("/audit");
audit.MapGet("/", async (CreditAppDbContext db)
        => await db.Audit.AsNoTracking().OrderByDescending(a => a.Timestamp).ToListAsync());
audit.MapPost("/", async (AuditLog a, CreditAppDbContext db) =>
{
    db.Audit.Add(a);
    await db.SaveChangesAsync();
    return Results.Created($"/api/audit/{a.Id}", a);
});

app.Run();
