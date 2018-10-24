using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OData4AspNetCore.Models
{
    [Table("Movie")]
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public sealed class MovieCsvMapper : ClassMap<Movie>
    {
        public MovieCsvMapper()
        {
            AutoMap();
            Map(m => m.RowVersion).Ignore();
        }
    }
}
