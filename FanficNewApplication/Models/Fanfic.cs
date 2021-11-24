using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FanficNewApplication.Models
{
    public class Fanfic
    {
        public int FanficId { get; set; }
        public string FanficName { get; set; }

        [Column(TypeName = "text")]
        public string ShortDescription { get; set; }
            
        public int FandomId { get; set; }
        public Fandom Fandom { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public ICollection<Chapter> Chapter { get; set; }
    }
}
