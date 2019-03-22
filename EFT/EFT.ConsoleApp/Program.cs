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

            var oij = db.Students.AsNoTracking().Between(DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeMilliseconds(), DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()).Where(d => d.Id == 1).ToList();
        }
    }
}
