using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortenerDAL.EF
{
    public class UrlShortenerContextFactory : IDesignTimeDbContextFactory<UrlShortenerContext>
    {
        public UrlShortenerContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<UrlShortenerContext> optionsBuilder = new DbContextOptionsBuilder<UrlShortenerContext>();
            string connectionString = @"server=.\SQLEXPRESS;database=UrlShortenerDB; integrated security=True; MultipleActiveResultSets=True;App=EntityFramework;";
            //config.GetConnectionString("optimumDB")
            optionsBuilder.UseSqlServer(connectionString);
            return new UrlShortenerContext(optionsBuilder.Options);
        }
    }
}
