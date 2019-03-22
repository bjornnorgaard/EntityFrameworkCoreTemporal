using System.Collections.Generic;
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

            var students = new List<Student>
            {
                new Student{Name = "John Doe"},
                new Student{Name = "Simon Says"}
            };

            db.Students.AddRange(students);
            db.SaveChanges();
        }
    }
}
