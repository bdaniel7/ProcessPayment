using System.Threading.Tasks;
using ProcessPayment.Common;
using ProcessPayment.Model;

namespace ProcessPayment.Domain
{
	public interface ICheapPaymentGateway : IPaymentGateway { }

	public class CheapPaymentGateway : ICheapPaymentGateway
	{
		/// <inheritdoc />
		public async Task<PaymentStatus> ProcessPayment(Payment payment)
		{
			return await Task.FromResult(new PaymentStatus()
			{
				State = PaymentStateEnum.Processed,
				Message = $"Processed by {GetType().Name}"
			});
		}
	}
}