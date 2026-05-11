[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/6QACnjim)
[![Open in Codespaces](https://classroom.github.com/assets/launch-codespace-2972f46106e565e64193e422d61a12cf1da4916b45550586e14ef0a7c637dd04.svg)](https://classroom.github.com/open-in-codespaces?assignment_repo_id=23854640)
# SESION DE LABORATORIO N° 02: Consumiendo datos de una base de datos Microsoft SQL Server

## OBJETIVOS
  * Comprender el funcionamiento de una aplicación que consume una base de datos relacional contenerizada.

## REQUERIMIENTOS
  * Software:
    - Docker Desktop 
    - .Net 7.0

## DESARROLLO

1. **Base de Datos**: Crear carpeta `db` y el archivo `clientes.sql`.
2. **Configuración Docker**: Crear `docker-compose.yaml` con el servicio `bd` (SQL Server).
3. **Inicialización**: Ejecutar el script SQL en el contenedor para crear las tablas `CLIENTES`, `TIPOS_DOCUMENTOS` y `CLIENTES_DOCUMENTOS`.
4. **Creación de API**: Crear proyecto `webapi` llamado `ClienteAPI` e instalar paquetes de EF Core.
5. **Scaffolding**: Importar la estructura de la base de datos al código mediante `dotnet ef dbcontext scaffold`.
6. **Controladores**: Generar controladores para exponer las entidades mediante una API REST.

---

## Actividades Encargadas

### 1. Código de los controladores para Cliente y ClientesDocumento

#### ClientesController.cs
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClienteAPI.Data;
using ClienteAPI.Models;

namespace ClienteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly BdClientesContext _context;

        public ClientesController(BdClientesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return cliente;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente) return BadRequest();
            _context.Entry(cliente).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clientes.Any(e => e.IdCliente == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCliente", new { id = cliente.IdCliente }, cliente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
```

#### ClientesDocumentosController.cs
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClienteAPI.Data;
using ClienteAPI.Models;

namespace ClienteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesDocumentosController : ControllerBase
    {
        private readonly BdClientesContext _context;

        public ClientesDocumentosController(BdClientesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientesDocumento>>> GetClientesDocumentos()
        {
            return await _context.ClientesDocumentos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientesDocumento>> GetClientesDocumento(int id)
        {
            var doc = await _context.ClientesDocumentos.FindAsync(id);
            if (doc == null) return NotFound();
            return doc;
        }

        [HttpPost]
        public async Task<ActionResult<ClientesDocumento>> PostClientesDocumento(ClientesDocumento doc)
        {
            _context.ClientesDocumentos.Add(doc);
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException)
            {
                if (_context.ClientesDocumentos.Any(e => e.IdCliente == doc.IdCliente)) return Conflict();
                else throw;
            }
            return CreatedAtAction("GetClientesDocumento", new { id = doc.IdCliente }, doc);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientesDocumento(int id)
        {
            var doc = await _context.ClientesDocumentos.FindAsync(id);
            if (doc == null) return NotFound();
            _context.ClientesDocumentos.Remove(doc);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
```
---

### Dockerfile (ClienteAPI)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ClienteAPI.csproj", "."]
RUN dotnet restore "./ClienteAPI.csproj"
COPY . .
RUN dotnet build "ClienteAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClienteAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClienteAPI.dll"]
```
