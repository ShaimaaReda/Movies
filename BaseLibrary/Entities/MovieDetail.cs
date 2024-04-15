using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class MovieDetail
    {
        public Guid Id{ get; set; }
        public string Description { get; set; }
        public byte[] Poster { get; set; }
        public string Actor { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
