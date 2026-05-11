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
    public class TiposDocumentosController : ControllerBase
    {
        private readonly BdClientesContext _context;

        public TiposDocumentosController(BdClientesContext context)
        {
            _context = context;
        }

        // GET: api/TiposDocumentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TiposDocumento>>> GetTiposDocumentos()
        {
            return await _context.TiposDocumentos.ToListAsync();
        }

        // GET: api/TiposDocumentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TiposDocumento>> GetTiposDocumento(byte id)
        {
            var tiposDocumento = await _context.TiposDocumentos.FindAsync(id);

            if (tiposDocumento == null)
            {
                return NotFound();
            }

            return tiposDocumento;
        }

        // PUT: api/TiposDocumentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTiposDocumento(byte id, TiposDocumento tiposDocumento)
        {
            if (id != tiposDocumento.IdTipoDocumento)
            {
                return BadRequest();
            }

            _context.Entry(tiposDocumento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TiposDocumentoExists(id))
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

        // POST: api/TiposDocumentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TiposDocumento>> PostTiposDocumento(TiposDocumento tiposDocumento)
        {
            _context.TiposDocumentos.Add(tiposDocumento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTiposDocumento", new { id = tiposDocumento.IdTipoDocumento }, tiposDocumento);
        }

        // DELETE: api/TiposDocumentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTiposDocumento(byte id)
        {
            var tiposDocumento = await _context.TiposDocumentos.FindAsync(id);
            if (tiposDocumento == null)
            {
                return NotFound();
            }

            _context.TiposDocumentos.Remove(tiposDocumento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TiposDocumentoExists(byte id)
        {
            return _context.TiposDocumentos.Any(e => e.IdTipoDocumento == id);
        }
    }
}
