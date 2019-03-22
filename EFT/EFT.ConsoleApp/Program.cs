using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EFT.Domain;
using EFT.Repository;
using EFT.Repository.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EFT.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Clearing and creating database... ");
            var db = new EftContext();
            db.Database.EnsureDeleted();
            db.Database.Migrate();
            Console.WriteLine("Done");

            Console.Write("Seeding database... ");
            var students = new List<Student>
            {
                new Student{Name = "John Doe"},
                new Student{Name = "Simon Says"},
                new Student{Name = "Kasper Skye"},
            };

            db.Students.AddRange(students);
            db.SaveChanges();
            Console.WriteLine("Done");

            Console.Write("Changing first student... ");
            var student = db.Students.FirstOrDefault();
            student.Name = "Mister Sir";
            db.SaveChanges();
            Console.WriteLine("Done");

            Console.Write("Fetching revisions of first student... ");
            var allRevisions = db.Students.AsNoTracking()
                .Between(DateTime.MinValue, DateTime.Now)
                .Where(s => s.Id == 1)
                .ToList();
            Console.WriteLine("Done");

            Console.WriteLine("");
            foreach (var r in allRevisions)
            {
                Console.WriteLine($"Id: {r.Id}, Name: {r.Name}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
