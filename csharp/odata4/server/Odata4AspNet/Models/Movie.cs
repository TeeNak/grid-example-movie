using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Odata4AspNet.Models
{

    [Table("Movie")]
    public class Movie
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
    }
}