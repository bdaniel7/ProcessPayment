using System.Threading.Tasks;
using ProcessPayment.Common;
using ProcessPayment.Model;

namespace ProcessPayment.Domain
{
	public interface IPaymentGateway
	{
		bool CanProcessPayment(Payment payment);
		Task<PaymentStatus> HandlePayment(Payment payment);
		Task<PaymentStatus> ProcessPayment(Payment payment);
	}

	public abstract class PaymentGateway : IPaymentGateway
	{
		protected readonly IPaymentGateway NextGateway;

		protected PaymentGateway(PaymentGateway nextGateway)
		{
			NextGateway = nextGateway;
		}

		public async Task<PaymentStatus> ProcessPayment(Payment payment)
        {
			if (CanProcessPayment(payment))
			{
				return await HandlePayment(payment);
			}

			return await NextGateway.ProcessPayment(payment);
        }

		/// <inheritdoc />
		public abstract bool CanProcessPayment(Payment payment);

		/// <inheritdoc />
		public abstract Task<PaymentStatus> HandlePayment(Payment payment);
	}
}