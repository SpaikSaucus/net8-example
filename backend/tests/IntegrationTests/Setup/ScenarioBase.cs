using Autofac.Extensions.DependencyInjection;
using Invest.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

namespace IntegrationTests.Setup
{
	public class ScenarioBase
	{
		public TestServer CreateServer()
		{
			var path = Assembly.GetAssembly(typeof(ScenarioBase)).Location;
			var host = Host.CreateDefaultBuilder()
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					//TODO : Check why is not working launchSettings.json in IntegrationTests Project.
					Environment.SetEnvironmentVariable("JWT.SymmetricKey", "6tdchcn3ihuc98h874xhnmm8inixu27rx5431xwzieicjaq098");

					config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
						.AddJsonFile($"appsettings.Local.json", optional: true, reloadOnChange: true);
					config.AddEnvironmentVariables();
				})
				.UseServiceProviderFactory(new AutofacServiceProviderFactory())
				.ConfigureWebHostDefaults(webHostBuilder =>
				{
					webHostBuilder
						.UseTestServer()
						.UseContentRoot(Path.GetDirectoryName(path))
						.UseStartup<TestsStartup>();
				})
				.Build();

			host.Start();
			return host.GetTestServer();
		}

		public static class Get
		{
			public const string Portfolios = "portfolios/";
		}

		public static class Post
		{
			public const string Portfolios = "portfolios/";
			public const string Login = "login/";
		}
	}
}
