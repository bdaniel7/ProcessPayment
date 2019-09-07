using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace ProcessPayment.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Run();
		}

		public static IWebHost CreateWebHostBuilder(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseSerilog((ctx, cfg) =>
				{
					cfg.Enrich.FromLogContext()
						.MinimumLevel.Is(LogEventLevel.Debug)
						.Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
						.Enrich.WithProperty("ApplicationName", "Payments")
						.WriteTo.RollingFile($"log/payments-api.txt");
				})
				.Build();
		}
	}
}