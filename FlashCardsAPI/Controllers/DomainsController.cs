using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlashCardsAPI.Models.DB;

namespace FlashCardsAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Domains")]
    public class DomainsController : Controller
    {
        private readonly FlashCards2Context _context;

        public DomainsController(FlashCards2Context context)
        {
            _context = context;
        }

        // GET: api/Domains
        [HttpGet]
        public IEnumerable<Domains> GetDomains()
        {
            return _context.Domains;
        }

        // GET: api/Domains/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDomains([FromRoute] byte id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domains = await _context.Domains.SingleOrDefaultAsync(m => m.DomainId == id);

            if (domains == null)
            {
                return NotFound();
            }

            return Ok(domains);
        }

        // PUT: api/Domains/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDomains([FromRoute] byte id, [FromBody] Domains domains)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != domains.DomainId)
            {
                return BadRequest();
            }

            _context.Entry(domains).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DomainsExists(id))
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

        // POST: api/Domains
        [HttpPost]
        public async Task<IActionResult> PostDomains([FromBody] Domains domains)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Domains.Add(domains);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DomainsExists(domains.DomainId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDomains", new { id = domains.DomainId }, domains);
        }

        // DELETE: api/Domains/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDomains([FromRoute] byte id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domains = await _context.Domains.SingleOrDefaultAsync(m => m.DomainId == id);
            if (domains == null)
            {
                return NotFound();
            }

            _context.Domains.Remove(domains);
            await _context.SaveChangesAsync();

            return Ok(domains);
        }

        private bool DomainsExists(byte id)
        {
            return _context.Domains.Any(e => e.DomainId == id);
        }
    }
}