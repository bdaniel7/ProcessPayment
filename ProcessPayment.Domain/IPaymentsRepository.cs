using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ProcessPayment.Model;

namespace ProcessPayment.Domain
{
	public interface IPaymentsRepository : IDisposable
	{
		Task<int> SavePayment(Payment payment, CancellationToken cancellationToken);
		Task<List<PaymentDto>> GetAllPayments(CancellationToken cancellationToken);
	}

	public class PaymentsRepository : IPaymentsRepository
	{
		readonly PaymentDbContext paymentDbContext;

		public PaymentsRepository(PaymentDbContext paymentDbContext)
		{
			this.paymentDbContext = paymentDbContext;
		}

		/// <inheritdoc />
		public async Task<int> SavePayment(Payment payment,
											CancellationToken cancellationToken)
		{

			paymentDbContext.Payments.Add(payment);
			return await paymentDbContext.SaveChangesAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task<List<PaymentDto>> GetAllPayments(CancellationToken cancellationToken)
		{
			return await paymentDbContext.Payments.
				ProjectToType<PaymentDto>().
				ToListAsync(cancellationToken);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			paymentDbContext?.Dispose();
		}
	}
}