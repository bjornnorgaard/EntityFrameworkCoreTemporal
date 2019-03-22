using System;
using System.Linq;
using EFT.Repository;
using EFT.Repository.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EFT.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new EftContext();
            db.Database.Migrate();

            var allRevisions = db.Students.AsNoTracking()
                .Between(DateTime.MinValue, DateTime.Now)
                .Where(s => s.Id == 1)
                .ToList();
        }
    }
}
