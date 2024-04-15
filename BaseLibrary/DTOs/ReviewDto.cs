using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class ReviewDto
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
    }
}
