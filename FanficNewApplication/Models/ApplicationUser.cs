using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanficNewApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Fanfic> Fanfic { get; set; }
    }
}   
