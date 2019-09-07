using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProcessPayment.Model
{
	public class PaymentStateConfiguration : IEntityTypeConfiguration<PaymentState>
	{
		public void Configure(EntityTypeBuilder<PaymentState> builder)
		{
			builder.ToTable("PaymentStates");

			builder.Property(e => e.Id)
				.ValueGeneratedNever();

			builder.HasKey(e => e.Id);
			builder.Property(e => e.Name)
				.IsRequired()
				.IsUnicode();

			builder.HasData(Extensions.PaymentStateAsEntities());
		}
	}
}