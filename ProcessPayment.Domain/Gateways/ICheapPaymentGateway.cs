using System.Threading.Tasks;
using ProcessPayment.Common;
using ProcessPayment.Model;

namespace ProcessPayment.Domain
{
	public interface ICheapPaymentGateway : IPaymentGateway { }

	public class CheapPaymentGateway : PaymentGateway, ICheapPaymentGateway
	{
		/// <inheritdoc />
		public CheapPaymentGateway(PaymentGateway nextGateway)
			: base(nextGateway)
		{ }

		/// <inheritdoc />
		public override bool CanProcessPayment(Payment payment)
		{
			return payment.Amount < 20;
		}

		/// <inheritdoc />
		public override async Task<PaymentStatus> ProcessPayment(Payment payment)
		{
			if (CanProcessPayment(payment))
			{
				return await Task.FromResult(new PaymentStatus()
				{
					State = PaymentStateEnum.Processed,
					Message = $"Processed by {GetType().Name}"
				});
			}

			return await nextGateway.ProcessPayment(payment);

		}
	}
}