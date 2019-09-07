using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProcessPayment.Model
{
	/// <summary>
	/// Provides an initialized and configured DbContext. It's used also by the migrations.
	/// </summary>
	public class DbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
	{
		public PaymentDbContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var builder = new DbContextOptionsBuilder<PaymentDbContext>();

			var connectionString = configuration.GetConnectionString("ProcessPayment");

			builder.UseSqlServer(connectionString,
				sql =>
				{
					sql.MigrationsAssembly(typeof(PaymentDbContext).Assembly.GetName().Name);
				});

			return new PaymentDbContext(builder.Options);
		}
	}
}