namespace ProcessPayment.Domain
{
	public interface IGatewayProvider
	{
		IPaymentGateway GetPaymentGateway(decimal amount);
	}

	public class GatewayProvider : IGatewayProvider
	{
		/// <inheritdoc />
		public IPaymentGateway GetPaymentGateway(decimal amount)
		{
			if (amount < 20)
				return new CheapPaymentGateway();

			if (amount > 20 && amount < 500)
				return new ExpensivePaymentGateway();

			return new PremiumPaymentGateway();
		}
	}
}