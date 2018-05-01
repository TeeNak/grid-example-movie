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

        // http://localhost:40221/Movies
        [EnableQuery]
        public IQueryable<Movie> Get()
        {
            return db.Movies;
        }

        // http://localhost:40221/Movies(1)
        [EnableQuery]
        public SingleResult<Movie> Get([FromODataUri] int key)
        {
            IQueryable<Movie> result = db.Movies.Where(p => p.Id == key);
            return SingleResult.Create(result);
        }

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

            db.Movies.AddOrUpdate(c => c.Id, movies.ToArray());
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}