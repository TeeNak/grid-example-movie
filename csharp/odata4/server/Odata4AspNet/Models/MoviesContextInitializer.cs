using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Odata4AspNet.Models
{
    public class MoviesContextInitializer
        : DropCreateDatabaseIfModelChanges<MoviesContext>
    {
        protected override void Seed(Odata4AspNet.Models.MoviesContext context)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();

            string resourceName = "Odata4AspNet.Models.SeedData.Movies.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    var movies = csvReader.GetRecords<Movie>().ToArray();
                    context.Movies.AddOrUpdate(c => c.Id, movies);
                }
            }

        }

    }
}