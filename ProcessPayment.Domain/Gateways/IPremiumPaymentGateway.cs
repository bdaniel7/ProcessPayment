using System;
using System.Threading.Tasks;
using ProcessPayment.Common;
using ProcessPayment.Model;

namespace ProcessPayment.Domain
{
	public interface IPremiumPaymentGateway : IPaymentGateway { }

	public class PremiumPaymentGateway : PaymentGateway, IPremiumPaymentGateway
	{
		/// <inheritdoc />
		public PremiumPaymentGateway(PaymentGateway nextGateway)
			: base(nextGateway) { }

		/// <inheritdoc />
		public override bool CanProcessPayment(Payment payment)
		{
			return payment.Amount > 500;
		}

		/// <inheritdoc />
		public override async Task<PaymentStatus> HandlePayment(Payment payment)
		{
			return await RetryPolicies.PremiumGatewayRetryPolicy
				.ExecuteAsync(processPayment);
		}

		Task<PaymentStatus> processPayment()
		{
			var time = DateTime.Now;

			if ((time.Second % 5) == 0)
				throw new PaymentNotProcessedException();

			return Task.FromResult(new PaymentStatus()
			{
				State = PaymentStateEnum.Processed,
				Message = $"Processed by {GetType().Name}"
			});
		}
	}
}