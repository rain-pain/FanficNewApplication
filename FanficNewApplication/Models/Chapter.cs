using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FanficNewApplication.Models
{
    public class Chapter
    {
        public int Id { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }
        public int FanficId { get; set; }
        public Fanfic Fanfic { get; set; }
    }
}
