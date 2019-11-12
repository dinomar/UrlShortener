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
        //internal UrlShortenerContext() { }
        public DbSet<LinkModel> Links { get; set; }
    }
}
