using Cqrs.Domain;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class CurrentAccountDbContext : DbContext
    {
        public DbSet<Account> Account { get; set; }
        public DbSet<Movements> Movements { get; set; }

        public CurrentAccountDbContext(DbContextOptions options) : base(options)
        {
        }

        protected CurrentAccountDbContext()
        {
        }
    }
}
