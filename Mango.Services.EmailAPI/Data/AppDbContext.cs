using Mango.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Mango.Services.EmailAPI.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<EmailLogger> emailLoggers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }
    }
}
