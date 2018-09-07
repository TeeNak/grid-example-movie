using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OData4AspNetCore.Models
{
    public static class SeedData
    {

        public static IList<Movie> GetMovies()
        {

            var list = new List<Movie>();

            list.Add(new Movie
            {
                Id = 1,
                Code = "AB0001",
                Name = "Star Wars"
            });

            return list;
        }
    }
}
