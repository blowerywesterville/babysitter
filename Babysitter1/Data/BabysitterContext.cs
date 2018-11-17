using Microsoft.EntityFrameworkCore;
using Babysitter1.Models;

namespace Babysitter1.Data
{
    public class BabysitterContext : DbContext
    {
        public BabysitterContext (DbContextOptions<BabysitterContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Job { get; set; }

        public DbSet<Family> Family { get; set; }
        public DbSet<Kid> Kid { get; set; }
        public DbSet<DefaultSystem> DefaultSystem { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
