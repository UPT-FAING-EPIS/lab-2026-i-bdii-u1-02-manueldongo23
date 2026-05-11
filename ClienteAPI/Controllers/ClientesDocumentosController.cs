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

        // GET: api/ClientesDocumentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientesDocumento>>> GetClientesDocumentos()
        {
            return await _context.ClientesDocumentos.ToListAsync();
        }

        // GET: api/ClientesDocumentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientesDocumento>> GetClientesDocumento(int id)
        {
            var clientesDocumento = await _context.ClientesDocumentos.FindAsync(id);

            if (clientesDocumento == null)
            {
                return NotFound();
            }

            return clientesDocumento;
        }

        // PUT: api/ClientesDocumentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientesDocumento(int id, ClientesDocumento clientesDocumento)
        {
            if (id != clientesDocumento.IdCliente)
            {
                return BadRequest();
            }

            _context.Entry(clientesDocumento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientesDocumentoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ClientesDocumentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClientesDocumento>> PostClientesDocumento(ClientesDocumento clientesDocumento)
        {
            _context.ClientesDocumentos.Add(clientesDocumento);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClientesDocumentoExists(clientesDocumento.IdCliente))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClientesDocumento", new { id = clientesDocumento.IdCliente }, clientesDocumento);
        }

        // DELETE: api/ClientesDocumentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientesDocumento(int id)
        {
            var clientesDocumento = await _context.ClientesDocumentos.FindAsync(id);
            if (clientesDocumento == null)
            {
                return NotFound();
            }

            _context.ClientesDocumentos.Remove(clientesDocumento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientesDocumentoExists(int id)
        {
            return _context.ClientesDocumentos.Any(e => e.IdCliente == id);
        }
    }
}
