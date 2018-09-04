using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using OData4AspNetCore.Models;

namespace OData4AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ODataController
    {
        private MoviesContext _db;

        public BooksController(MoviesContext context)
        {
            _db = context;
            if (context.Books.Count() == 0)
            {
                foreach (var b in SeedData.GetBooks())
                {
                    context.Books.Add(b);
                    context.Press.Add(b.Press);
                }
                context.SaveChanges();
            }
        }

        [EnableQuery]
        // http://localhost:54701/odata/Books
        public IActionResult Get()
        {
            return Ok(_db.Books);
        }

        [EnableQuery]
        public IActionResult Get([FromODataUri] int key)
        {
            return Ok(_db.Books.FirstOrDefault(c => c.Id == key));
        }

        [EnableQuery]
        public IActionResult Post([FromBody]Book book)
        {
            _db.Books.Add(book);
            _db.SaveChanges();
            return Created(book);
        }
    }
}
