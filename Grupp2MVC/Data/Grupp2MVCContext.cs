using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Grupp2MVC.Models;

namespace Grupp2MVC.Data
{
    public class Grupp2MVCContext : DbContext
    {
        public Grupp2MVCContext (DbContextOptions<Grupp2MVCContext> options)
            : base(options)
        {
        }

        public DbSet<Grupp2MVC.Models.Vehicle> Vehicle { get; set; } = default!;
    }
}
