using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlashCardsAPI.Models.DB;
using System.Data.SqlClient;

namespace FlashCardsAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Questions")]
    public class QuestionsController : Controller
    {
        private readonly FlashCards2Context _context;

        public QuestionsController(FlashCards2Context context)
        {
            _context = context;
        }

        // GET: api/Questions?DomainId=1
        [HttpGet]
        public IEnumerable<QuestionsDTO> GetQuestions([FromQuery] int DomainId)
        {
            //    return _context.Questions;
            var questions = from q in _context.Questions.
                            Where(q => q.DomainId == DomainId)
                            select new QuestionsDTO()
                            {
                                QuestionId = q.QuestionId,
                                QuestionText = q.QuestionText,
                                Answer = q.Answer,
                                TitleId = q.TitleId,
                                TitleDescription = q.Title.Description,
                                Reference = q.Reference,
                                HasImage = q.HasImage, 
                                ImagePath = q.ImagePath,
                                IsActive = q.IsActive,
                                IsVerified = q.IsVerified,
                                DomainId = q.DomainId,
                                TimeMultiplier = q.TimeMultiplier
                            };
            return questions;
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestions([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questions = await _context.Questions.Include(q => q.Title)
                .Select(q => 
                new QuestionsDTO()
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    Answer = q.Answer,
                    TitleId = q.TitleId,
                    TitleDescription = q.Title.Description,
                    Reference = q.Reference,
                    HasImage = q.HasImage,
                    ImagePath = q.ImagePath,
                    IsActive = q.IsActive,
                    IsVerified = q.IsVerified,
                    DomainId = q.DomainId,
                    TimeMultiplier = q.TimeMultiplier
                })
                .SingleOrDefaultAsync(m => m.QuestionId == id);

            if (questions == null)
            {
                return NotFound();
            }

            return Ok(questions);
        }

        // GET: api/Questions/Test?DomainId=1&NumberOfQuestions=6&tags=<tags><tag>3</tag><tag>22</tag></tags>
        [HttpGet("Test")]
        public IEnumerable<QuestionsDTO> GetTestQuestions([FromQuery] int DomainId, [FromQuery] string tags, [FromQuery] int NumberOfQuestions)
        {
            //    return _context.Questions;

            // Define SP parameters
            var parmNumberOfQuestions = new SqlParameter("NumberOfQuestions", NumberOfQuestions);
            var parmTags = ((tags != null) ? new SqlParameter("tags", tags) : new SqlParameter("tags", DBNull.Value));
            var parmDomainID = new SqlParameter("DomainID", DomainId);

            var test = _context.Questions
                .FromSql("EXECUTE spGetTestQuestions @NumberOfQuestions, @tags, @DomainID",
                    parmNumberOfQuestions, parmTags, parmDomainID)
                .Select(q=> new QuestionsDTO()
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    Answer = q.Answer,
                    TitleId = q.TitleId,
                    TitleDescription = q.Title.Description,
                    Reference = q.Reference,
                    HasImage = q.HasImage,
                    ImagePath = q.ImagePath,
                    IsActive = q.IsActive,
                    IsVerified = q.IsVerified,
                    DomainId = q.DomainId,
                    TimeMultiplier = q.TimeMultiplier
                }
                )
                .ToList();

            //var questions = from q in _context.Questions.
            //                Where(q => q.DomainId == DomainId)
            //                select new QuestionsDTO()
            //                {
            //                    QuestionId = q.QuestionId,
            //                    QuestionText = q.QuestionText,
            //                    Answer = q.Answer,
            //                    TitleId = q.TitleId,
            //                    TitleDescription = q.Title.Description,
            //                    Reference = q.Reference,
            //                    HasImage = q.HasImage,
            //                    ImagePath = q.ImagePath,
            //                    IsActive = q.IsActive,
            //                    IsVerified = q.IsVerified,
            //                    DomainId = q.DomainId
            //                };
            return test;
        }

        // PUT: api/Questions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestions([FromRoute] Guid id, [FromBody] Questions questions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != questions.QuestionId)
            {
                return BadRequest();
            }

            _context.Entry(questions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionsExists(id))
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

        // PUT: api/Questions
        [HttpPut]
        public async Task<IActionResult> PutQuestions([FromBody] Questions questions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != questions.QuestionId)
            //{
            //    return BadRequest();
            //}

            Guid id = questions.QuestionId;

            // Create XML tags from input tags in format <tags><tag>1</tag>...<tag>3</tag></tags>
            StringBuilder sb = new StringBuilder("<tags>");
            foreach (var tag in questions.QuestionTags)
            {
                sb.Append(string.Format("<tag>{0}</tag>", tag.TagId)); 
            }
            sb.Append("</tags>");
            string tags = sb.ToString();

            _context.Entry(questions).State = EntityState.Modified;
            var parmQuestionID = new SqlParameter("QuestionID", id);
            var parmTags = new SqlParameter("tags", tags);
            _context.Database.ExecuteSqlCommand("dbo.spInsertQuestionTags @QuestionID, @tags", parmQuestionID, parmTags);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //    return NoContent();
            //  return CreatedAtAction("GetQuestions", new { id = questions.QuestionId }, questions);
            return Ok();
        }

        // POST: api/Questions
        [HttpPost]
        public async Task<IActionResult> PostQuestions([FromBody] Questions questions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Questions.Add(questions);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestions", new { id = questions.QuestionId }, questions);
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestions([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questions = await _context.Questions.SingleOrDefaultAsync(m => m.QuestionId == id);
            if (questions == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(questions);
            await _context.SaveChangesAsync();

            return Ok(questions);
        }

        private bool QuestionsExists(Guid id)
        {
            return _context.Questions.Any(e => e.QuestionId == id);
        }
    }
}