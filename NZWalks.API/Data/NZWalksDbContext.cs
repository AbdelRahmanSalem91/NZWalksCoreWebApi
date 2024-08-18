using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<Walk>  Walks { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
    }
}
