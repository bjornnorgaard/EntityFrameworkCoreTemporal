using System.Collections.Generic;
using System.Linq;
using EFT.Domain;
using EFT.Repository;
using Microsoft.EntityFrameworkCore;

namespace EFT.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new EftContext();
            db.Database.Migrate();

            var student = db.Students.FirstOrDefault();
            student.Name = "John Hoff";
            db.SaveChanges();
        }
    }
}
