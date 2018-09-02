using System;
using System.Collections.Generic;
using Odata4AspNet.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Data.Entity.Migrations;
using System.Linq.Expressions;
using System.Net.Http;

namespace Odata4AspNet.Controllers
{
    public class MoviesController : ODataController
    {
        MoviesContext db = new MoviesContext();
        private bool ItemExists(int key)
        {
            return db.Movies.Any(p => p.Id == key);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET http://localhost:40221/Movies
        [EnableQuery]
        public IQueryable<Movie> Get()
        {
            return db.Movies;
        }

        // GET http://localhost:40221/Movies(1)
        [EnableQuery]
        public SingleResult<Movie> Get([FromODataUri] int key)
        {
            IQueryable<Movie> result = db.Movies.Where(p => p.Id == key);
            return SingleResult.Create(result);
        }

        // POST http://localhost:40221/Movies/
        public async Task<IHttpActionResult> Post(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Movies.Add(movie);
            await db.SaveChangesAsync();
            return Created(movie);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Movie> movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Movies.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            movie.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }

        public async Task<IHttpActionResult> Put([FromODataUri] int key, Movie update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.Id)
            {
                return BadRequest();
            }
            db.Entry(update).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var movie = await db.Movies.FindAsync(key);
            if (movie == null)
            {
                return NotFound();
            }
            db.Movies.Remove(movie);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // http://localhost:40221/Movies/Action.UpdateAll/
        [HttpPost]
        public async Task<IHttpActionResult> UpdateAll(ODataActionParameters parameters)
        {
            var movies = parameters["value"] as IEnumerable<Movie>;

            //db.Movies.AddOrUpdate(c => c.Id, movies.ToArray());
            foreach (var m in movies)
            {
                db.Entry(m).State = EntityState.Modified;
            }

            try
            {
                await db.SaveChangesAsync();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.First();

                var errorMessage = "";
                var clientValues = (Movie)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    errorMessage =
                        "Unable to save changes. The department was deleted by another user.";
                }
                else
                {
                    var databaseValues = (Movie)databaseEntry.ToObject();


                    errorMessage += "The record you attempted to edit "
                        + "was modified by another user after you got the original value. The "
                        + "edit operation was canceled and the current values in the database "
                        + "have been displayed. Please reload. ";

                    errorMessage += Environment.NewLine + "Current value: ";
                    errorMessage += Environment.NewLine + $"Code : {databaseValues.Code}";

                    if (databaseValues.Name != clientValues.Name)
                    {
                        //    ModelState.AddModelError("Name", "Current value: "
                        //        + databaseValues.Name);
                        errorMessage += Environment.NewLine + $"Name : {databaseValues.Name}";
                    }


                }

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, errorMessage));
            }
        }

        //// http://localhost:40221/Movies/Action.UpdateAllSync/
        //[HttpPost]
        //public IHttpActionResult UpdateAllSync(ODataActionParameters parameters)
        //{

        //    var updateAllAsyncWrap = (Func<ODataActionParameters, Task<IHttpActionResult>>)(async (p) =>
        //    {
        //        return await UpdateAll(p).ConfigureAwait(false);
        //    });

        //    var task = updateAllAsyncWrap(parameters);
        //    return task.Result; // ConfigureAwait is necessary to do this.
        //}

    }
}