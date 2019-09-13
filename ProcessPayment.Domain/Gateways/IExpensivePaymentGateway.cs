using System;
using System.Threading.Tasks;
using ProcessPayment.Common;
using ProcessPayment.Model;

namespace ProcessPayment.Domain
{
	public interface IExpensivePaymentGateway : IPaymentGateway { }

	public class ExpensivePaymentGateway : PaymentGateway, IExpensivePaymentGateway
	{
		/// <inheritdoc />
		public ExpensivePaymentGateway(PaymentGateway nextGateway)
			: base(nextGateway) { }

		/// <inheritdoc />
		public override bool CanProcessPayment(Payment payment)
		{
			return payment.Amount > 20 && payment.Amount < 500;
		}

		/// <inheritdoc />
		public override async Task<PaymentStatus> HandlePayment(Payment payment)
		{
			return await RetryPolicies.CheapGatewayRetryPolicy
				.ExecuteAsync(processPayment);
		}

		Task<PaymentStatus> processPayment()
		{
			var time = DateTime.Now;

			if ((time.Second % 7) == 0)
				throw new PaymentGatewayNotAvailableException();

			return Task.FromResult(new PaymentStatus()
			{
				State = PaymentStateEnum.Processed,
				Message = $"Processed by {GetType().Name}"
			});
		}
	}
}