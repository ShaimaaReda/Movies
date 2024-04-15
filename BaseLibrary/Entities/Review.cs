using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
        public ApplicationUser User { get; set; }
    }
}
