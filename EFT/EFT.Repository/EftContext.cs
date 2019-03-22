using EFT.Domain;
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
    }
}
