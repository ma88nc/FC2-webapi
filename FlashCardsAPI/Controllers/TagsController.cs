using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlashCardsAPI.Models;
using FlashCardsAPI.Models.DB;
using System.Net.Http;
using System.Net;

namespace FlashCardsAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Tags")]
    public class TagsController : Controller
    {
        private readonly FlashCards2Context _context;

        public TagsController(FlashCards2Context context)
        {
            _context = context;
        }

        // GET: api/Tags?DomainId=1
        [HttpGet]
        public IEnumerable<Tags> GetAllTags([FromQuery] int DomainId)
        {
            try
            {
                var tags = new Dictionary<int, Node>();
            //    var tlist = _context.Tags.Where(t => (t.DomainId == DomainId) && (t.IsActive == true)).Select(t => new Node()
                //{
                //    Description = t.TagDescription,
                //    //   DomainId = DomainId,
                //    Id = t.TagId,
                //    ParentId = t.ParentTagId.HasValue ? t.ParentTagId.Value : 0,
                //}).ToList();

                //tags = tlist
                //.ToDictionary(x => x.Id);

                //foreach (Node node in tags.Values)
                //{
                //    if (node.ParentId != 0)
                //    {
                //        node.Parent = tags[node.ParentId];
                //        node.Parent.Children.Add(node);
                //    }
                //}

                //List<Node> lstNodes = tags.Values.ToList();
                ////      return new JsonResult(@"[{""parent"":null,""parentId"":0,""id"":1,""description"":""Pop Culture"",""children"":[]},{""parent"":null,""parentId"":0,""id"":3,""description"":""World Religions"",""children"":[]}]");
                     return _context.Tags.Where(t => (t.DomainId == DomainId) && (t.IsActive == true));
                ////   return lstNodes;
                //HttpRequestMessage request = new HttpRequestMessage();
                //return request.CreateResponse(HttpStatusCode.OK, lstNodes);
            }
            catch (Exception ex)
            {
                return null;
            }            
        }

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTags([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tags = await _context.Tags.SingleOrDefaultAsync(m => (m.TagId == id) && (m.IsActive == true));

            if (tags == null)
            {
                return NotFound();
            }

            return Ok(tags);
        }

        // GET: api/Tags?QuestionID=FBFF6607-24C8-47F8-93FA-4FE431EAAD09
         [HttpGet("Questions/{QuestionID}")]
       // [HttpGet]
         public IEnumerable<Tags> GetTags([FromRoute] Guid QuestionID)
        {
            var query =
                from tags in _context.Tags
                join qtags in _context.QuestionTags on tags.TagId equals qtags.TagId
                where qtags.QuestionId == QuestionID
                select new Tags { TagId = qtags.TagId, TagDescription = tags.TagDescription };
            //Where(t => t.QuestionTags.Where(q => q.QuestionId == QuestionID));

            return query;
        }

        // PUT: api/Tags/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTags([FromRoute] int id, [FromBody] Tags tags)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tags.TagId)
            {
                return BadRequest();
            }

            _context.Entry(tags).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagsExists(id))
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

        // POST: api/Tags
        [HttpPost]
        public async Task<IActionResult> PostTags([FromBody] Tags tags)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Tags.Add(tags);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTags", new { id = tags.TagId }, tags);
        }

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTags([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tags = await _context.Tags.SingleOrDefaultAsync(m => m.TagId == id);
            if (tags == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tags);
            await _context.SaveChangesAsync();

            return Ok(tags);
        }

        private bool TagsExists(int id)
        {
            return _context.Tags.Any(e => e.TagId == id);
        }

        //private List<Tags> subtree(Tags tag, List<Tags> lstTags)
        //{

        //}
    }
}