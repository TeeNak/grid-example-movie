using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Odata4AspNet.Models
{
    public class MoviesContext : DbContext
    {
        public MoviesContext()
                : base("name=MoviesContext")
        {
            Database.SetInitializer<MoviesContext>(new MoviesContextInitializer());
            Database.Initialize(false);
        }
        public DbSet<Movie> Movies { get; set; }
    }
}