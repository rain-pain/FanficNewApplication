using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FanficNewApplication.Models;

namespace FanficNewApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FanficNewApplication.Models.Fanfic> Fanfic { get; set; }
        public DbSet<FanficNewApplication.Models.Fandom> Fandom { get; set; }
        public DbSet<FanficNewApplication.Models.Chapter> Chapter { get; set; }
    }
}
