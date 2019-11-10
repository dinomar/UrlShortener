using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using UrlShortenerDAL.Models;

namespace UrlShortenerDAL.EF
{
    public class UrlShortenerContext : DbContext
    {
        public UrlShortenerContext(DbContextOptions options) : base(options) { }
        internal UrlShortenerContext() { }

        public DbSet<LinkModel> Links { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (optionsBuilder.IsConfigured)
        //    {
        //        //var connectionString = @"server=.\SQLEXPRESS;database=BoardDb; integrated security=True; MultipleActiveResultSets=True;App=EntityFramework;";

        //        //optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
        }
    }
}
