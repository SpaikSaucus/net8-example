using Invest.Domain.Portfolio.Models;
using System;
using System.Collections.Generic;

namespace IntegrationTests.Setup
{
	public static class PortfoliosMock
	{
        public static readonly List<Portfolio> Get = new() {
			new Portfolio(1, Guid.Parse("7eb7c8db-5b0c-4755-ba3e-283b7ad7466c"), "Portfolio1"),
			new Portfolio(2, Guid.Parse("0c39bb11-587a-4af2-80d7-4e392f92add2"), "Portfolio2"),
			new Portfolio(3, Guid.Parse("285b2373-429e-49bd-87eb-1d2fab501b48"), "Portfolio3")
		};
	}
}
