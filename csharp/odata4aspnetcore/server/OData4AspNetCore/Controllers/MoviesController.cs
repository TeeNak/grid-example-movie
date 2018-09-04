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
        // http://localhost:54701/odata/Movies
        public IActionResult Get()
        {
            return Ok(_db.Movies);
        }
    }
}
