using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dbolilyi.Models
{
    public class DbolilyiContext : DbContext
    {
        public DbolilyiContext (DbContextOptions<DbolilyiContext> options)
            : base(options)
        {
        }

        public DbSet<Dbolilyi.Models.User> User { get; set; }
    }
}
