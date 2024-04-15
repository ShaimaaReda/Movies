using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public MovieDetail Details { get; set; }
        public ICollection<Review> Reviews { get; set;}
    }
}
