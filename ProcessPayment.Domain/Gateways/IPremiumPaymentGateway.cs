using System;
using System.Threading.Tasks;
using ProcessPayment.Common;
using ProcessPayment.Model;

namespace ProcessPayment.Domain
{
	public interface IPremiumPaymentGateway : IPaymentGateway { }

	public class PremiumPaymentGateway : IPremiumPaymentGateway
	{
		/// <inheritdoc />
		public async Task<PaymentStatus> ProcessPayment(Payment payment)
		{
			return await RetryPolicies.PremiumGatewayRetryPolicy.ExecuteAsync(processPayment);
		}

		Task<PaymentStatus> processPayment()
		{
			Random rand = new Random();
			int num = rand.Next(1,3);

			if (num == 2)
				throw new PaymentNotProcessedException();

			return Task.FromResult(new PaymentStatus()
			{
				State = PaymentStateEnum.Processed,
				Message = $"Processed by {GetType().Name}"
			});
		}
	}
}