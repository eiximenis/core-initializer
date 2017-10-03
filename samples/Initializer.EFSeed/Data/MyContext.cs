using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Initializer.EFSeed.Data
{
    public class MyContext : DbContext
    {

        public DbSet<MyEntity> MyEntities { get; set; }

        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }
    }
}
