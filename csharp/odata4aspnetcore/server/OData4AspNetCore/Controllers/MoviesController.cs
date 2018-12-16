using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OData4AspNetCore.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OData4AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ODataController
    {

        private MoviesContext _db;

        public MoviesController(MoviesContext context)
        {
            _db = context;
        }

        [EnableQuery]
        // GET http://localhost:54701/odata/Movies
        public IActionResult Get()
        {
            return Ok(_db.Movies);
        }

        // GET http://localhost:54701/odata/Movies(1)
        public IActionResult Get([FromODataUri]int key)
        {
            return Ok(_db.Movies.FirstOrDefault(c => c.Id == key));
        }

        // POST http://localhost:54701/odata/Movies
        // add single movie
        public IActionResult Post([FromBody]Movie movie)
        {
            try
            {
                // https://docs.microsoft.com/en-us/ef/core/modeling/generated-properties
                // If you add an entity to the context that has a value assigned to the property, 
                // then EF will attempt to insert that value rather than generating a new one.
                // A property is considered to have a value assigned if it is not assigned the CLR default value
                // (null for string, 0 for int, Guid.Empty for Guid, etc.).
                // For more information, see Explicit values for generated properties.

                movie.Id = 0;

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _db.Movies.Add(movie);
                _db.SaveChanges();
                return Created(movie);
            }
            catch
            {
                throw;
            }
        }

        private bool MovieExists(int id)
        {
            return _db.Movies.Any(x => x.Id == id);
        }

        // PATCH http://localhost:54701/odata/Movies(1)
        // the variable name "key" is default. if you change the name you need to set ODataRoutePrefix explicitly.
        public IActionResult Patch([FromODataUri] int key, [FromBody] Delta<Movie> movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _db.Movies.Find(key);
            if (entity == null)
            {
                return NotFound();
            }
            movie.Patch(entity);
            try
            {
                _db.SaveChanges();
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
                        "Unable to save changes. The record was deleted by another user.";
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

                return StatusCode(StatusCodes.Status412PreconditionFailed, errorMessage);
            }

            return Updated(entity);
        }

        // PUT http://localhost:54701/odata/Movies(1)
        public IActionResult Put([FromODataUri]int key, [FromBody] Movie update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != update.Id)
            {
                return BadRequest();
            }

            _db.Entry(update).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
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
                        "Unable to save changes. The record was deleted by another user.";
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

                return StatusCode(StatusCodes.Status412PreconditionFailed, errorMessage);


            }

            return Updated(update);
        }

        // DELETE http://localhost:54701/odata/Movies(1)
        public ActionResult Delete([FromODataUri] int key)
        {
            var movie = _db.Movies.Find(key);
            if (movie == null)
            {
                return NotFound();
            }
            _db.Movies.Remove(movie);
            _db.SaveChanges();
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        // http://localhost:54701/odata/Movies/Default.UpdateAll/
        [HttpPost]
        public IActionResult UpdateAll(ODataActionParameters parameters)
        {
            var movies = parameters["value"] as IEnumerable<Movie>;

            //db.Movies.AddOrUpdate(c => c.Id, movies.ToArray());
            foreach (var m in movies)
            {
                _db.Entry(m).State = EntityState.Modified;
            }

            try
            {
                _db.SaveChanges();
                return StatusCode((int)HttpStatusCode.NoContent);
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
                        "Unable to save changes. The record was deleted by another user.";
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

                return StatusCode(StatusCodes.Status412PreconditionFailed, errorMessage);

            }
        }


    }
}
