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
    [Route("api/Titles")]
    public class TitlesController : Controller
    {
        private readonly FlashCards2Context _context;

        public TitlesController(FlashCards2Context context)
        {
            _context = context;
        }

        // GET: api/Titles?DomainID=1
        [HttpGet]
        public IEnumerable<Titles> GetTitlesByDomain([FromQuery] int DomainId)
        {
            return _context.Titles.Where(m => m.DomainId == DomainId);
        }

        // GET: api/Titles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTitles([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var titles = await _context.Titles.SingleOrDefaultAsync(m => m.TitleId == id);

            if (titles == null)
            {
                return NotFound();
            }

            return Ok(titles);
        }

        // PUT: api/Titles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTitles([FromRoute] int id, [FromBody] Titles titles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != titles.TitleId)
            {
                return BadRequest();
            }

            _context.Entry(titles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TitlesExists(id))
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

        // POST: api/Titles
        [HttpPost]
        public async Task<IActionResult> PostTitles([FromBody] Titles titles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Titles.Add(titles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTitles", new { id = titles.TitleId }, titles);
        }

        // DELETE: api/Titles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTitles([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var titles = await _context.Titles.SingleOrDefaultAsync(m => m.TitleId == id);
            if (titles == null)
            {
                return NotFound();
            }

            _context.Titles.Remove(titles);
            await _context.SaveChangesAsync();

            return Ok(titles);
        }

        private bool TitlesExists(int id)
        {
            return _context.Titles.Any(e => e.TitleId == id);
        }
    }
}