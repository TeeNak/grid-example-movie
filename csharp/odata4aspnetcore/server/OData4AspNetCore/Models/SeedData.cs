using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OData4AspNetCore.Models
{
    public static class SeedData
    {

        public static IList<Movie> GetMovies()
        {

            //var list = new List<Movie>();

            //list.Add(new Movie
            //{
            //    Id = 1,
            //    Code = "AB0001",
            //    Name = "Star Wars"
            //});

            //return list;

            Assembly assembly = Assembly.GetExecutingAssembly();

            string resourceName = "OData4AspNetCore.Models.SeedData.Movies.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var csvReader = new CsvReader(reader);
                    csvReader.Configuration.RegisterClassMap<MovieCsvMapper>();
                    var movies = csvReader.GetRecords<Movie>().ToArray();
                    return movies;
                }
            }

        }
    }
}
