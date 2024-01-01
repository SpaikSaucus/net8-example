using Invest.Domain.Portfolio.Models;
using Invest.Domain.User.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Reflection;

namespace Invest.Infrastructure.EF
{
    public class InvestmentDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<InvestmentDbContext>
    {
        public InvestmentDbContext CreateDbContext(string[] args)
        {
            if (args == null || args.Length != 1)
                throw new Exception("ConnectionString is not found");

            var connectionString = args[0];
            var builder = new DbContextOptionsBuilder<InvestmentDbContext>();
            builder.UseMySQL(connectionString);
            return new InvestmentDbContext(builder.Options);
        }
    }
}
