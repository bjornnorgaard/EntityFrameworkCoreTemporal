using EFT.Domain;
using EFT.Repository.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EFT.Repository
{
    public class EftContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFT_Local;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.SetupTemporalEntities<Student>();
        }
    }
}
