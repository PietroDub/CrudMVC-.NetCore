# 📘 CRUD MVC com MongoDB usando Scaffold (Gambiarra com FakeDbContext)

Este guia mostra como criar um CRUD em ASP.NET Core MVC utilizando MongoDB **mesmo com o scaffold do Entity Framework**, através de um `FakeDbContext`.

---

# 🧠 Ideia Geral

```text
Scaffold (EF) → FakeDbContext → gerar Controller/View
Depois → trocar lógica EF → MongoDB
```

👉 O EF é usado **só para gerar código automático**
👉 O MongoDB é usado **de verdade na execução**

---

# 📁 Estrutura do Projeto

```
/Controllers
    ClientesController.cs

/Models
    Cliente.cs

/Data
    ContextMongoDb.cs
    MongoSettings.cs
    FakeDbContext.cs

appsettings.json
Program.cs
```

---

# 🔹 1. Criar Model

```csharp
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

public class Cliente
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required]
    public string Nome { get; set; }

    public string Email { get; set; }
}
```

---

# 🔹 2. Criar MongoSettings

```csharp
public class MongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
```

---

# 🔹 3. Configurar appsettings.json

```json
{
  "MongoSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "MinhaBase"
  }
}
```

---

# 🔹 4. Criar ContextMongoDb

```csharp
using MongoDB.Driver;
using Microsoft.Extensions.Options;

public class ContextMongoDb
{
    private readonly IMongoDatabase _database;

    public ContextMongoDb(IOptions<MongoSettings> settings)
    {
        var config = settings.Value;
        var client = new MongoClient(config.ConnectionString);
        _database = client.GetDatabase(config.DatabaseName);
    }

    public IMongoCollection<Cliente> Clientes =>
        _database.GetCollection<Cliente>("Clientes");
}
```

---

# 🔹 5. Criar FakeDbContext (para Scaffold)

```csharp
using Microsoft.EntityFrameworkCore;

public class FakeDbContext : DbContext
{
    public FakeDbContext(DbContextOptions<FakeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
}
```

---

# 🔹 6. Configurar Program.cs

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));

builder.Services.AddSingleton<MongoSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoSettings>>().Value);

builder.Services.AddSingleton<ContextMongoDb>();

// FAKE para o scaffold funcionar
builder.Services.AddDbContext<FakeDbContext>(options =>
    options.UseInMemoryDatabase("FakeDb"));
```

---

# 🔹 7. Gerar Scaffold

👉 Criar Controller com Views usando:

* Model: Cliente
* DbContext: FakeDbContext

Isso vai gerar:

* Controller CRUD completo
* Views prontas

---

# 🔹 8. Converter Controller (EF → Mongo)

## Substituições principais:

### 🔸 Contexto

```csharp
FakeDbContext → ContextMongoDb
```

---

### 🔸 Index

```csharp
_context.Clientes.ToListAsync()
→
_context.Clientes.Find(_ => true).ToListAsync()
```

---

### 🔸 Details

```csharp
FirstOrDefaultAsync(...)
→
Find(...).FirstOrDefaultAsync()
```

---

### 🔸 Create

```csharp
_context.Add()
SaveChanges()
→
InsertOneAsync()
```

---

### 🔸 Edit

```csharp
Update()
SaveChanges()
→
ReplaceOneAsync()
```

---

### 🔸 Delete

```csharp
Remove()
SaveChanges()
→
DeleteOneAsync()
```

---

### 🔸 Exists

```csharp
Any()
→
Find().AnyAsync()
```

---

# 🚫 Remover tudo que for EF

```csharp
SaveChangesAsync
Update
Add
Remove
FindAsync
using Microsoft.EntityFrameworkCore
```

---

# ⚠️ ERROS COMUNS E SOLUÇÕES

---

## ❌ 1. ConnectionString null

### Causa:

* Nome errado no JSON ou classe

### Solução:

```json
"MongoSettings"
```

```csharp
public string ConnectionString { get; set; }
public string DatabaseName { get; set; }
```

---

## ❌ 2. Unable to resolve service

### Causa:

ContextMongoDb não registrado

### Solução:

```csharp
builder.Services.AddSingleton<ContextMongoDb>();
```

---

## ❌ 3. No database provider configured

### Causa:

FakeDbContext sem provider

### Solução:

```csharp
options.UseInMemoryDatabase("FakeDb")
```

---

## ❌ 4. Tipo não pode ser inferido (Find)

### Causa:

```csharp
Find(_ => true)
```

### Solução:

```csharp
Find(x => true)
```

ou:

```csharp
Builders<Cliente>.Filter.Empty
```

---

## ❌ 5. FindAsync não existe

### Solução:

```csharp
Find(...).FirstOrDefaultAsync()
```

---

## ❌ 6. SaveChanges usado com Mongo

### Solução:

REMOVER

---

## ❌ 7. Id null no Create

### Solução:

```csharp
ModelState.Remove("Id");
```

---

# 🧠 Resumo para Prova

👉 EF serve só para gerar código
👉 Mongo faz o CRUD real

### Regra principal:

```text
Tudo que acessa banco → trocar por Mongo
```

---

# 🏁 Resultado Final

✔ Scaffold funcionando
✔ CRUD completo
✔ MongoDB integrado
✔ Código compatível com a prova

---

# 💡 Dica Final

Se travar na prova:

1. Gera scaffold
2. Substitui método por método
3. Remove EF
4. Aplica Mongo

👉 Simples, rápido e funciona sempre.
