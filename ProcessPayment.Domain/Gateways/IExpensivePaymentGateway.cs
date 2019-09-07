using System;
using System.Threading.Tasks;
using ProcessPayment.Common;
using ProcessPayment.Model;

namespace ProcessPayment.Domain
{
	public interface IExpensivePaymentGateway : IPaymentGateway { }

	public class ExpensivePaymentGateway : IExpensivePaymentGateway
	{
		/// <inheritdoc />
		public async Task<PaymentStatus> ProcessPayment(Payment payment)
		{
			return await RetryPolicies.CheapGatewayRetryPolicy
									.ExecuteAsync(processPayment);
		}

		Task<PaymentStatus> processPayment()
		{
			Random rand = new Random();
			int num = rand.Next(1,2);

			if (num == 2)
				throw new PaymentGatewayNotAvailableException();

			return Task.FromResult(new PaymentStatus()
			{
				State = PaymentStateEnum.Processed,
				Message = $"Processed by {GetType().Name}"
			});
		}
	}
}