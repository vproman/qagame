using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using QAGame.DataModel;
using QAGame.Service.Models;

namespace QAGame.Service.Controllers
{
    public class CustomQuestionsController : ApiController
    {
        private qagameContext db = new qagameContext();

        // GET: api/CustomQuestions
        public IQueryable<CustomQuestion> GetCustomQuestions()
        {
            return db.CustomQuestions;
        }

        // GET: api/CustomQuestions/5
        [ResponseType(typeof(CustomQuestion))]
        public async Task<IHttpActionResult> GetCustomQuestion(int id)
        {
            CustomQuestion customQuestion = await db.CustomQuestions.FindAsync(id);
            if (customQuestion == null)
            {
                return NotFound();
            }

            return Ok(customQuestion);
        }

        // PUT: api/CustomQuestions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCustomQuestion(int id, CustomQuestion customQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customQuestion.Id)
            {
                return BadRequest();
            }

            db.Entry(customQuestion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomQuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CustomQuestions
        [ResponseType(typeof(CustomQuestion))]
        public async Task<IHttpActionResult> PostCustomQuestion(CustomQuestion customQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CustomQuestions.Add(customQuestion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = customQuestion.Id }, customQuestion);
        }

        // DELETE: api/CustomQuestions/5
        [ResponseType(typeof(CustomQuestion))]
        public async Task<IHttpActionResult> DeleteCustomQuestion(int id)
        {
            CustomQuestion customQuestion = await db.CustomQuestions.FindAsync(id);
            if (customQuestion == null)
            {
                return NotFound();
            }

            db.CustomQuestions.Remove(customQuestion);
            await db.SaveChangesAsync();

            return Ok(customQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomQuestionExists(int id)
        {
            return db.CustomQuestions.Count(e => e.Id == id) > 0;
        }
    }
}