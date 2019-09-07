using System.Threading.Tasks;
using ProcessPayment.Common;
using ProcessPayment.Model;

namespace ProcessPayment.Domain
{
	public interface IPaymentGateway
	{
		Task<PaymentStatus> ProcessPayment(Payment payment);
	}
}