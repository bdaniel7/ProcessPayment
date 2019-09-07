using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProcessPayment.Model
{
	public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
	{
		public void Configure(EntityTypeBuilder<Payment> builder)
		{
			builder.ToTable("Payments");

			builder.Property(e => e.Id)
				.ValueGeneratedOnAdd()
				.HasAnnotation("SqlServer:ValueGenerationStrategy",
				SqlServerValueGenerationStrategy.IdentityColumn);

			builder.HasKey(e => e.Id);

			builder.Property(e => e.CreditCardNumber)
				.IsRequired();

			builder.Property(e => e.CardHolder)
				.IsRequired()
				.IsUnicode();

			builder.Property(e => e.ExpirationDate)
				.IsRequired();

			builder.Property(e => e.SecurityCode)
				.HasMaxLength(3);

			builder.Property(e => e.Amount)
				.IsRequired();

			builder.Property(e => e.Status)
				.IsRequired()
				.IsUnicode()
				.HasDefaultValueSql("('')");

			builder.HasOne(e => e.State);
		}
	}
}