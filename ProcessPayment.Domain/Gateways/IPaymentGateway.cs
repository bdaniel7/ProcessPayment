using System.Threading.Tasks;
using ProcessPayment.Common;
using ProcessPayment.Model;

namespace ProcessPayment.Domain
{
	public interface IPaymentGateway
	{
		bool CanProcessPayment(Payment payment);
		Task<PaymentStatus> ProcessPayment(Payment payment);
	}

	public abstract class PaymentGateway : IPaymentGateway
	{
		protected readonly IPaymentGateway nextGateway;

		protected PaymentGateway(PaymentGateway nextGateway)
		{
			this.nextGateway = nextGateway;
		}

		public async Task<PaymentStatus> StartProcessingPayment(Payment payment)
        {
			if (CanProcessPayment(payment))
			{
				return await ProcessPayment(payment);
			}

			return await nextGateway.ProcessPayment(payment);
        }

		/// <inheritdoc />
		public abstract bool CanProcessPayment(Payment payment);

		/// <inheritdoc />
		public abstract Task<PaymentStatus> ProcessPayment(Payment payment);
	}
}