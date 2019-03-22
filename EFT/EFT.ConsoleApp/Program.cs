using System;
using System.Collections.Generic;
using System.Linq;
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
            var db = new EftContext();
            db.Database.Migrate();

            var students = new List<Student>
            {
                new Student{Name = "John Doe"},
                new Student{Name = "Simon Says"},
                new Student{Name = "Kasper Skye"},
            };

            if (!db.Students.Any())
            {
                db.Students.AddRange(students);
                db.SaveChanges();
            }

            var student = db.Students.FirstOrDefault();
            student.Name = "Mister Sir";
            db.SaveChanges();

            var allRevisions = db.Students.AsNoTracking()
                .Between(DateTime.MinValue, DateTime.Now)
                .Where(s => s.Id == 1)
                .ToList();

            foreach (var r in allRevisions)
            {
                Console.WriteLine($"Id: {r.Id}, Name: {r.Name}");
            }

            var wait = Console.ReadKey();
        }
    }
}
