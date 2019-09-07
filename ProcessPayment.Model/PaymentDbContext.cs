using Microsoft.EntityFrameworkCore;

namespace ProcessPayment.Model
{
	public class PaymentDbContext : DbContext
	{
		public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base (options)
		{

		}

		public virtual DbSet<Payment> Payments { get; set; }
		public virtual DbSet<PaymentState> PaymentStates { set; get; }


		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurations<PaymentDbContext>();
		}
	}
}