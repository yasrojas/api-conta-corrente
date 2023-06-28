using Cqrs.Domain;
using Domain;
using Domain.Abstractions;
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
    public class CurrentAccountDbContext : DbContext, ICurrentAccountDbContext
    {
        public DbSet<Account> Account { get; set; }
        public DbSet<Movements> Movements { get; set; }

        public CurrentAccountDbContext(DbContextOptions<CurrentAccountDbContext> options) : base(options)
        {
        }
        public CurrentAccountDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(Console.WriteLine);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
