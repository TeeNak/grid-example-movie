using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
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

        [EnableQuery]
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

                _db.Movies.Add(movie);
                _db.SaveChanges();
                return Created(movie);
            }
            catch
            {
                throw;
            }
        }
    }
}
