using FanficNewApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanficNewApplication.ViewModels
{
    public class FanficsOfFandomViewModel
    {
        public Fandom Fandom { get; set; }
        public List<Fanfic> Fanfics { get; set; }
    }
}
