using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OData4AspNetCore.Models
{
    public class MoviesContext : DbContext
    {
        public MoviesContext(DbContextOptions<MoviesContext> options)
          : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<Press> Press { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //proper way to seed
            modelBuilder.Entity<Movie>().HasData(new Movie { Id = 1, Code = "AB0001", Name = "Star Wars" });

            modelBuilder.Entity<Book>().OwnsOne(c => c.Location);
        }
    }
}
