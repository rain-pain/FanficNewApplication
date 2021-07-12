using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FanficNewApplication.Models
{
    public class Fandom
    {
        public int Id { get; set; }
        public string FandomName { get; set; }

        [Column(TypeName = "text")]
        public string ShortDescription { get; set; }
        public ICollection<Fanfic> Fanfic { get; set; }
    }
}
