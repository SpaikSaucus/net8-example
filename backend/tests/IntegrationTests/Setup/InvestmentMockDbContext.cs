using Invest.Domain.Portfolio.Models;
using Invest.Domain.User.Models;
using Invest.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Setup
{
	public class InvestmentMockDbContext : InvestmentDbContext
	{
		public InvestmentMockDbContext(DbContextOptions options) : base(options)
		{
			this.Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var user in UsersMock.Get)
			{
				modelBuilder.Entity<User>().HasData(
					new User(user.UUID, user.UserName, user.Password, user.FirstName, user.LastName, user.Email)
				);
			}

			foreach (var portfolio in PortfoliosMock.Get)
			{
				modelBuilder.Entity<Portfolio>().HasData(
					new Portfolio()
					{
						Id = portfolio.Id,
						UserGuid = portfolio.UserGuid,
						Name = portfolio.Name,
						Created = portfolio.Created
					}
				);
			}

			base.OnModelCreating(modelBuilder);
		}
	}
}
